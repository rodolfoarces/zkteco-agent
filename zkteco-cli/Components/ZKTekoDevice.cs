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

		public string Serial = null;

		public List<ZKTekoAttendance> attendances = new List<ZKTekoAttendance>();

		public ZKTekoDevice(ConnectionDevice dev)
		{
			this.SetId(dev.GetId());
			this.SetIp(dev.GetIp());
			this.SetPort(dev.GetPort());
			this.SetPassword(dev.GetPassword());
		}

		public ZKTekoDevice(int id, string ip, int port, int password)
		{
			this.id = id;
			this.ip = ip;
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
		public void ObtainAttendance()
		{
			ZKTekoDeviceLoggger.Debug("Getting attendance");
			ZKTekoDeviceLoggger.Debug("Setting password");
			bool hasPassword = this.zkteco.SetCommPassword(this.GetPassword());

			if (hasPassword)
			{
				ZKTekoDeviceLoggger.Debug("Opennig connection to " + this.GetIp() + ":" + this.GetPort().ToString());
				bool IsConnected = this.zkteco.Connect_Net(this.GetIp(), this.GetPort());

				if (IsConnected)
				{
					string serial;
					bool hasSerial = this.zkteco.GetSerialNumber(this.GetId(), out serial);
					if (hasSerial)
					{
						this.SetSerial(serial);
						ZKTekoDeviceLoggger.Debug("Serial obtained: " + this.GetSerial());
					}
					int dwMachineNumber = 1; // Machine ID (in)
					bool readLog = this.zkteco.ReadGeneralLogData(dwMachineNumber);
					ZKTekoDeviceLoggger.Debug("Reading log");

					if (readLog)
					{
						//Variables locales
						string dwEnrollNumber = string.Empty; // Pointer that points to the BSTR variable. Its value is the user ID of an attendance record. A user ID contains a maximum of 24 digits
						int dwVerifyMode = 0; // Pointer that points to the LONG variable. Its value is the verification mode of an attendance record.
						int dwInOutMode = 0; // Pointer that points to the LONG variable. Its value is the attendance status of an attendance record.
						int dwYear = 0; // Pointer that points to the LONG variable. Its value is the year of an attendance record.
						int dwMonth = 0; // Pointer that points to the LONG variable. Its value is the month of an attendance record. 
						int dwDay = 0; // Pointer that points to the LONG variable. Its value is the day of an attendance record.
						int dwHour = 0; // Pointer that points to the LONG variable. Its value is the hour of an attendance record.
						int dwMinute = 0; // Pointer that points to the LONG variable. Its value is the minute of an attendance record.
						int dwSecond = 0; // Pointer that points to the LONG variable. Its value is the seconds of an attendance record.
						int dwWorkCode = 0; // Pointer that points to the LONG variable. Its value is the work code of an attendance record.
						ZKTekoDeviceLoggger.Debug("Reading buffer");

						while (this.zkteco.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
						{
							ZKTekoDeviceLoggger.Debug("Creating attendance");
							ZKTekoAttendance attendance = new ZKTekoAttendance(this.GetSerial(), dwEnrollNumber, dwVerifyMode, dwInOutMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, dwWorkCode);
							ZKTekoDeviceLoggger.Debug("Adding attendance to List");
							attendances.Add(attendance);
							ZKTekoDeviceLoggger.Debug(attendance.ToString());
						}
					}
					else
					{
						ZKTekoDeviceLoggger.Error("Error reading log");

					}
					this.zkteco.Disconnect();
				}
				else
				{
					ZKTekoDeviceLoggger.Error("Error connecting to device");
				}
			}
			else
			{
				ZKTekoDeviceLoggger.Error("Error setting password");
			}
		}
	}
}
