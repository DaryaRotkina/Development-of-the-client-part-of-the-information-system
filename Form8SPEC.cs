using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace KURS
{
    public partial class Form8SPEC : Form
    {
        public Form8SPEC()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)// Обновление таблицы дети
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Фамилия, Имя, Отчество, [Дата рождения], Группа.Название FROM Ребенок JOIN Группа ON Ребенок.Группа = [№Группы]";
            conn.Open();

            dataGridView1.Rows.Clear();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView1.Rows.Add(
                 rdr["Фамилия"].ToString(),
                 rdr["Имя"].ToString(),
                  rdr["Отчество"].ToString(),
                  rdr["Дата рождения"].ToString(),
                 rdr["Название"].ToString());
            rdr.Close();
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)// Вывод списка по группам
        {
            if (TB1_ListGr.Text == "")
            {
                MessageBox.Show("Вы не ввели данные");
                return;
            }
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ListGroup";

            cmd.Parameters.Add("@group", SqlDbType.NVarChar).Value = TB1_ListGr.Text;
            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                listBox1.Items.Add("---------------------------");
                listBox1.Items.Add("Группа: " + TB1_ListGr.Text);
                while (rdr.Read())
                    listBox1.Items.Add(
                     rdr["Фамилия"].ToString().Trim() + "\t" +
                     rdr["Имя"].ToString().Trim() + "\t" +
                      rdr["Отчество"].ToString().Trim());
                rdr.Close();

                conn.Close();
                TB1_ListGr.Text = "";
            }
            else
            {
                MessageBox.Show("Такой группы не существует или ни один ребенок не вхоит в эту группу");
                TB1_ListGr.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)// Заполнение таблицы Родители
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Родитель";
            conn.Open();

            dataGridView2.Rows.Clear();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView2.Rows.Add(rdr["№Родителя"].ToString(),
                 rdr["Фамилия"].ToString(),
                 rdr["Имя"].ToString(),
                  rdr["Отчество"].ToString(),
                  rdr["Телефон"].ToString(),
                   rdr["Паспорт"].ToString(),
                 rdr["Адрес"].ToString());
            rdr.Close();
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)// Получение номера телефона
        {
            if (TB1_Surn.Text == "" || TB2_Name.Text == "")
            {
                MessageBox.Show("Вы ввели не все данные!");
                return;
            }
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Phone";

            cmd.Parameters.Add("@Фамилия", SqlDbType.NVarChar).Value = TB1_Surn.Text;
            cmd.Parameters.Add("@Имя", SqlDbType.NVarChar).Value = TB2_Name.Text;

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

            string res = cmd.ExecuteScalar().ToString();// получаем первый столбец первой строки результирующего набора и преобразуем тип данных
            Phone.Text = "";
            switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
            {
                case 0:
                    Phone.Text = res;
                    MessageBox.Show("Операция прошла успешно");
                    break;
                case 1:

                    MessageBox.Show("Такого родителя не существует");

                    break;
                default:

                    MessageBox.Show("Неизвестная ошибка");
                    break;
            }
            conn.Close();
            TB1_Surn.Text = "";
            TB2_Name.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)// Вывод таблицы Общие занятия
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Фамилия, Занятие.Название, Группа.Название AS Группа, [День недели], Время FROM[Общие занятия] JOIN Сотрудник ON Сотрудник = [№Сотрудника] JOIN Занятие ON Занятие = [№Занятия] JOIN Группа ON Группа = Группа.[№Группы]";
            conn.Open();

            dataGridView3.Rows.Clear();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView3.Rows.Add(
                 rdr["Фамилия"].ToString(),
                 rdr["Название"].ToString(),
                  rdr["Группа"].ToString(),
                  rdr["День недели"].ToString(),
                 rdr["Время"].ToString());
            rdr.Close();
            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)// Вывод таблицы индивидуальные занятия
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Сотрудник.Фамилия, Занятие.Название, Ребенок.Фамилия AS ФамР, Ребенок.Имя AS ИмяР, Дата, Время FROM [Индивидуальные занятия] JOIN Сотрудник ON Сотрудник = [№Сотрудника] JOIN Занятие ON Занятие = [№Занятия] JOIN Ребенок ON Ребенок = [№Ребенка]";

            conn.Open();

            dataGridView4.Rows.Clear();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView4.Rows.Add(
                 rdr["Фамилия"].ToString(),
                 rdr["Название"].ToString(),
                  rdr["ФамР"].ToString(),
                   rdr["ИмяР"].ToString(),
                  rdr["Дата"].ToString(),
                 rdr["Время"].ToString());
            rdr.Close();
            conn.Close();
        }

        private void button7_Click(object sender, EventArgs e)// Вывод списка индивидуальных занятий у специалиста
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Вы ввели не все данные!");
                return;
            }
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ListZan";

            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@Дата", SqlDbType.Date).Value = dateTimePicker1.Value;

            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                listBox2.Items.Add("---------------------------");
                while (rdr.Read())
                    listBox2.Items.Add(
                     rdr["Фамилия"].ToString().Trim() + "   " +
                     rdr["Название"].ToString().Trim() + "   " +
                      rdr["ФамилияР"].ToString().Trim() + "   " +
                      rdr["ИмяР"].ToString().Trim() + "   " +
                       rdr["Дата"].ToString().Trim() + "   " +
                      rdr["Время"].ToString().Trim());
                rdr.Close();
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Такого сотрудника не существует или у него нет занятий");
                textBox1.Text = "";
            }
            conn.Close();
        }

        private void button11_Click(object sender, EventArgs e)// Изменение пользователем пароля
        {
            if (textBox6.Text == "" || textBox5.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Вы указали не все даннные!");
                return;
            }
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Upd_Pass";

            cmd.Parameters.Add("@Логин", SqlDbType.NVarChar).Value = textBox6.Text;
            cmd.Parameters.Add("@Пароль", SqlDbType.NVarChar).Value = textBox5.Text;
            cmd.Parameters.Add("@Новый", SqlDbType.NVarChar).Value = textBox4.Text;
            if (textBox4.Text.Length < 15)
            {
                int result = Convert.ToInt32(cmd.ExecuteScalar());// получаем первый столбец первой строки результирующего набора и преобразуем тип данных

                if (result == 0) // Такого пароля не существует
                {
                    MessageBox.Show("Указанный Вами пароль и логин не существуют!");
                }
                else
                {
                    MessageBox.Show("Операция прошла успешно");
                }
            }
            else
            {
                MessageBox.Show("Вы ввели слишком длинный пароль, длина пароля должна быть меньше 15 символов!");
            }
        }
    }
}
