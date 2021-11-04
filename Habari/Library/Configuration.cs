using Habari.PluginSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Habari.Library
{
    public class Configuration
    {
        public bool AllowDelete { get; protected set; }

        public bool AllowGet { get; protected set; }

        public bool AllowPost { get; protected set; }

        public bool AllowPut { get; protected set; }

        public Dictionary<string, string> AvailablePlugins { get; } = new Dictionary<string, string>();

        public int DefaultPageSize { get; protected set; }

        public List<IPlugin> Plugins { get; } = new List<IPlugin>();

        public List<Server> Servers { get; } = new List<Server>();

        public List<IPlugin> GetConnectionsFromRest(string userHostAddress, string uri) => (from server in Servers
                                                                                            from plugin in Plugins
                                                                                                // where uri.StartsWith(Path.Combine(server.Uri, plugin.Configuration.RestUri))
                                                                                            where new Regex(plugin.Configuration.RestUri, RegexOptions.IgnoreCase).Matches(uri).Any()
                                                                                            where server.Host + ":" + server.Port == userHostAddress
                                                                                            select plugin).ToList();

        //public List<IPlugin> GetConnectionsFromMetadata(string uri) => (from server in Servers
        //                                                                from plugin in Plugins
        //                                                                where uri.StartsWith(Path.Combine(server.Uri, plugin.Configuration.MetadataUri))
        //                                                                select plugin).ToList();

        private HttpListener Listener { get; } = new HttpListener();

        private volatile bool _keepGoing = true;

        public void ConfigureAndStartListener()
        {
            ConfigureListener();
            StartListener();
        }

        public void ConfigureListener()
        {
            Listener.Prefixes.Clear();
            foreach (Server server in Servers)
            {
                Listener.Prefixes.Add(server.HttpPrefix);
            }
        }

        public void LoadFromJToken(JToken token)
        {
            AllowDelete = token.ValueOrDefault("allowDelete", false);
            AllowGet = token.ValueOrDefault("allowSelect", true);
            AllowPost = token.ValueOrDefault("allowInsert", false);
            AllowPut = token.ValueOrDefault("allowUpdate", false);
            DefaultPageSize = token.ValueOrDefault("defaultPageSize", 0);
            foreach (JToken plugin in token["availablePlugins"].Children())
            {
                AvailablePlugins.Add(plugin.Value<string>("connectionType"), plugin.Value<string>("path"));
            }
        }

        public void StartListener()
        {
            ListenerLoop();
        }

        public void StopListener()
        {
            _keepGoing = false;
            lock (Listener)
            {
                Listener.Stop();
            }
        }

        private void ListenerLoop()
        {
            Listener.Start();
            while (_keepGoing)
            {
                try
                {
                    //GetContextAsync() returns when a new request come in
                    IAsyncResult result = Listener.BeginGetContext(new AsyncCallback(ListenerCallback), Listener);
                    lock (Listener)
                    {
                        if (_keepGoing)
                        {
                            result.AsyncWaitHandle.WaitOne();
                        }
                    }
                }
                catch (Exception e)
                {
                    string m = e.ToString();
                    if (e is HttpListenerException)
                    {
                        return;
                    }
                }
            }
            StopListener();
        }

        private void ListenerCallback(IAsyncResult asyncResult)
        {
            if (_keepGoing)
            {
                HttpListener listener = (HttpListener)asyncResult.AsyncState;
                HttpListenerContext context = listener.EndGetContext(asyncResult);
                ComputeUri(context);
            }
        }

        private void ComputeUri(HttpListenerContext context)
        {
            List<IPlugin> pluginsRest = GetConnectionsFromRest(context.Request.UserHostAddress, context.Request.RawUrl);
            //List<IPlugin> pluginsMetadata = GetConnectionsFromMetadata(context.Request.RawUrl);
            string contentType = string.Empty;
            object content = null;
            bool handled = true;
            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            using (var response = context.Response)
            {
                response.Headers.Add("Server", Assembly.GetExecutingAssembly().GetName().Name + " version " + Assembly.GetExecutingAssembly().GetName().Version);
                try
                {
                    foreach (IPlugin pluginRest in pluginsRest)
                    {
                        handled &= pluginRest.Process(context, out statusCode, ref contentType, ref content);
                    }
                    //foreach (IPlugin pluginMetadata in pluginsMetadata)
                    //{
                    //    handled &= pluginMetadata.ProcessMetadata(context, out statusCode, ref contentType, ref content);
                    //}

                    response.StatusCode = (int)statusCode;
                    response.ContentType = contentType + "; charset=utf-8";
                    if (content != null)
                    {
                        var buffer = Encoding.UTF8.GetBytes(content.ToString());
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                }
                catch (Exception e)
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e));
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                finally
                {
                    response.OutputStream.Close();
                }
            }
        }
    }
}
