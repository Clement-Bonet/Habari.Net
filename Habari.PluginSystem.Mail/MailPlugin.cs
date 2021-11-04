using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Authentication;

namespace Habari.PluginSystem.Mail
{
    public class MailPlugin : IPlugin
    {
        public IConfiguration Configuration { get; protected set; }

        public bool Configure(JToken token, int defaultPageSize, bool allowDelete, bool allowGet, bool allowPost, bool allowPut)
        {
            Configuration = new MailConfiguration();
            Configuration.LoadFromJToken(token, defaultPageSize, allowDelete, allowGet, allowPost, allowPut);
            return true;
        }

        public bool Process(HttpListenerContext context, out HttpStatusCode statusCode, ref string contentType, ref object content)
        {
            if (context.Request.HttpMethod.ToUpper() == "GET")
            {
                MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(((MailConfiguration)Configuration).From);
                foreach (var mailAddress in ((MailConfiguration)Configuration).Bccs)
                {
                    mailAddress.Name = ReplaceQueryString(context.Request.QueryString, mailAddress.Name);
                    mailAddress.Address = ReplaceQueryString(context.Request.QueryString, mailAddress.Address);
                    mimeMessage.Bcc.Add(mailAddress);
                }

                foreach (var mailAddress in ((MailConfiguration)Configuration).Ccs)
                {
                    mailAddress.Name = ReplaceQueryString(context.Request.QueryString, mailAddress.Name);
                    mailAddress.Address = ReplaceQueryString(context.Request.QueryString, mailAddress.Address);
                    mimeMessage.Cc.Add(mailAddress);
                }

                foreach (var mailAddress in ((MailConfiguration)Configuration).Tos)
                {
                    mailAddress.Name = ReplaceQueryString(context.Request.QueryString, mailAddress.Name);
                    mailAddress.Address = ReplaceQueryString(context.Request.QueryString, mailAddress.Address);
                    mimeMessage.To.Add(mailAddress);
                }
                mimeMessage.Subject = ReplaceQueryString(context.Request.QueryString, ((MailConfiguration)Configuration).Subject);
                TextFormat testPartType = ((MailConfiguration)Configuration).IsBodyHtml ? TextFormat.Html : TextFormat.Plain;
                mimeMessage.Body = new TextPart(testPartType) { Text = ReplaceQueryString(context.Request.QueryString, ((MailConfiguration)Configuration).Body) };

                //SmtpClient smtpClient = new SmtpClient(((MailConfiguration)Configuration).Host, ((MailConfiguration)Configuration).Port);
                using (SmtpClient client = new SmtpClient())
                {
                    client.SslProtocols = ((MailConfiguration)Configuration).SslProtocols;
                    client.Connect(((MailConfiguration)Configuration).Host, ((MailConfiguration)Configuration).Port, SecureSocketOptions.Auto);
                    client.Authenticate(((MailConfiguration)Configuration).Login, ((MailConfiguration)Configuration).Password);
                    if (((MailConfiguration)Configuration).Asynchronous)
                    {
                        client.SendAsync(mimeMessage);
                    }
                    else
                    {
                        client.Send(mimeMessage);
                    }
                    client.Disconnect(true);
                }
                statusCode = HttpStatusCode.OK;
                return true;
            }

            statusCode = HttpStatusCode.NotFound;
            return false;
        }

        private string ReplaceQueryString(NameValueCollection queryString, string str)
        {
            foreach (string item in queryString)
            {
                str = str.Replace("{" + item + "}", queryString[item], StringComparison.InvariantCultureIgnoreCase);
            }

            return str;
        }
    }
}
