using System.Collections.Generic;

namespace zkteco_cli.API
{
    internal class ApiData
    {
        public string access_token { get; set; }
        public string refreshToken { get; set; }

        public ApiData(string access_tk,string refresh_tk)
        {
            this.access_token = access_tk;
            this.refreshToken = refresh_tk;
        }
        public ApiData()
        {
            this.access_token = string.Empty;
            this.refreshToken = string.Empty;

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
