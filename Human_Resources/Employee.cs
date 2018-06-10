using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources
{
    public class Employee:IComparer<Employee>,IComparable
    {
        private int id;
        private string name;
        private DateTime birthDate;
        private double wage;
        private int idDepartment;

        public Employee()
        {
            this.id = 0;
            this.name = "";
            this.birthDate = DateTime.MinValue;
            this.wage = 0;
            this.idDepartment = 0;
        }

        public Employee(int id, string name, DateTime birthDate, double wage, int idDepartment)
        {
            this.id = id;
            this.name = name;
            this.birthDate = birthDate;
            this.wage = wage;
            this.idDepartment = idDepartment;
        }



        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        public double Wage { get => wage; set => wage = value; }
        public int IdDepartment { get => idDepartment; set => idDepartment = value; }

        public int Compare(Employee x, Employee y)
        {
            return x.name.CompareTo(y.name);
        }

        public int CompareTo(object obj)
        {
            Employee e = (Employee)obj;
            return this.name.CompareTo(e.name);
        }

        public static explicit operator bool(Employee e)
        {
            if(e.IdDepartment!=0)
                return true;
            return false;
        }
    }
}
