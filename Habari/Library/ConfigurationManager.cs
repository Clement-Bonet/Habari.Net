using Habari.Plugin;
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

        public string PluginDirectory { get => "plugins"; }

        public Dictionary<string, Type> AvailablePluginsConnections { get; private set; } = new Dictionary<string, Type>();

        public List<Configuration> Configurations { get; } = new List<Configuration>();

        public void GenerateBlankConfig(string path)
        {

        }

        public void LoadConfiguration(string configurationFilename)
        {
            if (AvailablePluginsConnections == null || !AvailablePluginsConnections.Any())
            {
                LoadPlugins();
            }

            JToken configurationFile = JObject.Parse(File.ReadAllText(configurationFilename))["configurations"];
            foreach (JToken configurationToken in configurationFile.Children())
            {
                Configuration configuration = new Configuration();
                foreach(JToken server in configurationToken["servers"].Children())
                {
                    Server srv = new Server();
                    srv.LoadFromJToken(server);
                    configuration.Servers.Add(srv);
                }
                foreach (JToken connection in configurationToken["connections"].Children())
                {
                    Type type = AvailablePluginsConnections.FirstOrDefault(item => item.Key.Equals(connection.Value<string>("connectionType"), StringComparison.InvariantCultureIgnoreCase)).Value;
                    if (type != null)
                    {
                        IConnection cnn = (IConnection)Activator.CreateInstance(type);
                        cnn.LoadFromJToken(connection);
                        configuration.Connections.Add(cnn);
                    }
                }
                Configurations.Add(configuration);
            }
        }

        private void LoadPlugins()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginDirectory));
            }

            Dictionary<string, Type> availableConnections = (
                    from file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginDirectory))
                    from type in Assembly.LoadFrom(file).GetTypes()
                    where type.IsPublic
                    where type.GetInterfaces().Contains(typeof(IConnection))
                    select new KeyValuePair<string, Type>(((IConnection)Activator.CreateInstance(type)).Type, type)
                ).ToDictionary(element => element.Key, element => element.Value);

            AvailablePluginsConnections = availableConnections;
        }
    }
}
