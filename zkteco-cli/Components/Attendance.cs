using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace zkteco_cli.Components
{
    internal class Attendance
    {
        Device device;
        Employee employee;

        private static readonly ILog log = LogManager.GetLogger(typeof(Attendance));

		/*	The VerifyMode parameter specifies the verification mode. The values are described 
		 *	as follows: under normal conditions: 
		 *	0 indicates password verification, 
		 *	1 fingerprint verification, and 
		 *	2 card verification 
		 */
		int VerifyMode;
		
		/* The InOutMode parameter specifies the attendance status. The values are described
		 * as follows:
		 * 0-Check-In (Default)
		 * 1-Check-Out
		 * 2-Break-Out
		 * 3-Break-In
		 * 4-OT-In
		 * 5-OT-Out
		*/
		int InOutMode;

		/* Variables of the date/time in which the user check in the device
		 */
		int Year;
		int Month;
		int Day;
		int Hour;
		int Minute;
		int Second;

		/* If used is the work code for the attendance, a user may have multiple work codes
		 * for differente purposes
		 */
		int WorkCode;
	}
}
