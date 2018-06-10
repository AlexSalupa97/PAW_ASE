using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medici_Pacienti
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int result) != true)
            {
                e.Cancel = true;
                this.errorProvider1.SetError(textBox1, "Cod invalid");
            }
            else
                errorProvider1.Clear();
        }

    

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (float.TryParse(textBox3.Text, out float resut) != true)
            {
                e.Cancel = true;
                this.errorProvider1.SetError(textBox3, "Tarif invalid");
            }
            else
                errorProvider1.Clear();
        }

    }
}
