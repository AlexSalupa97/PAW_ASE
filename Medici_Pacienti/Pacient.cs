using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medici_Pacienti
{
    class Pacient : IComparable
    {
        private readonly int codP;
        private string numeP;
        private int codM;
        private float tarif_consultatie;

        public Pacient(int codP, string nume, int codM, float tarif)
        {
            this.codP = codP;
            this.NumeP = nume;
            this.CodM = codM;
            this.Tarif_consultatie = tarif;
        }

        public int CodP => codP;

        public string NumeP { get => numeP; set => numeP = value; }
        public int CodM { get => codM; set => codM = value; }
        public float Tarif_consultatie { get => tarif_consultatie; set => tarif_consultatie = value; }

        public int CompareTo(object obj)
        {
            Pacient p = (Pacient)obj;
            return (int)(-this.tarif_consultatie+p.tarif_consultatie);
        }
    }
}
