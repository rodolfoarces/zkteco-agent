using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace zkteco_cli.Components
{
    internal class AttendanceEmployee
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AttendanceEmployee));

        string EnrollNumber = null;
        string Name = null;

        public AttendanceEmployee(string enrollnumber)
        {
            this.SetEnrollNumber(enrollnumber);
        }

        public AttendanceEmployee(string enrollnumber, string name)
        {
            this.SetEnrollNumber(enrollnumber);
            this.SetName(name);
        }

        public void SetEnrollNumber(String number)
        {
            this.EnrollNumber = number;
        }

        public String GetEnrollNumber()
        {
            return this.EnrollNumber;
        }

        public void SetName(String name)
        {
            this.Name = name;

        }

        public String GetName()
        {
            if (this.Name == null) 
            { 
                return null; 
            } 
            else 
            { 
                return this.Name; 
            }
        }

        public override string ToString()
        {
            return this.EnrollNumber.ToString();
        }
    }
}
