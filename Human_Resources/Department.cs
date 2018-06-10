﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources
{
    public class Department
    {
        private int id;
        private string name;

        public Department()
        {
            this.id = 0;
            this.name = "";
        }

        public Department(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
