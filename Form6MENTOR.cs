using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace KURS
{
    public partial class Form6MENTOR : Form
    {
        public Form6MENTOR()
        {
            InitializeComponent();
            // Для таблицы Общих занятий
            sqlDA1_ZanO.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView4.DataSource = dataSet61.Общие_занятия;
            SelectZanO();
            // Для таблицы Индивидуальных занятий
            sqlDA2_ZanI.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView5.DataSource = dataSet71.Индивидуальные_занятия;
            SelectZanI();
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

        private void button2_Click(object sender, EventArgs e)// Список детей в определенной группе
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

        private void button3_Click(object sender, EventArgs e)// Вывод даннных о родителях
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

        private void button4_Click(object sender, EventArgs e) // Получение номера телефона родителя
        {
            if(TB1_Surn.Text == "" || TB2_Name.Text == "")
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

        private void button5_Click(object sender, EventArgs e)// Вывод таблицы график
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Фамилия, Имя, Смена, Дата FROM[График работы] JOIN Сотрудник ON[График работы].Сотрудник = Сотрудник.[№Сотрудника] ";
            conn.Open();

            dataGridView3.Rows.Clear();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView3.Rows.Add(rdr["Фамилия"].ToString(),
                 rdr["Имя"].ToString(),
                  rdr["Смена"].ToString(),
                 rdr["Дата"].ToString());
            rdr.Close();
            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)// вывод смены по дате
        {
            if (textBox1.Text == "" || textBox2.Text == "")
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
            cmd.CommandText = "Graph_1";

            cmd.Parameters.Add("@Фамилия", SqlDbType.NVarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@Дата", SqlDbType.Date).Value = textBox2.Text;
            
            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;
            try
            {
                textBox3.Text = "";
                string res = cmd.ExecuteScalar().ToString();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:
                        textBox3.Text = res;
                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Такой записи не существует");

                        break;
                    case 2:

                        MessageBox.Show("Такого сотрудника не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Вы ввели не корректные данные!");
            }
           
            conn.Close();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView4_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
        public void SelectZanO() // Метод для получения данных из таблицы Общие занятия
        {
            dataSet61.Общие_занятия.Clear();
            sqlDA1_ZanO.Fill(dataSet61.Общие_занятия);
        }

        private void button7_Click(object sender, EventArgs e)// Обновление таблицы общие занятия
        {
            SelectZanO();
        }

        private void button8_Click(object sender, EventArgs e)// Сохранение изменений в общих занятиях
        {
            // First process deletes.
           sqlDA1_ZanO.Update(dataSet61.Общие_занятия.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
           sqlDA1_ZanO.Update(dataSet61.Общие_занятия.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
           sqlDA1_ZanO.Update(dataSet61.Общие_занятия.Select(null, null, DataViewRowState.Added));
        }

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView5_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.ThrowException = false;
        }

        public void SelectZanI() // Метод для получения данных из таблицы Общие занятия
        {
            dataSet71.Индивидуальные_занятия.Clear();
            sqlDA2_ZanI.Fill(dataSet71.Индивидуальные_занятия);
        }

        private void button10_Click(object sender, EventArgs e)// Обновление таблицы индивидуальные занятия
        {
            SelectZanI();
        }

        private void button9_Click(object sender, EventArgs e)// Сохранение изменений в общих занятиях
        {
            // First process deletes.
            sqlDA2_ZanI.Update(dataSet71.Индивидуальные_занятия.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            sqlDA2_ZanI.Update(dataSet71.Индивидуальные_занятия.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            sqlDA2_ZanI.Update(dataSet71.Индивидуальные_занятия.Select(null, null, DataViewRowState.Added));
        }

        private void button11_Click(object sender, EventArgs e)// Изменение пароля пользователем
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
