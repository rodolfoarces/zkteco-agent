using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace zkteco_cli.ZKTeco
{
	internal class ZKTecoAttendance
    {
		public string serial { get; set; }

		public string user_id { get; set; }

		/*	The VerifyMode parameter specifies the verification mode. The values are described 
		 *	as follows: under normal conditions: 
		 *	0 indicates password verification, 
		 *	1 fingerprint verification, and 
		 *	2 card verification 
		 */
		public int verify_mode { get; set; }
		
		/* The InOutMode parameter specifies the attendance status. The values are described
		 * as follows:
		 * 0-Check-In (Default)
		 * 1-Check-Out
		 * 2-Break-Out
		 * 3-Break-In
		 * 4-OT-In
		 * 5-OT-Out
		*/
		public int in_out_mode { get; set;}

		/* Variables of the date/time in which the user check in the device
		 */

		public int year { get; set; }
		public int month { get; set; }
		public int day { get; set; }
		public int hour { get; set; }
		public int minute { get; set; }
		public int second { get; set; }

		/* If used is the work code for the attendance, a user may have multiple work codes
		 * for differente purposes
		 */
		public int work_code { get; set; }

		public ZKTecoAttendance(string serial, string enrollnumber, int verifymode, int inoutmode, int year, int month, int day, int hour, int minute, int second, int workcode)
		{
			this.serial = serial;
			this.user_id = enrollnumber;
			this.verify_mode = verifymode;
			this.in_out_mode = inoutmode;
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
			this.work_code = workcode;

		}

        public ZKTecoAttendance()
        {
            this.serial = String.Empty;
            this.user_id = String.Empty;
            this.verify_mode = 0;
            this.in_out_mode = 0;
            this.year = 0;
            this.month = 0;
            this.day = 0;
            this.hour = 0;
            this.minute = 0;
            this.second = 0;
            this.work_code = 0;

        }

        public override string ToString()
        {
			string obj;

			obj = "Serial: " + this.serial.ToString() + " ";
			obj += "User ID: " + this.user_id.ToString() + " ";
			obj += "VerifyMode: " + this.verify_mode.ToString() + " ";
			obj += "InOutMode: " + this.in_out_mode.ToString() + " ";
			obj += "Year: " + this.year.ToString() + " ";
			obj += "Month: " + this.month.ToString() + " ";
			obj	+= "Day: " + this.day.ToString() + " ";
			obj += "Hour: " + this.hour.ToString() + " ";
			obj += "Minute: " + this.minute.ToString() + " ";
			obj += "Second: " + this.second.ToString() + " ";
			obj += "WorkCode: " + this.work_code.ToString();

			return obj;
        }

    }
}
