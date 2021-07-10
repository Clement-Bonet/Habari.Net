using Newtonsoft.Json.Linq;
using System.Net;

namespace Habari.Plugin
{
    public interface IConnection
    {
        bool AllowDelete { get; }

        bool AllowGet { get; }

        bool AllowPost { get; }

        bool AllowPut { get; }

        string MetadataUri { get; }

        string Type { get; }

        string Uri { get; }

        IConnection DeepCopy();

        void LoadFromJToken(JToken connection);

        HttpStatusCode ProcessMetadata(HttpListenerContext context, ref object content);

        HttpStatusCode Process(HttpListenerContext context, ref object content);
    }
}
