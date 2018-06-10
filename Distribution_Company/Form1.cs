using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Distribution_Company
{
    public partial class Form1 : Form
    {
        List<Supplier> listaS;
        List<Product> listaP;

        Font font;
        PrintDocument pd;
        int counter;

        public Form1()
        {
            InitializeComponent();
            listaS = new List<Supplier>();
            listaP = new List<Product>();


            string line;
            StreamReader file = new StreamReader("supplier.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] campuri = line.Split(' ');
                Supplier s = new Supplier(int.Parse(campuri[0]), campuri[1]);
                listaS.Add(s);
            }
            file.Close();


            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = File.Create("ListaS1.dat"))
                formatter.Serialize(fs, listaS);

            if (File.Exists("ListaS1.dat"))
            {
                FileStream fs = new FileStream("ListaS.dat", FileMode.Open, FileAccess.Read);
                List<Supplier> l=(List<Supplier>)formatter.Deserialize(fs);
                fs.Close();
            }

            XmlSerializer xml = new XmlSerializer(typeof(List<Supplier>));
            using (StreamWriter writer = new StreamWriter("supplier.xml"))
                xml.Serialize(writer, listaS);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection1.Open();

            DataTable products = new DataTable();
            sqlDataAdapter1.Fill(products);

            List<Product> loadP = new List<Product>();
            foreach (DataRow dr in products.Rows)
            {
                var values = dr.ItemArray;
                Product p = new Product((int)values[0], (string)values[1], (int)values[2], (double)values[3], (int)values[4]);
                loadP.Add(p);

            }

            foreach (Product pr in loadP)
            {
                ListViewItem lv = new ListViewItem(pr.Id.ToString());
                lv.SubItems.Add(pr.Name);
                lv.SubItems.Add(pr.Units.ToString());
                lv.SubItems.Add(pr.Price.ToString());
                lv.SubItems.Add(pr.Supplierid.ToString());

                listView1.Items.Add(lv);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Product>));
            using (StreamWriter writer = new StreamWriter("listp.xml"))
                serializer.Serialize(writer, loadP);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            foreach (Supplier s in listaS)
                f2.comboBox1.Items.Add(s.Id+" Nume: "+s.Name);
            f2.comboBox1.SelectedIndex = 0;

            

            if (f2.ShowDialog() == DialogResult.OK)
            {
                string idS = f2.comboBox1.SelectedItem.ToString();
                string[] tokenizat = idS.Split(' ');
                Product p = new Product(int.Parse(f2.textBox1.Text), f2.textBox2.Text, int.Parse(f2.textBox3.Text),double.Parse(f2.textBox4.Text), int.Parse(tokenizat[0]));

                listaP.Add(p);

                listView1.Items.Clear();
                listaP.Sort();
                foreach (Product pr in listaP)
                {
                    ListViewItem lv = new ListViewItem(pr.Id.ToString());
                    lv.SubItems.Add(pr.Name);
                    lv.SubItems.Add(pr.Units.ToString());
                    lv.SubItems.Add(pr.Price.ToString());
                    lv.SubItems.Add(pr.Supplierid.ToString());

                    listView1.Items.Add(lv);
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem selectat = null;
            if (listView1.SelectedItems.Count > 0)
                selectat = listView1.SelectedItems[0];

            int i = 0;
            List<Product> copie = new List<Product>(listaP);
            foreach (Product p in copie)
            {
                if (p.Id == int.Parse(selectat.SubItems[0].Text))
                    listaP.RemoveAt(i);
                i++;
            }

            Form2 f2 = new Form2();
            f2.textBox1.Text = selectat.SubItems[0].Text;
            f2.textBox2.Text = selectat.SubItems[1].Text;
            f2.textBox3.Text = selectat.SubItems[2].Text;
            f2.textBox4.Text = selectat.SubItems[3].Text;
            f2.comboBox1.Text = f2.textBox1.Text + " Nume: " + selectat.SubItems[4].Text;

            if (f2.ShowDialog() == DialogResult.OK)
            {
                string idS = f2.comboBox1.Text;
                string[] tokenizat = idS.Split(' ');
                Product p = new Product(int.Parse(f2.textBox1.Text), f2.textBox2.Text, int.Parse(f2.textBox3.Text), double.Parse(f2.textBox4.Text), int.Parse(tokenizat[0]));

                listaP.Add(p);
            }

            listView1.Items.Clear();

            listaP.Sort();

            foreach (Product pr in listaP)
            {
                ListViewItem lv = new ListViewItem(pr.Id.ToString());
                lv.SubItems.Add(pr.Name);
                lv.SubItems.Add(pr.Units.ToString());
                lv.SubItems.Add(pr.Price.ToString());
                lv.SubItems.Add(pr.Supplierid.ToString());

                listView1.Items.Add(lv);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem selectat = null;
            if (listView1.SelectedItems.Count > 0)
                selectat = listView1.SelectedItems[0];

            int i = 0;
            List<Product> copie = new List<Product>(listaP);
            foreach (Product p in copie)
            {
                if (p.Id == int.Parse(selectat.SubItems[0].Text))
                    listaP.RemoveAt(i);
                i++;
            }

            listView1.Items.Clear();

            listaP.Sort();

            foreach (Product pr in listaP)
            {
                ListViewItem lv = new ListViewItem(pr.Id.ToString());
                lv.SubItems.Add(pr.Name);
                lv.SubItems.Add(pr.Units.ToString());
                lv.SubItems.Add(pr.Price.ToString());
                lv.SubItems.Add(pr.Supplierid.ToString());

                listView1.Items.Add(lv);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string afisare = null;

            foreach (Product p in listaP)
                afisare += (double)p+"\r\n";

            MessageBox.Show(afisare);
        }

        private void openChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();

            List<int> count = new List<int>(listaS.Count);

            for (int i = 1; i <= listaS.Count; i++)
            {
                count.Add(0);
            }

            foreach (Product p in listaP)
            {
                count[p.Supplierid-1]++;
            }

            foreach (Supplier s in listaS)
                f3.chart1.Series["Nr. produse"].Points.AddXY(s.Name, count[s.Id-1]);

            f3.Show();

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            font = new Font("Calibri", 20);
            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.printpage);
            counter = 0;
            pd.Print();
        }

        private void printpage(object sender, PrintPageEventArgs ev)
        {
            float lines = 0;
            float yPos = 0;
            int count = 0;

            float leftM = ev.MarginBounds.Left;
            float topM = ev.MarginBounds.Top;

            lines = ev.MarginBounds.Height / font.GetHeight(ev.Graphics);

            if (listaS.Count >= counter)
            {
                while (count < lines && listaS.Count > counter)
                {
                    yPos = topM + (count * font.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(listaS[counter].Name, font, Brushes.Cyan, leftM, yPos, new StringFormat());
                    count++;
                    counter++;
                }

                if (counter < listaS.Count)
                    ev.HasMorePages = true;
                else
                    ev.HasMorePages = false;
            }
        }

        private void insertDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            



            sqlDataAdapter1.InsertCommand.Parameters.Clear();

            sqlDataAdapter1.InsertCommand.Parameters.Add("@id", SqlDbType.Int).Value = listaP[0].Id;
            sqlDataAdapter1.InsertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = listaP[0].Name;
            sqlDataAdapter1.InsertCommand.Parameters.Add("@units", SqlDbType.Int).Value = listaP[0].Units;
            sqlDataAdapter1.InsertCommand.Parameters.Add("@price", SqlDbType.Float).Value = listaP[0].Price;
            sqlDataAdapter1.InsertCommand.Parameters.Add("@supplier", SqlDbType.Int).Value = listaP[0].Supplierid;
            sqlDataAdapter1.InsertCommand.ExecuteNonQuery();

            //INSERT INTO[products] ([id], [name], [units], [price], [idS]) VALUES(@id, @name, @units, @price, @idS);
            

            //sqlDataAdapter1.Update(myDataSet);
            MessageBox.Show("Insert");

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X&&e.Alt)
                Application.Exit();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X && e.Alt)
                Application.Exit();
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X && e.Alt)
                Application.Exit();
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X && e.Alt)
                Application.Exit();
        }
    }
}
