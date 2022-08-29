namespace zkteco_cli.API
{
    internal class ApiEndpoint
    {
        public long username { get; set; }
        public string password { get; set; }
        public string application { get; set; }
        public string login_uri { get; set; }
        public string upload_uri { get; set; }

        public ApiEndpoint(string login_uri, string upload_uri,long username,string password,string application)
        {
            this.username = username;
            this.password = password;
            this.application = application;
            this.login_uri = login_uri;
            this.upload_uri = upload_uri;
        }

        public ApiEndpoint()
        {
        }
        public long GetUsername()
        {
            return username;
        }
        public string GetPassword()
        {
            return password;
        }

        public string GetLoginURL()
        {
            return login_uri;
        }
 
        public string GetUploadURL()
        {
            return upload_uri;
        }

        public string GetApplication()
        {
            return application;
        }

        public override string ToString()
        {
            string obj = "URL: " + this.login_uri + " ";
            obj += "Upload URL: " + this.upload_uri + " ";
            obj += "Username: " + this.username.ToString()+ " ";
            obj += "Password: " + this.password + " ";
            obj += "Application: " + this.application + " ";
            return obj;
        }
    }
}
