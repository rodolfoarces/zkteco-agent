using System.Collections.Generic;

namespace zkteco_cli.API
{
    internal class ApiResponse
    {
        bool success { get; set; }

        ApiData data { get; set; }

        string errors { get; set; }
        public ApiResponse()
        {

        }

        public bool GetSuccess()
        {
            return success;
        }

        public ApiData GetData ()
        {
            return data;
        }

        public string GetErrors()
        {
            return errors;
        }


    }

    
}
