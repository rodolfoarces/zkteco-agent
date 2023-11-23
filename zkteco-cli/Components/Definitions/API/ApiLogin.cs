using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace zkteco_cli.API
{
    internal class ApiLogin
    {
        public string username { get; set;  }

        public string password { get; set; }

        public string app {  get; set; }

        public ApiLogin(string username, string password, string app)
        {
            this.username = username;
            this.password = password;
            this.app = app;
        }

        public ApiLogin()
        {
            this.username = string.Empty;
            this.password = string.Empty;
            this.app = string.Empty;
        }

        public string GetUsername ()
        {
            return this.username;
        }

        public string GetPassword ()
        {
            return this.password;
        }

        public string GetApp() 
        { 
            return this.app; 
        }

        public override string ToString()
        {
            string obj = "Username: " + this.GetUsername() + " ";
            obj += "Application: " + this.GetApp() + " ";
            return obj;
        }


    }
}
