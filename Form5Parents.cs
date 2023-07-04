using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace KURS
{
    public partial class Form5Parents : Form
    {
        public Form5Parents()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TB1_IDC.Text == "" || TB2_IDR.Text == "" || TB3_Status.Text == "" || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Вы ввели не все данные!");
                return;
            }
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "New_Parents";

            cmd.Parameters.Add("@РебенокФ", SqlDbType.NVarChar).Value =TB1_IDC.Text;
            cmd.Parameters.Add("@РебенокИ", SqlDbType.NVarChar).Value = textBox2.Text;
            cmd.Parameters.Add("@РодительФ", SqlDbType.NVarChar).Value = TB2_IDR.Text;
            cmd.Parameters.Add("@РодительИ", SqlDbType.NVarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@Статус", SqlDbType.NVarChar).Value = TB3_Status.Text;

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;
            
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                    dataGridView1.Rows.Add(rdr[0].ToString(),
                     rdr[1].ToString().Trim(),
                     rdr[2].ToString().Trim(),
                     rdr[3].ToString().Trim(),
                     rdr[4].ToString().Trim());
                rdr.Close();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:

                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Такого родителя не существует");
                        break;
                    case 2:

                        MessageBox.Show("Такого ребенка не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }

                conn.Close();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("В базе уже имеется такая строка");
            }
            TB1_IDC.Text = "";
            TB2_IDR.Text = "";
            TB3_Status.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Родитель.Фамилия, Родитель.Имя, Ребенок.Фамилия AS ФамилияР , Ребенок.Имя AS ИмяР, Статус FROM Родство JOIN Ребенок ON Ребенок = [№Ребенка] JOIN Родитель ON Родитель = [№Родителя]";
            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView1.Rows.Add( rdr["Фамилия"].ToString(),
                 rdr["Имя"].ToString().Trim(),
                  rdr["ФамилияР"].ToString().Trim(),
                   rdr["ИмяР"].ToString().Trim(),
                 rdr["Статус"].ToString().Trim());
            rdr.Close();
            conn.Close();
        }
    }
}
