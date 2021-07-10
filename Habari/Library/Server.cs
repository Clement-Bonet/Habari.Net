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

        public void LoadFromJToken(JToken server)
        {
            Host = server.Value<string>("host");
            MetadataUri = server.Value<string>("metadataUri");
            Port = server.Value<int>("port");
            Uri = server.Value<string>("uri");
            Secured = server.Value<bool>("secured");
        }
    }
}
