using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distribution_Company
{
    [Serializable]
    public class Supplier
    {
        private int id;
        private string name;

        public Supplier(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

        public Supplier()
        {
            this.id = 0;
            this.name = "";
        }
    }
}
