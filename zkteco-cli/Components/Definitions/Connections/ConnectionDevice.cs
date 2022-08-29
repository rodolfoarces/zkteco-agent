namespace zkteco_cli.Connections
{
	internal class ConnectionDevice
	{
		public int id { get; set; }
		public string ip { get; set; }
		public int port { get; set; }
		public int password { get; set; }

		public ConnectionDevice(int id, string ip, int port, int password)
		{
			this.id = id;
			this.ip = ip;
			this.port = port;
			this.password = password;
		}

		public ConnectionDevice()
		{
		}

		public int GetId()
		{
			return this.id;
		}

		public string GetIp()
		{
			return this.ip;
		}

		public int GetPort()
		{
			return this.port;
		}

		public int GetPassword()
        {
			return this.password;
        }

        public override string ToString()
        {
			string obj = " ConnectionDevice";
			obj = obj + " id: " + this.id.ToString();
			obj = obj + " ip: " + this.ip;
			obj = obj + " port: " + this.port.ToString();
			obj = obj + " password: " + this.password.ToString();

			return obj;
		}
    }

}
