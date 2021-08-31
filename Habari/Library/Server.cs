using Habari.PluginSystem;
using Newtonsoft.Json.Linq;

namespace Habari.Library
{
    public class Server
    {
        public string Host { get; private set; }

        public string MetadataUri { get; private set; }

        public int Port { get; private set; }

        public string Uri { get; private set; }

        public bool Secured { get; private set; }

        public string HttpPrefix { get => "http" + (Secured ? "s" : string.Empty) + "://" + Host + ":" + Port + "/"; }

        public void LoadFromJToken(JToken token)
        {
            Host = token.Value<string>("host");
            MetadataUri = token.Value<string>("metadataUri");
            Port = token.Value<int>("port");
            Uri = token.Value<string>("uri");
            Secured = token.Value<bool>("secured");
        }
    }
}
