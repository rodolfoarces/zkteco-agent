namespace zkteco_cli.API
{
    internal class ApiResponse
    {
        bool success { get; set; }

        public ApiData data { get; set; }

        string errors { get; set; }
        public ApiResponse()
        {
            this.success = false;
            this.errors = string.Empty;
            this.data = new ApiData(string.Empty,string.Empty);

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
            string obj = "Success: " + this.success.ToString() + " "; 
            obj += "Data: " + this.data.ToString() + " ";
            return obj;
        }

    }
}

    
