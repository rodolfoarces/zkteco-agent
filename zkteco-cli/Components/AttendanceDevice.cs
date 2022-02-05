using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace zkteco_cli.Components
{
    internal class AttendanceDevice
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AttendanceDevice)); 

        int DeviceId = 0;
        string DeviceSerial = null;
        string DeviceIP = null;

        public AttendanceDevice(int id, string serial, string ip)
        {
            this.DeviceId = id;
            this.DeviceSerial = serial;
            this.DeviceIP = ip;
        }

        public void SetDeviceId( int id)
        {
            if (this.DeviceId != id)
            {
                this.DeviceId = id;
            }
        }

        public int GetDeviceId()
        {
            return this.DeviceId;
        }

        public void SetDeviceSerial(string serial)
        {
            if (!string.IsNullOrEmpty(serial))
            {
                this.DeviceSerial= serial;
            }
        }

        public string GetDeviceSerial()
        {
            return DeviceSerial;
        }

        public void SetDeviceIP(string ip)
        {
            if (!string.IsNullOrEmpty(ip))
            {
                this.DeviceIP = ip;
            }
        }

        public string GetDeviceIP()
        {
            return this.DeviceIP;
        }

    }
}
