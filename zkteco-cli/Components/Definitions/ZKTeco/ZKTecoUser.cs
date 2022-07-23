using System;
using System.Collections.Generic;

namespace zkteco_cli.ZKTeco
{
	public class ZKTecoUser
	{
		public string Id { get; set; }
		public string Name { get; set; }

		private string Password { get; set; }

		public int Privilege { get; set; }

		public bool Enabled { get; set; }

		public string Serial { get; set; }
		public ZKTecoUser()
		{
			this.Id = String.Empty;
			this.Name = String.Empty;
			this.Password = String.Empty;
			this.Serial = String.Empty;
			this.Privilege = 0;
			this.Enabled = false;

		}

		public ZKTecoUser(string serial, string id, string name, string password, int privilege, bool enabled)
		{
			this.Serial = serial;
			this.Id = id;
			this.Name = name;
			this.Password = password;
			this.Privilege = privilege;
			this.Enabled = enabled;

		}


		public string GetPassword()
		{
			if (String.IsNullOrEmpty(this.Password))
			{
				return null;
			}
			else
			{
				return this.Password;
			}

		}

		public override string ToString()
		{
			string obj = "Id: " + this.Id.ToString() + " ";
			obj += "Name: " + this.Name.ToString() + " ";
			obj += "Privileges: " + this.Privilege.ToString() + " ";
			obj += "Enabled: " + this.Enabled.ToString() + " ";
			obj += "Serial: " + this.Serial.ToString() + " ";
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
				return (Id == z.Id) && (Serial == z.Serial);
			}
		}

		public override int GetHashCode()
		{
			int hashCode = 1771101144;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
			hashCode = hashCode * -1521134295 + Privilege.GetHashCode();
			hashCode = hashCode * -1521134295 + Enabled.GetHashCode();
			return hashCode;
		}
	}
}