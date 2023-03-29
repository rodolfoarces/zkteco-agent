using System.Collections.Generic;

namespace zkteco_cli.API
{
    internal class ApiData
    {
        public string access_token { get; set; }
        public string refreshToken { get; set; }

        public ApiData()
        {

        }

        public string GetAccessToken()
        {
            return this.access_token;
        }
        public string GetRefreshToken()
        {
            return this.refreshToken;
        }

        public override string ToString()
        {
            string obj = "Access Token: " + this.access_token.ToString() + " ";
            obj += "Refresh Token: " + this.refreshToken.ToString() + " ";
            return obj;
        }
    }
}
