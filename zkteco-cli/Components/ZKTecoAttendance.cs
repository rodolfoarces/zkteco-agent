using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace zkteco_cli.Components
{
    internal class ZKTecoAttendance
    {
		public string Serial { get; set; }

		public string UserId { get; set; }

		/*	The VerifyMode parameter specifies the verification mode. The values are described 
		 *	as follows: under normal conditions: 
		 *	0 indicates password verification, 
		 *	1 fingerprint verification, and 
		 *	2 card verification 
		 */
		public int VerifyMode { get; set; }
		
		/* The InOutMode parameter specifies the attendance status. The values are described
		 * as follows:
		 * 0-Check-In (Default)
		 * 1-Check-Out
		 * 2-Break-Out
		 * 3-Break-In
		 * 4-OT-In
		 * 5-OT-Out
		*/
		public int InOutMode { get; set;}

		/* Variables of the date/time in which the user check in the device
		 */
		public int Year { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }

		/* If used is the work code for the attendance, a user may have multiple work codes
		 * for differente purposes
		 */
		public int WorkCode { get; set; }

		public ZKTecoAttendance(string serial, string enrollnumber, int verifymode, int inoutmode, int year, int month, int day, int hour, int minute, int second, int workcode)
		{
			this.Serial = serial;
			this.UserId = enrollnumber;
			this.VerifyMode = verifymode;
			this.InOutMode = inoutmode;
			this.Year = year;
			this.Month = month;
			this.Day = day;
			this.Hour = hour;
			this.Minute = minute;
			this.Second = second;
			this.WorkCode = workcode;

		}

        public override string ToString()
        {
			string obj;

			obj = "Serial: " + Serial.ToString() + " ";
			obj += "User ID: " + UserId.ToString() + " ";
			obj += "VerifyMode: " + VerifyMode.ToString() + " ";
			obj += "InOutMode: " + InOutMode.ToString() + " ";
			obj += "Year: " + Year.ToString() + " ";
			obj += "Month: " + Month.ToString() + " ";
			obj	+= "Day: " + Day.ToString() + " ";
			obj += "Hour: " + Hour.ToString() + " ";
			obj += "Minute: " + Minute.ToString() + " ";
			obj += "Second: " + Second.ToString() + " ";
			obj += "WorkCode: " + WorkCode.ToString();

			return obj;
        }

    }
}
