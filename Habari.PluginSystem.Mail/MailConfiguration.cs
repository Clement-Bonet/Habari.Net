using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Authentication;
using System.Text;

namespace Habari.PluginSystem.Mail
{
    public class MailConfiguration : IConfiguration
    {
        public bool AllowDelete => false;

        public bool AllowGet => true;

        public bool AllowPost => false;

        public bool AllowPut => false;

        public bool Asynchronous { get; private set; }

        public string Body { get; private set; }

        public List<MailboxAddress> Bccs { get; } = new List<MailboxAddress>();

        public List<MailboxAddress> Ccs { get; } = new List<MailboxAddress>();

        public bool EnableSsl { get; private set; }

        public MailboxAddress From { get; private set; }

        public string Host { get; private set; }

        public string Login { get; private set; }

        public bool IsBodyHtml { get; private set; }

        public string MetadataUri => string.Empty;

        public string Password { get; private set; }

        public int Port { get; private set; }

        public MailboxAddress ReplyTo { get; private set; }

        public string Subject { get; private set; }

        public SslProtocols SslProtocols { get; private set; }

        public List<MailboxAddress> Tos { get; } = new List<MailboxAddress>();

        public string Type => "Mail";

        public string RestUri { get; protected set; }

        public void LoadFromJToken(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            Asynchronous = token.Value<bool>("asynchronous");
            Body = token.Value<string>("body");
            From = CastToMailboxAddress(token["from"]);
            Host = token.Value<string>("host");
            IsBodyHtml = token.Value<bool>("isBodyHtml");
            Login = token.Value<string>("login");
            EnableSsl = token.Value<bool>("enableSsl");
            Password = token.Value<string>("password");
            Port = token.Value<int>("port");
            ReplyTo = CastToMailboxAddress(token["replyTo"]);
            RestUri = token.Value<string>("restUri");
            Subject = token.Value<string>("subject");
            SslProtocols = SslProtocols.None;
            foreach (JToken sslProtocol in token["sslProtocols"].Children())
            {
                SslProtocols |= CastToSslProtocol(sslProtocol);
            }
            foreach (JToken address in token["cc"].Children())
            {
                Ccs.Add(CastToMailboxAddress(address));
            }
            foreach (JToken address in token["bcc"].Children())
            {
                Bccs.Add(CastToMailboxAddress(address));
            }
            foreach (JToken address in token["to"].Children())
            {
                Tos.Add(CastToMailboxAddress(address));
            }
        }

        private SslProtocols CastToSslProtocol(JToken token)
        {
            SslProtocols sslProtocols;
            if(token is JProperty && ((JProperty)token).Value is JValue && ((JValue)((JProperty)token).Value).Value<bool>())
            {
                sslProtocols = (SslProtocols)Enum.Parse(SslProtocols.GetType(), ((JProperty)token).Name, true);
            }
            else if (token is JValue)
            {
                sslProtocols = (SslProtocols)Enum.Parse(SslProtocols.GetType(), (string)token, true);
            }
            else
            {
                throw new Exception("Error in the configuration");
            }
            return sslProtocols;
        }

        private MailboxAddress CastToMailboxAddress(JToken token)
        {
            MailboxAddress mailboxAddress;
            if (token is JObject)
            {
                mailboxAddress = new MailboxAddress(Encoding.UTF8, token.Value<string>("name"), token.Value<string>("address"));
            }
            else if (token is JValue)
            {
                mailboxAddress = MailboxAddress.Parse((string)token);
            }
            else
            {
                throw new Exception("Error in the configuration");
            }
            return mailboxAddress;
        }
    }
}
