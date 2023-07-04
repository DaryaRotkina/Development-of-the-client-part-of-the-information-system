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
    public partial class Form7MED : Form
    {
        public Form7MED()
        {
            InitializeComponent();
            // Для таблицы Дневника здоровья
           sqlDA1_DZ.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView3.DataSource = dataSet81.Дневник_здоровья;
            SelectDZ();
        }

        private void button1_Click(object sender, EventArgs e)// Вывод данных о детях
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

        private void button2_Click(object sender, EventArgs e)// Получить списки групп
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
                MessageBox.Show("Такой группы не существует илив группе нет ги одного ребенка");
                TB1_ListGr.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)// Вывод данных о родителях
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

        private void button4_Click(object sender, EventArgs e) // Получить номер телефона родителя
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

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView3_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.ThrowException = false;
        }

        //для обработки ошибок, возникающих при нарушении каких-либо ограничений или правил, не перехватываемых обработчиком DataError
        private static void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.Status == UpdateStatus.ErrorsOccurred)
            {
                MessageBox.Show(args.Errors.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }

        public void SelectDZ() // Метод для получения данных из таблицы Общие занятия
        {
            dataSet81.Дневник_здоровья.Clear();
            sqlDA1_DZ.Fill(dataSet81.Дневник_здоровья);
        }

        private void button5_Click(object sender, EventArgs e)// Обновить таблицу дневник здоровья
        {
            SelectDZ();
        }

        private void button6_Click(object sender, EventArgs e)// Сохранение данных в таблицу дневник здоровья
        {
            // First process deletes.
            sqlDA1_DZ.Update(dataSet81.Дневник_здоровья.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            sqlDA1_DZ.Update(dataSet81.Дневник_здоровья.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            sqlDA1_DZ.Update(dataSet81.Дневник_здоровья.Select(null, null, DataViewRowState.Added));
        }

        private void button7_Click(object sender, EventArgs e)// Список детей с определенной группой здоровья
        {
            if (textBox1.Text == "")
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
            cmd.CommandText = "ListGroupZD";

            cmd.Parameters.Add("@groupZD", SqlDbType.NVarChar).Value = textBox1.Text;
            SqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.HasRows)
            {
                listBox2.Items.Add("---------------------------");
                listBox2.Items.Add("Группа здоровья: " + textBox1.Text);
                while (rdr.Read())
                    listBox2.Items.Add(
                     rdr["Фамилия"].ToString().Trim() + "   " +
                     rdr["Имя"].ToString().Trim() + "   " +
                     rdr["Отчество"].ToString().Trim() + "   " +
                      rdr["Название"].ToString().Trim());
                rdr.Close();

                conn.Close();
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Такой группы не существует или к этой группе не относится ни один ребенок");
                textBox1.Text = "";
            }
        }

        private void button8_Click(object sender, EventArgs e)// Изменение пользователем пароля
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
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

            cmd.Parameters.Add("@Логин", SqlDbType.NVarChar).Value = textBox2.Text;
            cmd.Parameters.Add("@Пароль", SqlDbType.NVarChar).Value = textBox3.Text;
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
