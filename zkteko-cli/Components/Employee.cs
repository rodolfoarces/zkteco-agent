using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zkteko_cli.Components
{
    class Employee
    {
        int Id = 0;
        string Name = string.Empty;

        public void SetName(string name)
        {
            this.Name = name;
        }
        public string GetName()
        {
            return this.Name;
        }
        public void SetId(int id)
        {
            this.Id = id;
        }
        public int GetId()
        {
            return this.Id;
        }
    }
}
