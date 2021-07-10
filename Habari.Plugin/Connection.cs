using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace Habari.Plugin
{
    public abstract class Connection : IConnection
    {
        public bool AllowDelete { get; protected set; }

        public bool AllowGet { get; protected set; }

        public bool AllowPost { get; protected set; }

        public bool AllowPut { get; protected set; }

        public string MetadataUri { get; protected set; }

        public virtual string Type { get { return "Generic"; } }

        public string Uri { get; protected set; }

        public virtual IConnection DeepCopy()
        {
            IConnection result = (IConnection)this.MemberwiseClone();
            return result;
        }

        public virtual void LoadFromJToken(JToken connection)
        {
            AllowDelete = connection.Value<bool>("allowDelete");
            AllowGet = connection.Value<bool>("allowGet");
            AllowPost = connection.Value<bool>("allowPost");
            AllowPut = connection.Value<bool>("allowPut");
            MetadataUri = connection.Value<string>("metadataUri");
            Uri = connection.Value<string>("uri");
        }

        public virtual string MetadataContent()
        {
            return MetadataUri + "MetadataContent";
        }

        public virtual HttpStatusCode ProcessMetadata(HttpListenerContext context, ref object content)
        {
            HttpStatusCode result = HttpStatusCode.OK;
            byte[] metadataArray = Encoding.UTF8.GetBytes(MetadataContent());

            context.Response.OutputStream.WriteAsync(metadataArray, 0, metadataArray.Length);

            return result;
        }

        public virtual HttpStatusCode Process(HttpListenerContext context, ref object content)
        {
            return HttpStatusCode.OK;
        }
    }
}
