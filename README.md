# Habari.Net

> A configuration-driven HTTP gateway for .NET that exposes relational databases (MySQL, PostgreSQL, Oracle, SQL Server) through REST/OData-style endpoints, with a plugin system to add new behaviours (CSV export, e-mail, templates...).

**Status: proof of concept (2021).** Read-only database access works; write operations and the template plugin are stubs. See [Status](#status).

---

## About

Habari.Net is a small server built on `HttpListener`. Instead of writing one API per database, you describe your connections and endpoints in a JSON file (`config.json`). Each incoming request is matched against the `restUri` regular expression of every configured connection and handed to the matching **plugin**.

Plugins are separate assemblies, loaded at startup through an `AssemblyLoadContext` (`PluginLoadContext`), so they can be developed, built and deployed independently of the host.

## Architecture

```
HTTP request
     │
 Habari (host)  ── reads config.json, loads plugin DLLs
     │
     ├─ match request URL against each connection's `restUri` (regex)
     │
     ▼
 IPlugin.Process(...)   ← Habari.PluginSystem (shared contract)
     ├─ Habari.PluginSystem.Database  → MySQL / PostgreSQL / Oracle / SQL Server
     ├─ Habari.PluginSystem.OData     → OData connection
     ├─ Habari.PluginSystem.ToCsv     → CSV output (`?format=csv`)
     ├─ Habari.PluginSystem.Mail      → send an e-mail from an endpoint (MailKit)
     └─ Habari.PluginSystem.Template  → templating (not implemented yet)
```

| Project | Role |
|---|---|
| `Habari` | Host: configuration, HTTP listener, plugin loading |
| `Habari.PluginSystem` | Shared contract (`IPlugin`, `IConfiguration`) |
| `Habari.PluginSystem.Database` | Schema discovery (tables, views, procedures, packages, indexes, keys) and `SELECT` with `$select` |
| `Habari.PluginSystem.ToCsv` | Returns results as `text/csv` (CsvHelper) |
| `Habari.PluginSystem.Mail` | Sends an e-mail through SMTP (MailKit) |
| `Habari.PluginSystem.OData` | OData connection |
| `Habari.PluginSystem.Template` | Template plugin (stub) |

## Getting started

**Prerequisites:** .NET SDK (the projects currently target `net5`; upgrading to a supported version such as .NET 8 is on the to-do list) and a reachable database.

```bash
git clone https://github.com/Csluikidikilest/Habari.Net.git
cd Habari.Net
dotnet build Habari.sln
```

1. Copy `Habari/config.example.json` to `Habari/config.json` and adapt it (see below). `config.json` is git-ignored so your credentials stay local.
2. Set each plugin `path` to the built DLL on your machine.
3. Start the server:

```bash
dotnet run --project Habari -- Habari/config.json
```

By default the server listens on `http://127.0.0.1:9000/`.

## Configuration

```jsonc
{
  "configurations": [{
    "allowSelect": true,
    "allowInsert": false,
    "allowUpdate": false,
    "allowDelete": false,
    "availablePlugins": [
      { "connectionType": "Habari.PluginSystem.Database",
        "path": "<path>/Habari.PluginSystem.Database.dll" }
    ],
    "servers": [{ "host": "127.0.0.1", "port": 9000, "secured": false, "uri": "/" }],
    "connections": [{
      "connectionType": "Habari.PluginSystem.Database",
      "restUri": "^/api/mydb",
      "databaseEngine": "mysql",          // mysql | oracle | postgresql | sqlserver
      "host": "<db-host>", "port": 3306,
      "databaseName": "<database>", "schemaName": "<schema>",
      "login": "<user>", "password": "<password>",
      "maxSimultaneousConnections": 10,
      "elements": [{
        "objectName": "<table-or-view>",
        "objectType": "TABLE|VIEW",
        "restUri": "/{objectName}",
        "defaultPageSize": 50
      }]
    }]
  }]
}
```

> **Never commit real credentials.** `config.json` is git-ignored; only `config.example.json` (placeholders) is versioned.

Requests are routed by the `restUri` regular expressions. Adding `?format=csv` can be combined with the `ToCsv` connection to export results as CSV.

## Status

| Feature | State |
|---|---|
| `SELECT` on tables / views (with `$select`) | Working |
| Schema discovery (tables, views, procedures, packages...) | Working |
| CSV export | Working |
| E-mail sending | Working |
| `POST` / `PUT` / `DELETE` | Not implemented (stubs) |
| Template plugin | Not implemented |
| Authentication | Not implemented |

## Roadmap

- [ ] Upgrade to a supported .NET version
- [ ] Implement write operations behind the `allowInsert` / `allowUpdate` / `allowDelete` flags
- [ ] Authentication and authorisation
- [ ] Parameterised queries and unit tests
- [ ] Replace absolute plugin paths with a relative `plugins/` folder
- [ ] Replace the legacy `Ssl2` / `Ssl3` defaults of the mail plugin with TLS 1.2+ only (already the case in `config.example.json`)

## License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.
