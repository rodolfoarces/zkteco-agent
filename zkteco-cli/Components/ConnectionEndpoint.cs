using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkteco_cli.Components
{
    internal class ConnectionEndpoint
    {
        public string url { get; set; }
        public string token { get; set; }

        public ConnectionEndpoint()
        {
            // Somthing odd
        }

        public ConnectionEndpoint(string url, string token)
        {
            this.url = url;
            this.token = token;
        }

        public override string ToString()
        {
            string obj = "URL: " + this.url.ToString() + " ";
            obj += "Token: " + this.token.ToString();
            return obj;
        }

    }
}
