using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace KURS
{
    public partial class Form4Pass : Form
    {
        public Form4Pass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == ""|| textBox2.Text == "" || textBox3.Text == "")
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
            cmd.CommandText = "New_pass";

            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@login", SqlDbType.NVarChar).Value = textBox2.Text;
            cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = textBox3.Text;

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;
           
                SqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                    dataGridView1.Rows.Add(
                     rdr["Фамилия"].ToString().Trim(),
                     rdr["Логин"].ToString().Trim(),
                     rdr["Пароль"].ToString().Trim());
                rdr.Close();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:

                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Вы не можете использовать данный логин, поскольку он уже принадлежит друому пользователю");
                        break;
                    case 2:

                        MessageBox.Show("У сотрудника уже имеется логин и пароль");
                        break;
                    case 3:

                        MessageBox.Show("Такого сотрудника не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }

                conn.Close();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Сотрудник.Фамилия, Логин, Пароль FROM Пароли JOIN Сотрудник ON Сотрудник = [№Сотрудника]";
            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView1.Rows.Add(
                 rdr["Фамилия"].ToString().Trim(),
                 rdr["Логин"].ToString().Trim(),
                 rdr["Пароль"].ToString().Trim());
            rdr.Close();
            conn.Close();
        }
    }
}
