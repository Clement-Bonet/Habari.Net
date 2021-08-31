using Habari.Library;
using Habari.PluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Habari
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationManager.Instance.LoadConfiguration(args.Any() ? args[0] : "config.json");

            foreach (var item in ConfigurationManager.Instance.AvailablePluginsConnections)
            {
                Console.WriteLine("type: " + item.Key);
            }

            Console.Title = ConfigurationManager.Instance.Configurations.First().Servers.First().HttpPrefix;
            ConfigurationManager.Instance.Configurations.ForEach(c => c.ConfigureAndStartListener());
        }
    }
}
