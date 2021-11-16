using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using zkemkeeper;


namespace zkteco_cli.Components
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
		private CZKEM zkteco = new CZKEM();

		public bool TestConnect(string ip, int port, int password)
		{
			Console.WriteLine("Opening connection for testing");
			if (this.zkteco.SetCommPassword(password))
			{
				log.Info("Setting password");
				this.IsConnected = true;
			}

			if (this.zkteco.Connect_Net(ip, port))
			{
				log.Info("Connection established");
				this.IsConnected = true;
				this.zkteco.Disconnect();
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
				this.zkteco.Disconnect();
				this.IsConnected = false;
			}
		}

		public string GetSerial()
		{
			log.Debug("Getting serial");
			log.Debug("Setting password");
			bool hasPassword = zkteco.SetCommPassword(this.Password);

			if (hasPassword)
            {
				log.Debug("Opennig connection");
				this.IsConnected = this.zkteco.Connect_Net(this.Host, this.Port);
			}
			else
            {
				log.Error("Password not set, cannot connect");
				this.IsConnected = false;
			}
			
			if (this.IsConnected)
			{
				log.Debug("Connected");
				bool hasSerial = this.zkteco.GetSerialNumber(0, out this.Serial);
				if (hasSerial)
                {
					log.Debug("Serial obtained");
				}
				else
                {
					log.Error("Serial not obtained");
				}
				this.zkteco.Disconnect();
			}
			else
			{
				log.Error("Connection error");
			}

			return this.Serial;

		}

		public void GetAttendance(int machineNumber)
        {
			log.Debug("Getting serial");
			log.Debug("Setting password");
			bool hasPassword = zkteco.SetCommPassword(this.Password);

			if (hasPassword)
			{
				log.Debug("Opennig connection");
				this.IsConnected = this.zkteco.Connect_Net(this.Host, this.Port);
			}
			else
			{
				log.Error("Password not set, cannot connect");
				this.IsConnected = false;
			}

			if (this.IsConnected)
			{
				log.Debug("Connected");
				int dwMachineNumber = machineNumber; // Machine ID (in)
				bool readLog = this.zkteco.ReadGeneralLogData(dwMachineNumber);

				if (readLog)
				{
					log.Info("Reading buffer");

					// Comments are from de SDK documentation
					
					//string dwTMachineNumber = 0; // Pointer that points to the LONG variable. Its value is the machine ID of an attendance record.
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

					while (this.zkteco.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
					{ 
						log.Debug("Machine ID: " + dwMachineNumber.ToString() + " , ");
						log.Debug("User ID of an att: " + dwEnrollNumber.ToString() + " , ");
						log.Debug("Date and time of att: " + dwYear.ToString() + " " + dwMonth.ToString() + " " + dwDay.ToString() + " " + dwHour.ToString() + ":" + dwMinute.ToString());

					}

					this.Disconnect();
				}
				else
				{
					log.Error("Buffer reading error");
				}
			}
			else
            {
				log.Error("Not Connected");
			}



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