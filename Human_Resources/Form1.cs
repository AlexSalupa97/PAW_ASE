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

namespace Human_Resources
{
    public partial class Form1 : Form
    {

        List<Department> listaD;
        List<Employee> listaE;

        Font font;
        PrintDocument pd;
        int counter;

        public Form1()
        {
            InitializeComponent();
            listaD = new List<Department>();
            listaE = new List<Employee>();

            string linie;

            StreamReader reader = new StreamReader("listaD.txt");
            while ((linie = reader.ReadLine()) != null)
            {
                string[] splitter = linie.Split(' ');
                Department d = new Department(int.Parse(splitter[0]), splitter[1]);
                listaD.Add(d);
            }

            listaE.Add(new Employee());
            BindingList<Employee> l = new BindingList<Employee>(listaE);
            dataGridView1.DataSource = l;

            sqlConnection1.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            sqlDataAdapter1.Fill(table);
            

            foreach (DataRow row in table.Rows)
            {
                var rand = row.ItemArray;
                Employee emp = new Employee((int)rand[0], (string)rand[1], (DateTime)rand[2], (double)rand[3], (int)rand[4]);
                listaE.Add(emp);
            }

            BindingList<Employee> l = new BindingList<Employee>(listaE);
            dataGridView1.DataSource = l;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();

            foreach(Department d in listaD)
                f2.listBox1.Items.Add(d.Id+" - "+d.Name);

            if (f2.ShowDialog() == DialogResult.OK)
            {
                String[] s = f2.listBox1.SelectedItem.ToString().Split(' ');
                Employee e1 = new Employee(int.Parse(f2.textBox1.Text), f2.textBox2.Text, f2.dateTimePicker1.Value, double.Parse(f2.textBox4.Text), int.Parse(s[0]));
                listaE.Add(e1);
                BindingList<Employee> l = new BindingList<Employee>(listaE);
                dataGridView1.DataSource = l;

            }

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    Employee emp = listaE.ElementAt(row.Index);
                    sqlDataAdapter1.DeleteCommand.Parameters.Clear();
                    sqlDataAdapter1.DeleteCommand.Parameters.Add("@Original_idE", SqlDbType.Int).Value = emp.Id;
                    sqlDataAdapter1.DeleteCommand.ExecuteNonQuery();
                    listaE.RemoveAt(row.Index);
                }

            BindingList<Employee> l = new BindingList<Employee>(listaE);
            dataGridView1.DataSource = l;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                Form2 f2= new Form2();
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                Employee emp = listaE.ElementAt(row.Index);
                listaE.RemoveAt(row.Index);
                

                f2.textBox1.Text = emp.Id.ToString();
                f2.textBox2.Text = emp.Name;
                f2.dateTimePicker1.Value = emp.BirthDate;
                f2.textBox4.Text = emp.Wage.ToString();
                foreach (Department d in listaD)
                    f2.listBox1.Items.Add(d.Id + " - " + d.Name);

                try
                {
                    if (f2.ShowDialog() == DialogResult.OK)
                    {
                        String[] s = f2.listBox1.SelectedItem.ToString().Split(' ');
                        Employee e1 = new Employee(int.Parse(f2.textBox1.Text), f2.textBox2.Text, f2.dateTimePicker1.Value, double.Parse(f2.textBox4.Text), int.Parse(s[0]));
                        listaE.Add(e1);
                        BindingList<Employee> l = new BindingList<Employee>(listaE);
                        dataGridView1.DataSource = l;

                    }
                }
                catch (Exception ex)
                {
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int ct = 0;
            foreach (Employee emp in listaE)
                if (!(bool)emp)
                    ct++;

            MessageBox.Show(ct+ " unemployed!");
        }

        private void exportXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            XmlSerializer xml = new XmlSerializer(typeof(List<Employee>));
            using (StreamWriter writer = new StreamWriter("listaE.xml"))
                xml.Serialize(writer, listaE);

        }

        private void sortAscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Employee> sortatasc = new List<Employee>(listaE);
            sortatasc.Sort();
            BindingList<Employee> l = new BindingList<Employee>(sortatasc);
            dataGridView1.DataSource = l;
        }

        private void sortDescToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Employee> sortatasc = new List<Employee>(listaE);
            sortatasc.Sort();
            sortatasc.Reverse();
            BindingList<Employee> l = new BindingList<Employee>(sortatasc);
            dataGridView1.DataSource = l;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            font = new Font("Arial", 14);
            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(printpage);
            counter = 0;
            pd.Print();
        }

        private void printpage(object sender, PrintPageEventArgs e)
        {
            float linii = 0;
            float yPos = 0;
            int count = 0;

            float leftM = e.MarginBounds.Left;
            float topM = e.MarginBounds.Top;

            linii = e.MarginBounds.Height / font.GetHeight(e.Graphics);

            if (listaD.Count >= counter)
            {
                while (count < linii && counter < listaD.Count)
                {
                    yPos = topM + (count * font.GetHeight(e.Graphics));
                    e.Graphics.DrawString(listaD[counter].Name, font, Brushes.Black, leftM, yPos, new StringFormat());
                    counter++;
                    count++;
                }

                if (listaD.Count > counter)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }

        }

        private void updateDBToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //sqlDataAdapter1.DeleteCommand.ExecuteNonQuery();

            DataTable table = new DataTable();
            sqlDataAdapter1.Fill(table);
            List<Employee> listaInBD = new List<Employee>();

            foreach (DataRow row in table.Rows)
            {
                var rand = row.ItemArray;
                Employee emp = new Employee((int)rand[0], (string)rand[1], (DateTime)rand[2], (double)rand[3], (int)rand[4]);
                listaInBD.Add(emp);
            }

            int neinserate = 0;
            

            foreach (Employee emp in listaE)

                if (emp.BirthDate.Year > 1800 && !listaInBD.Contains(emp))
                {
                    try
                    {
                        sqlDataAdapter1.InsertCommand.Parameters.Clear();

                        sqlDataAdapter1.InsertCommand.Parameters.Add("@idE", SqlDbType.Int).Value = emp.Id;
                        sqlDataAdapter1.InsertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = emp.Name;
                        sqlDataAdapter1.InsertCommand.Parameters.Add("@birthdate", SqlDbType.DateTime).Value = emp.BirthDate;
                        sqlDataAdapter1.InsertCommand.Parameters.Add("@wage", SqlDbType.Float).Value = float.Parse(emp.Wage.ToString());
                        sqlDataAdapter1.InsertCommand.Parameters.Add("@idD", SqlDbType.Int).Value = emp.IdDepartment;

                        sqlDataAdapter1.InsertCommand.ExecuteNonQuery();

                        MessageBox.Show("Inserted idE: " + emp.Id);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

            

        }
    }
}
