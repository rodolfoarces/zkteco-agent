using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkteco_cli.Connections
{
    internal class ConnectionEndpoint
    {
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string application { get; set; }

        public ConnectionEndpoint()
        {
            // Somthing odd
        }

        public ConnectionEndpoint(string url, string username, string password, string application)
        {
            this.url = url;
            this.username = username;
            this.password = password;
            this.application = application;
        }
        public override string ToString()
        {
            string obj = "URL: " + this.url.ToString() + " ";
            obj += "Username: " + this.username.ToString() + " ";
            obj += "Password: " + this.password.ToString() + " ";
            obj += "Application: " + this.application.ToString();
            return obj;
        }

    }
}
