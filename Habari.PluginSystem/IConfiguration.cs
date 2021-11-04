using Newtonsoft.Json.Linq;

namespace Habari.PluginSystem
{
    public interface IConfiguration
    {
        bool AllowDelete { get; }

        bool AllowGet { get; }

        bool AllowPost { get; }

        bool AllowPut { get; }

        string MetadataUri { get; }

        string Type { get; }

        string RestUri { get; }

        void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut);
    }
}
