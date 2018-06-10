using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medici_Pacienti
{
    public partial class Form1 : Form
    {
        List<Medic> listaM;
        List<Pacient> listaP;
        List<int> listaCodM;
        List<String> specializari;
        List<int> countSpecializari;

        public Font printFont;
        public PrintDocument pd;
        public int counter = 0;

        public Form1()
        {
            InitializeComponent();
            listaM = new List<Medic>();
            listaP = new List<Pacient>();
            listaCodM = new List<int>();
            specializari = new List<string>();
            countSpecializari = new List<int>();
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

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if(textBox2.Text.Length<2)

            {
                e.Cancel = true;
                this.errorProvider1.SetError(textBox2, "Nume invalid");
            }
            else
                errorProvider1.Clear();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Medic m = new Medic(int.Parse(textBox1.Text), textBox2.Text, textBox3.Text);

            List<Medic> medicCopie = new List<Medic>(listaM);

            if (listaM.Count == 0)
            {
                listaM.Add(m);
            }
            else
            {

               

                foreach (Medic md in medicCopie)
                    if (md.CodM != m.CodM)
                    {
                        listaM.Add(m);
                    }
            }

            int okSpec = 1;

            if (specializari.Count == 0)
            {
                specializari.Add(m.Specialitate);
                countSpecializari.Add(0);
            }
            else
            {

                List<string> specializariCopie = new List<string>(specializari);

                foreach (String s in specializariCopie)
                    if (m.Specialitate.Equals(s))
                        okSpec = 0;
                if(okSpec==1)
                {
                    specializari.Add(m.Specialitate);
                    countSpecializari.Add(0);
                }
            }

            
            int index = 0;
            while (m.Specialitate != specializari.ElementAt(index))
                index++;

            int okCount = 1;

            foreach (Medic md in medicCopie)
                if (m.CodM == md.CodM)
                    okCount = 0;


            if(okCount==1)
                countSpecializari[index] = countSpecializari.ElementAt(index) + 1;
            
           

            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                Pacient p = new Pacient(int.Parse(form2.textBox1.Text), form2.textBox2.Text, m.CodM, float.Parse(form2.textBox3.Text));
                listaP.Add(p);

                listView2.Items.Clear();
                ListViewItem lv2;

                foreach (Pacient pc in listaP)
                {
                    lv2 = new ListViewItem(pc.CodP.ToString());
                    lv2.SubItems.Add(pc.NumeP);
                    lv2.SubItems.Add(pc.CodM.ToString());
                    lv2.SubItems.Add(pc.Tarif_consultatie.ToString());
                    listView2.Items.Add(lv2);
                }
            }




            ListViewItem lv1;

            int ok = 1;

            foreach (Medic md in listaM)
            {
                ok = 1;
                foreach (int i in listaCodM)
                {
                    if (md.CodM == i)
                        ok = 0;
                    
                }
                if (ok == 1)
                {
                    lv1 = new ListViewItem(md.CodM.ToString());
                    lv1.SubItems.Add(md.NumeM);
                    lv1.SubItems.Add(md.Specialitate);
                    listView1.Items.Add(lv1);
                    listaCodM.Add(md.CodM);
                }
            }

        }




















        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem selectat = null;
            if (listView1.SelectedItems.Count > 0)
            {
                selectat = listView1.SelectedItems[0];
                listView2.Items.Clear();
                ListViewItem lv2;
                foreach (Pacient pc in listaP)
                {
                    if (pc.CodM == int.Parse(selectat.SubItems[0].Text))
                    {
                        lv2 = new ListViewItem(pc.CodP.ToString());
                        lv2.SubItems.Add(pc.NumeP);
                        lv2.SubItems.Add(pc.CodM.ToString());
                        lv2.SubItems.Add(pc.Tarif_consultatie.ToString());
                        listView2.Items.Add(lv2);
                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void calculeazaTarifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem selectat = null;
            if (listView1.SelectedItems.Count > 0)
            {
                float suma = 0;
                selectat = listView1.SelectedItems[0];
                int cod = int.Parse(selectat.SubItems[0].Text);
                foreach (Pacient p in listaP)
                {
                    if (p.CodM == cod)
                    {
                        suma = suma + p.Tarif_consultatie;
                    }
                }
                textBox4.Text = suma.ToString();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Form3 form3 = new Form3();

            List<Pacient> sortat = listaP;
            sortat.Sort();

            foreach (Medic m in listaM)
                foreach (Pacient p in sortat)
                {
                    if (m.CodM == p.CodM)
                        form3.textBox1.Text += m.NumeM + " " + p.Tarif_consultatie.ToString() + "\r\n";
                }
            form3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            for (int i = 0; i < specializari.Count; i++)
                f4.chart1.Series["Nr. medici / specializare"].Points.AddXY(specializari[i], countSpecializari[i]);

            f4.Show();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printFont = new Font("Arial", 12);
            pd=new PrintDocument();
            counter = 0;
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
            pd.Print();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;

            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);

            if (listaM.Count >= counter)
            {
                while (count < linesPerPage && listaM.Count > counter)
                {
                    yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(listaM[counter].NumeM + " " + listaM[counter].Specialitate, printFont, Brushes.Black,leftMargin, yPos, new StringFormat());
                    count++;
                    counter++;
                }
            }

            if (counter < listaM.Count)
                ev.HasMorePages = true ;
            else
                ev.HasMorePages = false;
        }

        private void insertInDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var sqlQuery = "select * from Medici where 0 = 1";
            //SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Medici;Integrated Security=True;Pooling=False");
            //DataSet dataSet = new DataSet();
            //dataAdapter.Fill(dataSet);

            //var newRow = dataSet.Tables["Medici"].NewRow();
            //newRow["codM"] = listaM[0].CodM;
            //newRow["numeM"] = listaM[0].NumeM;
            //newRow["specializare"] = listaM[0].Specialitate;
            //dataSet.Tables["Medici"].Rows.Add(newRow);

            //new SqlCommandBuilder(dataAdapter);
            //dataAdapter.Update(dataSet);
        }
    }
}
