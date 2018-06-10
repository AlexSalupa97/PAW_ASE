using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medici_Pacienti
{
    class Medic
    {
        private readonly int codM;
        private string numeM;
        private string specialitate;

        public Medic(int cod, string nume, string spec)
        {
            this.codM = cod;
            this.NumeM = nume;
            this.Specialitate = spec;
        }

        

        public string NumeM { get => numeM; set => numeM = value; }
        public string Specialitate { get => specialitate; set => specialitate = value; }
        public int CodM => codM;
    }
}
