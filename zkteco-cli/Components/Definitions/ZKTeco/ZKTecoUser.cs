using System;
using System.Collections.Generic;

namespace zkteco_cli.ZKTeco
{
	public class ZKTecoUser
	{
		public string id { get; set; }
		public string name { get; set; }

		private string password { get; set; }

		public int privilege { get; set; }

		public bool enabled { get; set; }

		public string serial { get; set; }
		public ZKTecoUser()
		{
			this.id = String.Empty;
			this.name = String.Empty;
			this.password = String.Empty;
			this.serial = String.Empty;
			this.privilege = 0;
			this.enabled = false;

		}

		public ZKTecoUser(string serial, string id, string name, string password, int privilege, bool enabled)
		{
			this.serial = serial;
			this.id = id;
			this.name = name;
			this.password = password;
			this.privilege = privilege;
			this.enabled = enabled;

		}


        public string GetPassword()
		{
			if (String.IsNullOrEmpty(this.password))
			{
				return null;
			}
			else
			{
				return this.password;
			}

		}

		public override string ToString()
		{
			string obj = "Id: " + this.id.ToString() + " ";
			obj += "Name: " + this.name.ToString() + " ";
			obj += "Privileges: " + this.privilege.ToString() + " ";
			obj += "Enabled: " + this.enabled.ToString() + " ";
			obj += "Serial: " + this.serial.ToString() + " ";
			return obj;
		}

		public override bool Equals(Object zktecouser)
		{
			//Check for null and compare run-time types.
			if ((zktecouser == null) || !this.GetType().Equals(zktecouser.GetType()))
			{
				return false;
			}
			else
			{
				ZKTecoUser z = (ZKTecoUser)zktecouser;
				return (id == z.id) && (serial == z.serial);
			}
		}

		public override int GetHashCode()
		{
			int hashCode = 1771101144;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(password);
			hashCode = hashCode * -1521134295 + privilege.GetHashCode();
			hashCode = hashCode * -1521134295 + enabled.GetHashCode();
			return hashCode;
		}
	}
}