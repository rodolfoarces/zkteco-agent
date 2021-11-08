using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using zkemkeeper;


namespace agent_cli.Components
{
	class Device
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Device));
		public int Id { get; set; }
		public string Name { get; set; }
		public string UID { get; set; }
		public string Host = "192.168.1.201";
		public int Port = 4370;
		public int Password = 0;
		public string MAC = string.Empty;
		private string Serial = string.Empty;
		private bool IsConnected = false;

		// Loading ZKTeko Class device
		private CZKEM zkteko = new CZKEM();

		public bool TestConnect(string ip, int port, int password)
		{
			Console.WriteLine("Opening connection for testing");
			if (this.zkteko.SetCommPassword(password))
			{
				log.Info("Setting password");
				this.IsConnected = true;
			}

			if (this.zkteko.Connect_Net(ip, port))
			{
				log.Info("Connection established");
				this.IsConnected = true;
				this.zkteko.Disconnect();
			}
			else
            {
				log.Error("Connection failed");
				this.IsConnected = false;
			}
			return this.IsConnected;
			
		}

		public void Disconnect()
		{
			if (this.IsConnected)
			{
				this.zkteko.Disconnect();
				this.IsConnected = false;
			}
		}

		public string GetSerial()
		{
			log.Debug("Getting serial");
			log.Debug("Setting password");
			bool hasPassword = zkteko.SetCommPassword(this.Password);

			if (hasPassword)
            {
				log.Debug("Opennig connection");
				this.IsConnected = this.zkteko.Connect_Net(this.Host, this.Port);
			}
			else
            {
				log.Error("Password not set, cannot connect");
				this.IsConnected = false;
			}
			
			if (this.IsConnected)
			{
				log.Debug("Connected");
				bool hasSerial = this.zkteko.GetSerialNumber(0, out this.Serial);
				if (hasSerial)
                {
					log.Debug("Serial obtained");
				}
				else
                {
					log.Error("Serial not obtained");
				}
				this.zkteko.Disconnect();
			}
			else
			{
				log.Error("Connection error");
			}

			return this.Serial;

		}

		public override string ToString() 
		{
			string toString = "ID:" + this.Id.ToString() + " , ";
			toString += "Name:" + this.Name.ToString() + " , ";
			toString += "UID:" + this.UID.ToString() + " , ";
			toString += "Host:" + this.Host.ToString() + " , ";
			toString += "Port:" + this.Port.ToString() + " , ";
			toString += "MAC:" + this.MAC.ToString();

			return toString;
		}
	}
}