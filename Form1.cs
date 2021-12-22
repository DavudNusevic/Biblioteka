using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form1 : Form
    {
        SqlConnection cn;
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int id_grida;
        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(DBConnnection.getConnection());
            Load_Data();
        }

        void Load_Data()
        {
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM Autor", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3].ToString());
            }
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                try
                {
                    cn.Open();
                    cm = new SqlCommand(" INSERT INTO Autor (Naziv, DatumRodjenja, Biografija )" +
                                        " VALUES (@naziv, @datumRodjenja, @biografija)", cn);

                    cm.Parameters.AddWithValue("@naziv", textBox1.Text);
                    cm.Parameters.AddWithValue("@datumRodjenja", dateTimePicker1.Value);
                    cm.Parameters.AddWithValue("@biografija", textBox2.Text);

                    cm.ExecuteNonQuery();
                    MessageBox.Show("Uspešno ste dodadali novog učenika", "Ovbevestenje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cn.Close();

                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message);
                }
                Load_Data();
            }else
            {
                MessageBox.Show("Niste uneli potrebne podatke o uceniku", "Obavestenje", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da želite da izbrišete selektovani red?", "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand(" DELETE FROM Autor " +
                                        " WHERE id=" + id_grida, cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    Load_Data();
                    MessageBox.Show("Red je uspesno izbrisan", "Obaveštenje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            var s = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            id_grida = int.Parse(s);
            cn.Open();
            cm = new SqlCommand($"SELECT * FROM AUTOR WHERE ID = {id_grida}", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            textBox1.Text = dr[1].ToString();
            textBox2.Text = dr[3].ToString();
            dateTimePicker1.Value = DateTime.Parse(dr[2].ToString());
            cn.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("UPDATE Autor SET Naziv = @naziv, Biografija = @biografija, DatumRodjenja =@datumRodjenja" +
                $"                WHERE Id = {id_grida}", cn);
            cm.Parameters.AddWithValue("@naziv", textBox1.Text);
            cm.Parameters.AddWithValue("@datumRodjenja", dateTimePicker1.Value);
            cm.Parameters.AddWithValue("@biografija", textBox2.Text);

            cm.ExecuteNonQuery();
            cn.Close();
            Load_Data();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Load_Data_Search();

        }

        void Load_Data_Search()
        {
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand($"SELECT * FROM Autor WHERE Naziv LIKE '%{textBox3.Text}%' OR Biografija LIKE '%{textBox3.Text}%' OR DatumRodjenja LIKE'%{dateTimePicker2.Value}%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3].ToString());
            }
            cn.Close();
        }

    }
}
