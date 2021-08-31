using Newtonsoft.Json.Linq;
using System.Net;

namespace Habari.PluginSystem
{
    public interface IPlugin
    {
        IConfiguration Configuration { get; }

        bool Configure(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut);

        bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref object content);

        bool ProcessMetadata(HttpListenerContext context, out HttpStatusCode statusCode, ref object content);
    }
}
