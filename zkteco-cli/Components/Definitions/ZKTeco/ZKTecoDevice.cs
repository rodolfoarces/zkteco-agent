using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using zkemkeeper;
using zkteco_cli.Connections;


namespace zkteco_cli.ZKTeco
{
	internal class ZKTecoDevice
	{
		private static readonly ILog ZKTecoDeviceLoggger = LogManager.GetLogger(typeof(ZKTecoDevice));

		private int id = 0;
		public string ip = null;
		public int port = 0;
		private int password = 0;
		public string type = "zkteco";

		// Loading ZKTeco Class device
		private CZKEM zkteco = new CZKEM();

		public string Serial = null;

		public List<ZKTecoAttendance> attendances = new List<ZKTecoAttendance>();
		public List<ZKTecoUser> users = new List<ZKTecoUser>();

		public ZKTecoDevice(ConnectionDevice dev)
		{
			this.SetId(dev.GetId());
			this.SetIp(dev.GetIp());
			this.SetPort(dev.GetPort());
			this.SetPassword(dev.GetPassword());
		}

		public ZKTecoDevice(int id, string ip, int port, int password)
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

			ZKTecoDeviceLoggger.Info("Opening connection for testing");
			if (this.zkteco.SetCommPassword(this.GetPassword()))
			{
				ZKTecoDeviceLoggger.Info("Setting password");
				if (this.zkteco.Connect_Net(this.GetIp(), this.GetPort()))
				{
					ZKTecoDeviceLoggger.Info("Connection established");
					IsConnected = true;
					this.zkteco.Disconnect();
				}
				else
				{
					ZKTecoDeviceLoggger.Info("Connection error");
				}
			}

			return IsConnected;

		}
		public void ObtainSerial()
		{

			ZKTecoDeviceLoggger.Debug("Getting serial");
			ZKTecoDeviceLoggger.Debug("Setting password");
			bool hasPassword = zkteco.SetCommPassword(this.GetPassword());

			if (hasPassword)
			{
				ZKTecoDeviceLoggger.Debug("Opennig connection to " + this.GetIp() + ":" + this.GetPort().ToString());
				bool IsConnected = this.zkteco.Connect_Net(this.GetIp(), this.GetPort());

				if (IsConnected)
				{
					string serial;
					ZKTecoDeviceLoggger.Debug("Connected");
					bool hasSerial = this.zkteco.GetSerialNumber(this.GetId(), out serial);
					if (hasSerial)
					{
						this.SetSerial(serial);
						ZKTecoDeviceLoggger.Debug("Serial obtained: " + this.GetSerial());
					}
					else
					{
						ZKTecoDeviceLoggger.Error("Serial not obtained");
					}
				}
				else
				{
					ZKTecoDeviceLoggger.Error("Connection error");
				}
			}
			else
			{
				ZKTecoDeviceLoggger.Error("Password not set, cannot connect");
			}


		}
		public void ObtainAttendance()
		{
			ZKTecoDeviceLoggger.Debug("Getting attendance");
			ZKTecoDeviceLoggger.Debug("Setting password");
			bool hasPassword = this.zkteco.SetCommPassword(this.GetPassword());

			if (hasPassword)
			{
				ZKTecoDeviceLoggger.Debug("Opennig connection to " + this.GetIp() + ":" + this.GetPort().ToString());
				bool IsConnected = this.zkteco.Connect_Net(this.GetIp(), this.GetPort());

				if (IsConnected)
				{
					string serial;
					bool hasSerial = this.zkteco.GetSerialNumber(this.GetId(), out serial);
					if (hasSerial)
					{
						this.SetSerial(serial);
						ZKTecoDeviceLoggger.Debug("Serial obtained: " + this.GetSerial());
					}
					int dwMachineNumber = 1; // Machine ID (in)
					bool readLog = this.zkteco.ReadGeneralLogData(dwMachineNumber);
					ZKTecoDeviceLoggger.Debug("Reading log");

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
						ZKTecoDeviceLoggger.Debug("Reading buffer");

						while (this.zkteco.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
						{
							ZKTecoDeviceLoggger.Debug("Creating attendance");
							ZKTecoAttendance attendance = new ZKTecoAttendance(this.GetSerial(), dwEnrollNumber.ToString(), dwVerifyMode, dwInOutMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, dwWorkCode);
							ZKTecoDeviceLoggger.Debug("Adding attendance to List");
							attendances.Add(attendance);
							ZKTecoDeviceLoggger.Debug(attendance.ToString());
						}
					}
					else
					{
						ZKTecoDeviceLoggger.Error("Error reading attendance log");

					}
					this.zkteco.Disconnect();
				}
				else
				{
					ZKTecoDeviceLoggger.Error("Error connecting to device");
				}
			}
			else
			{
				ZKTecoDeviceLoggger.Error("Error setting password");
			}
		}

		public void ObtainUsers()
		{
			ZKTecoDeviceLoggger.Debug("Getting users");
			ZKTecoDeviceLoggger.Debug("Setting password");
			bool hasPassword = this.zkteco.SetCommPassword(this.GetPassword());

			if (hasPassword)
			{
				ZKTecoDeviceLoggger.Debug("Opennig connection to " + this.GetIp() + ":" + this.GetPort().ToString());
				bool IsConnected = this.zkteco.Connect_Net(this.GetIp(), this.GetPort());

				if (IsConnected)
				{
					string serial;
					bool hasSerial = this.zkteco.GetSerialNumber(this.GetId(), out serial);
					if (hasSerial)
					{
						this.SetSerial(serial);
						ZKTecoDeviceLoggger.Debug("Serial obtained: " + this.GetSerial());
					}
					else
					{
						ZKTecoDeviceLoggger.Error("Error obtaining serial");
					}
					int dwMachineNumber = 1; // Machine ID (in)
					bool readUserLog = this.zkteco.ReadAllUserID(dwMachineNumber);
					ZKTecoDeviceLoggger.Debug("Reading user log");
					if (readUserLog)
					{
						//Variables locales
						// Pointer that points to the BSTR variable. Its value is the user ID of an attendance record.
						// A user ID contains a maximum of 24 digits
						string dwEnrollNumber = string.Empty;
						// User name
						string name = string.Empty;
						// User password
						string password = string.Empty;
						// User privilege, The Privilege parameter specifies the user privilege.
						// The value 0 indicates common user, 1 registrar, 2 administrator, and 3 super administrator.
						int privileges = 0;
						// Flag that indicates whether a user account is enabled,  The Enable parameter specifies whether a user account is enabled.
						// The value 1 indicates that the user account is enabled and 0 indicates that the user account is disabled
						bool enabled = false;
						ZKTecoDeviceLoggger.Debug("Reading buffer");
						while (this.zkteco.SSR_GetAllUserInfo(dwMachineNumber, out dwEnrollNumber, out name, out password, out privileges, out enabled))
						{
							ZKTecoDeviceLoggger.Debug("Creating user");
							ZKTecoUser user = new ZKTecoUser(this.GetSerial(), dwEnrollNumber.ToString(), name.ToString(), password.ToString(), privileges, enabled);
							ZKTecoDeviceLoggger.Debug("Adding user to List");
							users.Add(user);
							ZKTecoDeviceLoggger.Debug(user.ToString());
						}
					}
					else
					{
						ZKTecoDeviceLoggger.Error("Error reading user log");

					}
					this.zkteco.Disconnect();
				}
				else
				{
					ZKTecoDeviceLoggger.Error("Error connecting");
				}
			}
			else
			{
				ZKTecoDeviceLoggger.Error("Error setting password");
			}
		}
	}
}
