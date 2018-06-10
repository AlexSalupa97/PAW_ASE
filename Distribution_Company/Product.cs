using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distribution_Company
{
    [Serializable]
    public class Product:IComparable
    {
        private int id;
        private string name;
        private int units;
        private double price;
        private int supplierid;

        public Product(int id, string name, int units, double price, int supplierid)
        {
            this.id = id;
            this.name = name;
            this.units = units;
            this.price = price;
            this.supplierid = supplierid;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Units { get => units; set => units = value; }
        public double Price { get => price; set => price = value; }
        public int Supplierid { get => supplierid; set => supplierid = value; }

        public int CompareTo(object obj)
        {
            Product p = (Product)obj;
            return this.name.CompareTo(p.name);
        }

        public static explicit operator double(Product p)
        {
            return p.price * p.units;
        }

        public Product()
        {
            this.id = 0;
            this.name = "";
            this.units = 0;
            this.price = 0;
            this.supplierid = 0;
        }
    }
}
