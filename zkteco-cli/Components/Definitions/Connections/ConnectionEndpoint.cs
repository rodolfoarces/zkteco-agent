using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkteco_cli.Connections
{
    internal class ConnectionEndpoint
    {
        public string ip { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public ConnectionEndpoint()
        {
            // Somthing odd
        }

        public ConnectionEndpoint(string url, string username, string password, string application)
        {
            this.ip = url;
            this.username = username;
            this.password = password;
        }
        public override string ToString()
        {
            string obj = "URL: " + this.ip.ToString() + " ";
            obj += "Username: " + this.username.ToString() + " ";
            obj += "Password: " + this.password.ToString() + " ";
            return obj;
        }

    }
}
