using Habari.PluginSystem;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Habari.Library
{
    public sealed class ConfigurationManager
    {
        private static readonly Lazy<ConfigurationManager> lazy = new Lazy<ConfigurationManager>(() => new ConfigurationManager());

        public static ConfigurationManager Instance { get => lazy.Value; }

        public HttpListener Listener { get => new HttpListener(); }

        public Dictionary<string, Type> AvailablePluginsConnections { get; private set; } = new Dictionary<string, Type>();

        public List<Configuration> Configurations { get; } = new List<Configuration>();

        public void GenerateBlankConfig(string path)
        {

        }

        public void LoadConfiguration(string configurationFilename)
        {
            JToken configurationFile = JObject.Parse(File.ReadAllText(configurationFilename))["configurations"];
            foreach (JToken configurationToken in configurationFile.Children())
            {
                Configuration configuration = new Configuration();
                configuration.LoadFromJToken(configurationToken);
                foreach (JToken server in configurationToken["servers"].Children())
                {
                    Server srv = new Server();
                    srv.LoadFromJToken(server);
                    configuration.Servers.Add(srv);
                }
                foreach (JToken connection in configurationToken["connections"].Children())
                {
                    if (!configuration.AvailablePlugins.ContainsKey(connection.Value<string>("connectionType")))
                    {
                        throw new Exception("The plugin {connection.Value<string>(\"connectionType\")} is not available.");
                    }

                    Assembly assembly = LoadPlugin(configuration.AvailablePlugins[connection.Value<string>("connectionType")]);
                    if (assembly != null)
                    {
                        IEnumerable<IPlugin> plugins = CreatePlugins(assembly);
                        foreach (var plugin in plugins)
                        {
                            plugin.Configure(connection, configuration.DefaultPageSize, configuration.AllowDelete, configuration.AllowGet, configuration.AllowPost, configuration.AllowPut);
                            configuration.Plugins.Add(plugin);
                        }
                    }
                }
                Configurations.Add(configuration);
            }
        }

        private Assembly LoadPlugin(string pluginLocation)
        {
            if (!File.Exists(pluginLocation))
            {
                return null;
            }

            Console.WriteLine($"Loading plugins from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginLocation));
        }

        private IEnumerable<IPlugin> CreatePlugins(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(type))
                {
                    IPlugin result = Activator.CreateInstance(type) as IPlugin;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements IPlugin in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}
