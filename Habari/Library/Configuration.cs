using Habari.Plugin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Habari.Library
{
    public class Configuration
    {
        public List<Server> Servers { get; } = new List<Server>();

        public List<IConnection> Connections { get; } = new List<IConnection>();

        public List<IConnection> GetConnectionsFromUri(string uri) => (from server in Servers
                                                                       from connection in Connections
                                                                       where uri.StartsWith(Path.Combine(server.Uri, connection.Uri))
                                                                       select connection).ToList();

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
            List<IConnection> connections = GetConnectionsFromUri(context.Request.RawUrl);
            object content = null;
            using (var response = context.Response)
            {
                try
                {
                    foreach (IConnection connection in connections)
                    {
                        response.StatusCode = connection.Process(context, ref content);
                    }
                    if (!handled)
                    {
                        response.StatusCode = 404;
                    }
                }
                catch (Exception e)
                {
                    response.StatusCode = 500;
                    response.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e));
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
