using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using zkemkeeper;


namespace zkteco_cli.Components
{
    internal class ZKTekoDevice  
    {
		private static readonly ILog ZKTekoDeviceLoggger = LogManager.GetLogger(typeof(ZKTekoDevice));

		private int id = 0;
		public string ip = null;
		public int port = 0;
		private int password = 0;
		public string type = "zkteko";

		// Loading ZKTeko Class device
		private CZKEM zkteco = new CZKEM();

		public string Serial;

		public List<ZKTekoAttendance> attendances = new List<ZKTekoAttendance>();

        public ZKTekoDevice (ConnectionDevice dev)
        {
            this.SetId(dev.GetId());
            this.SetIp(dev.GetIp());
            this.SetPort(dev.GetPort());
            this.SetPassword(dev.GetPassword());
        }

		public ZKTekoDevice (int id, string ip, int port, int password)
        {
			this.id = id;
			this.ip	= ip;
			this.port = port;
			this.password = password;
        }

		public void SetId(int id)
		{
			this.id = id;
		}

		public int GetId()
		{
			return this.id;
		}

		public void SetIp(string ip)
		{
			this.ip = ip;
		}

		public string GetIp()
		{
			return this.ip;
		}

		public void SetPort(int port)
		{
			this.port = port;
		}

		public int GetPort()
		{
			return this.port;
		}

		public void SetPassword(int pass)
		{
			this.password = pass;
		}

		public int GetPassword()
		{
			return this.password;
		}

		private void SetSerial(string serial)
		{
			this.Serial = serial;

		}
		public string GetSerial()
		{
			return this.Serial;
		}
		public bool TestConnect(string ip, int port, int password)
		{
			bool IsConnected = false;

			ZKTekoDeviceLoggger.Info("Opening connection for testing");
			if (this.zkteco.SetCommPassword(this.GetPassword()))
			{
				ZKTekoDeviceLoggger.Info("Setting password");
				if (this.zkteco.Connect_Net(this.GetIp(), this.GetPort()))
				{
					ZKTekoDeviceLoggger.Info("Connection established");
					IsConnected = true;
					this.zkteco.Disconnect();
				}
				else
                {
					ZKTekoDeviceLoggger.Info("Connection error");
				}
			}
			
			return IsConnected;

		}
		public void ObtainSerial()
		{

			ZKTekoDeviceLoggger.Debug("Getting serial");
			ZKTekoDeviceLoggger.Debug("Setting password");
			bool hasPassword = zkteco.SetCommPassword(this.GetPassword());

			if (hasPassword)
			{
				ZKTekoDeviceLoggger.Debug("Opennig connection to " + this.GetIp() + ":" + this.GetPort().ToString());
				bool IsConnected = this.zkteco.Connect_Net(this.GetIp(), this.GetPort());

				if (IsConnected)
				{
					string serial;
					ZKTekoDeviceLoggger.Debug("Connected");
					bool hasSerial = this.zkteco.GetSerialNumber(this.GetId(), out serial);
					if (hasSerial)
					{
						this.SetSerial(serial);
						ZKTekoDeviceLoggger.Debug("Serial obtained: " + this.GetSerial());
					}
					else
					{
						ZKTekoDeviceLoggger.Error("Serial not obtained");
					}
					this.zkteco.Disconnect();
				}
				else
				{
					ZKTekoDeviceLoggger.Error("Connection error");
				}
			}
			else
			{
				ZKTekoDeviceLoggger.Error("Password not set, cannot connect");
			}


		}

	}
}
