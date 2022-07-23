using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkteco_cli.Connections;

namespace zkteco_cli.API
{
    internal class ApiEndpoint
    {
        string username { get; set; }
        string password { get; set; } 
        string application { get; set; }
        string login_url { get; set; }
        string upload_url { get; set; }

        ApiResponse apiresponse { get; set; }

        public ApiEndpoint(ConnectionEndpoint endpoint)
        {


        }
        public string GetUsername()
        {
            return username;
        }
        public void SetUsername(string usr)
        {
            this.username = usr;
        }
        public string GetPassword()
        {
            return password;
        }
        public void SetPassword(string pass)
        {
            this.password = pass;
        }
        public string GetLoginURL()
        {
            return login_url;
        }
        public void SetLoginURL(string URL)
        {
            this.login_url = URL;
        }
        public string GetUploadURL()
        {
            return upload_url;
        }
        public void SetUploadURL(string URL)
        {
            this.upload_url = URL;
        }
        public string GetApplication()
        {
            return application;
        }
        public ApiResponse GetApiResponse()
        {
            return apiresponse;
        }
        public void SetApiResponse(ApiResponse response)
        {
            this.apiresponse = response;
        }
    }
}
