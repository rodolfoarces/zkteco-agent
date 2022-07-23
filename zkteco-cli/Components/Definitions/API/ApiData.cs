using System.Collections.Generic;

namespace zkteco_cli.API
{
    internal class ApiData
    {
        string access_token { get; set; }
        string refreshToken { get; set; }

        public ApiData()
        {

        }

        public string GetAccessToken()
        {
            return access_token;
        }
        public string GetRefreshToken()
        {
            return refreshToken;
        }
    }


}
