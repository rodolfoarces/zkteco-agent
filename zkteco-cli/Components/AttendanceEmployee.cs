using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkteco_cli.Components
{
    internal class AttendanceEmployee
    {

        String EnrollNumber = null;
        String Name = null;

        public AttendanceEmployee(string number, string name)
        {
            this.SetEnrollNumber(number);
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
    }
}
