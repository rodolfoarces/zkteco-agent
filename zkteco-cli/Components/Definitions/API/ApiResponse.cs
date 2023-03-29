namespace zkteco_cli.API
{
    internal class ApiResponse
    {
        bool success { get; set; }

        public ApiData data { get; set; }

        string errors { get; set; }
        public ApiResponse()
        {

        }

        public bool GetSuccess()
        {
            return success;
        }

        public ApiData GetData()
        {
            return data;
        }

        public string GetErrors()
        {
            return errors;
        }
        public override string ToString()
        {
            string obj = "Data: " + this.data.ToString() +  " ";
            obj += "Success: " + this.success.ToString() + " ";
            return obj;
        }

    }
}

    
