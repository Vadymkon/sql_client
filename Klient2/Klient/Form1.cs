﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klient2
{
    public partial class Form1 : Form
    {
        public DataTable T;
        string DB = @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=ChemistryStaff; Integrated Security=True";
        List<String> VocClasses = new List<String> { };

        public Form1()
        {
            InitializeComponent();
        }

        void CheckChoose(CheckBox checkBox)
        {
            checkBox.Enabled = false;
            List<CheckBox> form1checks = new List<CheckBox> { checkBox1, checkBox2, checkBox3, checkBox4 };
            foreach (var i in form1checks)
                if (i != checkBox) {
                    i.Checked = false;
                    i.Enabled = true;
                }

            checkBox5.Enabled = checkBox == checkBox1;
            checkBox6.Enabled = checkBox == checkBox1;

        }

        void EnableAllTextBoxes(bool enable)
        {
            List<TextBox> textBoxes = new List<TextBox> { };
            panel1.Controls.OfType<TextBox>().ToList().ForEach(textB => textB.Enabled = enable);
            comboBox1.Enabled = enable;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose(checkBox1);
            EnableAllTextBoxes(false);
            //textBox1.Enabled = checkBox5.Checked;
            //comboBox1.Enabled = checkBox6.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose(checkBox2);
            EnableAllTextBoxes(true);
            textBox1.Enabled = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose(checkBox3);
            EnableAllTextBoxes(true);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose(checkBox4);
            EnableAllTextBoxes(true);
        }
        /*        static DataTable GetTable()
                {
                    // Here we create a DataTable with four columns.
                    DataTable T = new DataTable();
                    T.Columns.Add("ID", typeof(int));
                    T.Columns.Add("Ім'я", typeof(string));
                    T.Columns.Add("Прізвище", typeof(string));
                    T.Columns.Add("Дата народження", typeof(DateTime));

                    // Here we add five DataRows.
                    T.Rows.Add(25, "Німеччина", "David", DateTime.Now);
                    T.Rows.Add(50, "Україна", "Sam", DateTime.Now);
                    T.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
                    T.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
                    T.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
                    return T;
                }*/

        void UpdateDB() {
            //string DB = @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=ChemistryStaff; Integrated Security=True";
            String Query = @"SELECT Mt.ID, Name, Formular,St.Class, MolWeight, State FROM Compounds as Mt
                                INNER JOIN Classes as St
                                ON Mt.Class = St.ID ";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                Conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(Query, DB);
                da.Fill(ds);
                T = ds.Tables[0];
                dataGridView1.DataSource = T;
                Query = "select Class from Classes";
                VocClasses.Clear();
                using (SqlCommand cmd = new SqlCommand(Query, Conn))
                using (SqlDataReader read = cmd.ExecuteReader())  // reader based on comand-object
                {
                    while (read.Read()) //go every line of table
                        VocClasses.Add((read["Class"].ToString()));
                }
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(VocClasses.ToArray());
            }
        } 

        private void Form1_Load(object sender, EventArgs e)
        {
            Size = new Size(900, 600);
            string Query = "select * from Compounds where Name = N'Метан' OR Name = N'Етен'";
            using (SqlConnection Conn = new SqlConnection(DB)) //made connection
            {
                DataSet ds = new DataSet(); 
                DataTable dt = new DataTable();
                Conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(Query, DB); 
                da.Fill(ds); //filling info in ds 
                T = ds.Tables[0]; 
                /*
                using (SqlCommand cmd = new SqlCommand(Query, Conn)) //for commands
                using (SqlDataReader read = cmd.ExecuteReader())  // reader based on comand-object
                {
                    while (read.Read()) //go every line of table
                    {
                        textBox1.Text = (read["Name"].ToString()); 
                        textBox2.Text = (read["Class"].ToString());
                        break; //for ONLY first line
                    }
                }
                */
                if (T.Rows.Count<2)
                {
                    string insertQuery = @"
                                    INSERT INTO Compounds (Name, Formular, Class, MolWeight, State)
                                    VALUES
                                        (N'Метан', N'CH4', 1, 16.04, N'Газоподібний'),
                                        (N'Етен', N'C2H4', 2, 28.05, N'Газоподібний')";
                    // Made object cmd for executing scripts
                    using (SqlCommand cmd = new SqlCommand(insertQuery, Conn))
                    {
                        // Execute SQL-script
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)   MessageBox.Show("Added start values.");
                        else                    MessageBox.Show("Problems with start values.");
                    }
                }

                
            }
            UpdateDB(); //show DB in view
        }

        void ExecuteQuery(String insertQuery)
        {
            using (SqlConnection Conn = new SqlConnection(DB)) {
            using (SqlCommand cmd = new SqlCommand(insertQuery, Conn))
            {
                    Conn.Open();
                // Execute SQL-script
                int rowsAffected = cmd.ExecuteNonQuery();
                //if (rowsAffected > 0) MessageBox.Show("Added values.");
                //else MessageBox.Show("Problems with values.");
            }
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox6.Checked;
        }

        public bool IntFormat(string p)
        {
            if (p.Trim().Length == 0) return true;
            Regex R = new Regex(@"^[0-9]+([.,][0-9]+)?$");
            Match M = R.Match(p);
            return M.Success;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !checkBox5.Checked && !checkBox6.Checked) ;
            else { 
            if (!IntFormat(textBox5.Text)) { MessageBox.Show("Wrong MolWEIGHT"); return; };
            if (!IntFormat(textBox1.Text)) { MessageBox.Show("Wrong ID"); return; };
            }

            String qName = textBox2.Text != "" ? textBox2.Text : "Template",
                   qFormular = textBox3.Text != "" ? textBox3.Text : "C3H20",
                   qClass = comboBox1.Text != "" ? (VocClasses.IndexOf(comboBox1.Text)+1).ToString() : "2",
                   qMolWeight = textBox5.Text != "" ? textBox5.Text : "20",
                   qState = textBox4.Text != "" ? textBox4.Text : "Газоподібний",
                   qID = textBox1.Text != "" ? textBox1.Text : "26";
            if (checkBox4.Checked)
            {
                String Query = $@"DELETE FROM Compounds WHERE ";
                bool isSomething = false;
                UpdateDB();
                int startRows = T.Rows.Count;

                if (textBox2.Text != "") { Query += $"Name = N'{qName}'"; isSomething = true; }

                if (textBox3.Text != "") { if (isSomething) Query += " AND "; Query += $"Formular = N'{qFormular}' "; isSomething = true; }

                if (textBox4.Text != "") { if (isSomething) Query += " AND "; Query += $"State = N'{qState}'"; isSomething = true; }

                if (textBox5.Text != "") { if (isSomething) Query += " AND "; Query += $"MolWeight = {qMolWeight}"; isSomething = true; }

                if (comboBox1.Text != "") { if (isSomething) Query += " AND "; Query += $"Class = {qClass}"; isSomething = true; }

                if (textBox1.Text != "") { if (isSomething) Query += " AND "; Query += $"ID = {qID}"; }
                ExecuteQuery(Query);
                UpdateDB();
                MessageBox.Show(qClass);
                MessageBox.Show($"Deleted {startRows - T.Rows.Count} value");
            }
            //check textBoxes
            if (checkBox1.Checked)
            {
                String Query = @"SELECT Mt.ID, Name, Formular,St.Class, MolWeight, State FROM Compounds as Mt
                                INNER JOIN Classes as St
                                ON Mt.Class = St.ID ";
                bool cID = checkBox6.Checked && textBox1.Text != "";
                bool cClass = checkBox5.Checked && comboBox1.Text != "";
                if (cID||cClass) Query += " WHERE ";
                if (cID) Query += $"Mt.ID = {textBox1.Text}";
                if (cID && cClass) Query += " AND ";
                if (cClass) Query += $"St.Class = N'{comboBox1.Text}'";
                using (SqlConnection Conn = new SqlConnection(DB))
                using (SqlCommand cmd = new SqlCommand(Query, Conn)) //for commands
                {
                        Conn.Open();
                using (SqlDataReader read = cmd.ExecuteReader())  // reader based on comand-object
                {
                    while (read.Read()) //go every line of table
                    {
                        textBox1.Text = (read["ID"].ToString());
                        textBox2.Text = (read["Name"].ToString());
                        textBox3.Text = (read["Formular"].ToString());
                        textBox4.Text = (read["State"].ToString());
                        textBox5.Text = (read["MolWeight"].ToString());
                        comboBox1.SelectedIndex = comboBox1.Items.IndexOf((read["Class"].ToString()));
                        break; //for ONLY first line
                        }
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(Query, DB);
                        da.Fill(ds); //filling info in ds 
                        T = ds.Tables[0];
                        dataGridView1.DataSource = T;
                    }
                }
                return;
              }
            if (checkBox2.Checked)
            {
                String Query = $@"   INSERT INTO Compounds (Name, Formular, Class, MolWeight, State)
                                    VALUES (N'{qName}', N'{qFormular}', {qClass}, {qMolWeight}, N'{qState}')";
                ExecuteQuery(Query);
                MessageBox.Show("Added 1 value");
            }
            if (checkBox3.Checked)
            {

                if (textBox2.Text.Trim() == "") { MessageBox.Show("Name is not found."); return; };
                if (textBox3.Text.Trim() == "") { MessageBox.Show("Formular is not found."); return; };
                if (textBox4.Text.Trim() == "") { MessageBox.Show("State is not found."); return; };
                String Query = $@"UPDATE Compounds set Name = N'{qName}', MolWeight = {qMolWeight}, Class = {qClass}, Formular = N'{qFormular}', State = N'{qState}' WHERE ID = {qID};";
                ExecuteQuery(Query);
                MessageBox.Show("Updated 1 value");
            }

            UpdateDB();
        }

        void UpdateDB2()
        {
            String Query = @"SELECT * FROM Classes";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                Conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(Query, DB);
                da.Fill(ds);
                T = ds.Tables[0];
                dataGridView2.DataSource = T;
            }
        }

        void EnableAllTextBoxes2(bool enable)
        {
            List<TextBox> textBoxes = new List<TextBox> { };
            panel2.Controls.OfType<TextBox>().ToList().ForEach(textB => textB.Enabled = enable);
            comboBox1.Enabled = enable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //panel1.Visible = false;
            //panel2.Visible = true;
            panel1.Location = new Point(-1200, -1200);
            panel2.Location = new Point(0, 0);
            UpdateDB2();
        }

        void CheckChoose2(CheckBox checkBox)
        {
            checkBox.Enabled = false;
            List<CheckBox> form1checks = new List<CheckBox> { checkBox12, checkBox10, checkBox9, checkBox11 };
            foreach (var i in form1checks)
                if (i != checkBox)
                {
                    i.Checked = false;
                    i.Enabled = true;
                }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose2(checkBox12);
            EnableAllTextBoxes2(true);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose2(checkBox11);
            EnableAllTextBoxes2(true);
            textBox10.Enabled = false;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose2(checkBox10);
            EnableAllTextBoxes2(true);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            CheckChoose2(checkBox9);
            EnableAllTextBoxes2(true);
        }

        //EXECUTE PANEL 2 BUTTON
        private void button4_Click(object sender, EventArgs e)
        {

            String qName = textBox9.Text != "" ? textBox9.Text : "Template",
                   qID = textBox10.Text != "" ? textBox10.Text : "26";
            //2
            if (checkBox11.Checked)
            {
                String Query = $@" INSERT INTO Classes (Class)
                                    VALUES (N'{qName}')";
                ExecuteQuery(Query);
                MessageBox.Show("Added 1 value");
            }
            if (!IntFormat(textBox10.Text.Trim())) { MessageBox.Show("WRONG ID!"); return;}
            //other

            if (checkBox9.Checked)
            {
                if (textBox9.Text.Trim() == "" && textBox10.Text.Trim() == "") { MessageBox.Show("What you need to delete???"); return; }
                String Query = $@"DELETE FROM Classes WHERE ";
                bool isSomething = false;
                UpdateDB2();
                int startRows = T.Rows.Count;

                if (textBox9.Text != "") { Query += $"Class = N'{qName}'"; isSomething = true; }
                if (textBox10.Text != "") { if (isSomething) Query += " AND "; Query += $"ID = {qID}"; }
                ExecuteQuery(Query);
                UpdateDB2();
                MessageBox.Show($"Deleted {startRows - T.Rows.Count} value");
            }

            if (checkBox12.Checked)
            {
                String Query = @"SELECT * FROM Classes";
                bool cID = textBox10.Text != "";
                bool cName = textBox9.Text != "";
                if (cID || cName) Query += " WHERE ";
                if (cID) Query += $"ID = {qID}";
                if (cID && cName) Query += " AND ";
                if (cName) Query += $"Class = N'{qName}'";
               
                using (SqlConnection Conn = new SqlConnection(DB))
                using (SqlCommand cmd = new SqlCommand(Query, Conn)) //for commands
                {
                    Conn.Open();
                    using (SqlDataReader read = cmd.ExecuteReader())  // reader based on comand-object
                    {
                        while (read.Read()) //go every line of table
                        {
                            textBox10.Text = (read["ID"].ToString());
                            textBox9.Text = (read["Class"].ToString());
                            break; //for ONLY first line
                        }
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(Query, DB);
                        da.Fill(ds); //filling info in ds 
                        T = ds.Tables[0];
                        dataGridView2.DataSource = T;
                    }
                }
                return;
            }

            if (checkBox10.Checked)
            {
                if (textBox9.Text.Trim() == "" && textBox10.Text.Trim() == "") { MessageBox.Show("What you need to update???"); return; }
                String Query = $@"UPDATE Classes set Class = N'{qName}' WHERE ID = {qID};";
                ExecuteQuery(Query);
                MessageBox.Show("Updated 1 value");
            }

            UpdateDB2();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //panel2.Visible = false;
            //panel1.Visible = true;

            panel2.Location = new Point(-1200, -1200);
            panel1.Location = new Point(0, 0);
            UpdateDB();
        }
    }
}
