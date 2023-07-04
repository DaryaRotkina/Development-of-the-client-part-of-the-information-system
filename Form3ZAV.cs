using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace KURS
{
    public partial class Form3ZAV : Form
    {
        public Form3ZAV()
        {
            InitializeComponent();
            // Для таблицы Сотрудник
            sqlDataAdapter1.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView1.DataSource = dataSet11.Сотрудник;
            SelectEmployee();
            // Для таблицы Дети
            sqlDA2_Children.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView2.DataSource = dataSet21.Ребенок;
            SelectChildren();
            // Для таблицы Родители
            sqlDA3_Parents.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView3.DataSource = dataSet31.Родитель;
            SelectParents();
            // Для групп
            sqlDA4_Group.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);
            dataGridView6.DataSource = dataSet51.Группа;
            SelectGroup();

        }

        private void Form3ZAV_Load(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Close();
        }

        private void button1_UpdSotr_Click(object sender, EventArgs e)// Обновление таблицы Сотрудник
        {
            SelectEmployee();
        }

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) 
        {
            MessageBox.Show(e.Exception.Message, "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void SelectEmployee() // Метод для получения данных из таблицы Сотрудник
        {
            dataSet11.Сотрудник.Clear();
            sqlDataAdapter1.Fill(dataSet11.Сотрудник);
        }

        private void button2_SaveSotr_Click(object sender, EventArgs e)// Сохранение таблицы сотрудник
        {
            // First process deletes.
            sqlDataAdapter1.Update(dataSet11.Сотрудник.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            sqlDataAdapter1.Update(dataSet11.Сотрудник.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            sqlDataAdapter1.Update(dataSet11.Сотрудник.Select(null, null, DataViewRowState.Added));
        }

        private void button3_NewPass_Click(object sender, EventArgs e)// Кнопка добавления нового пользователя
        {
            Form4Pass newPass = new Form4Pass();
            newPass.Show();
        }

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e) 
        {
            MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.ThrowException = false;
        }
        public void SelectChildren() // Метод для получения данных из таблицы Ребенок
        {
            dataSet21.Ребенок.Clear();
            sqlDA2_Children.Fill(dataSet21.Ребенок);
        }

        private void button1_UpdD_Click(object sender, EventArgs e)// Кнопка обновления таблицы Ребенок
        {
            SelectChildren();
        }

        private void button2_SaveD_Click(object sender, EventArgs e)// Сохранение таблицы ребенок
        {
            // First process deletes.
            sqlDA2_Children.Update(dataSet21.Ребенок.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            sqlDA2_Children.Update(dataSet21.Ребенок.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            sqlDA2_Children.Update(dataSet21.Ребенок.Select(null, null, DataViewRowState.Added));
        }

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView3_DataError(object sender, DataGridViewDataErrorEventArgs e) 
        {
            MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.ThrowException = false;
        }
        public void SelectParents() // Метод для получения данных из таблицы Родитель
        {
            dataSet31.Родитель.Clear();
            sqlDA3_Parents.Fill(dataSet31.Родитель);
        }

        private void button1_UpdR_Click(object sender, EventArgs e)// Обновление таблицы Родители
        {
            SelectParents();
        }

        private void button2_SaveR_Click(object sender, EventArgs e)// Сохранение данных таблицы Родители
        {
            // First process deletes.
            sqlDA3_Parents.Update(dataSet31.Родитель.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            sqlDA3_Parents.Update(dataSet31.Родитель.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            sqlDA3_Parents.Update(dataSet31.Родитель.Select(null, null, DataViewRowState.Added));
        }

        private void button1_Click(object sender, EventArgs e)// Кнопка для добавления Родства
        {
            Form5Parents form5 = new Form5Parents();
            form5.Show();
        }

        private void button2_Click(object sender, EventArgs e)// Кнопка обновления таблицы график работы
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT [График работы].Сотрудник, Сотрудник.Фамилия, Смена.Название, Дата FROM [График работы] JOIN Сотрудник ON [График работы].Сотрудник = Сотрудник.[№Сотрудника] JOIN Смена ON [График работы].Смена = Смена.[№Смены]";
            conn.Open();

            dataGridView4.Rows.Clear();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
                dataGridView4.Rows.Add(rdr["Сотрудник"].ToString(),
                 rdr["Фамилия"].ToString().Trim(),
                 rdr["Название"].ToString().Trim(),
                 rdr["Дата"].ToString().Trim());
            rdr.Close();
            conn.Close();
        }


        private void button3_Click(object sender, EventArgs e)// Обновление даты графика
        {
            if (textBox1.Text == "")
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
            cmd.CommandText = "Upd_Graph_Date";

            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@Дата1", SqlDbType.Date).Value = dateTimePicker2.Value;
            cmd.Parameters.Add("@Дата2", SqlDbType.Date).Value = dateTimePicker3.Value;

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

                SqlDataReader rdr = cmd.ExecuteReader();
                dataGridView4.Rows.Clear();
                while (rdr.Read())
                    dataGridView4.Rows.Add(rdr["Сотрудник"].ToString(),
                     rdr["Фамилия"].ToString().Trim(),
                     rdr["Название"].ToString().Trim(),
                     rdr["Дата"].ToString().Trim());
                rdr.Close();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:

                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Вы не можете изменить дату, поскольку сотрудник не работал в этот день или записи не существует");
                        break;
                    case 2:

                        MessageBox.Show("Такого сотрудника не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }

                conn.Close();
            textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)// Добавление графика
        {
            if (TB1_sm.Text == "" || TB2_Sotr.Text == "")
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
            cmd.CommandText = "New_Graph";

            cmd.Parameters.Add("@Смена", SqlDbType.Int).Value = TB1_sm.Text;
            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = TB2_Sotr.Text;
            cmd.Parameters.Add("@Дата", SqlDbType.Date).Value = dateTimePicker1.Value;

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;
            
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                dataGridView4.Rows.Clear();
                while (rdr.Read())
                    dataGridView4.Rows.Add(rdr["Сотрудник"].ToString(),
                     rdr["Фамилия"].ToString().Trim(),
                     rdr["Название"].ToString().Trim(),
                     rdr["Дата"].ToString().Trim());
                rdr.Close();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:

                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Такие данные уже существуют");
                        break;
                    case 2:

                        MessageBox.Show("Такой смены не существует");
                        break;
                    case 3:

                        MessageBox.Show("Такого сотрудника не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }

                conn.Close();
                TB1_sm.Text = "";
                TB2_Sotr.Text = "";
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Введены не корректные данные!");
            }
        }

        private void button5_Click(object sender, EventArgs e)// Обновление смены графика
        {
            if (textBox2.Text == "" || textBox3.Text == "")
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
            cmd.CommandText = "Upd_Graph";

            cmd.Parameters.Add("@Смена", SqlDbType.Int).Value = textBox2.Text;
            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = textBox3.Text;
            cmd.Parameters.Add("@Дата", SqlDbType.Date).Value = dateTimePicker4.Value;

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                dataGridView4.Rows.Clear();
                while (rdr.Read())
                    dataGridView4.Rows.Add(rdr["Сотрудник"].ToString(),
                     rdr["Фамилия"].ToString().Trim(),
                     rdr["Название"].ToString().Trim(),
                     rdr["Дата"].ToString().Trim());
                rdr.Close();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:

                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Вы не можете изменить смену, поскольку сотрудник не работал в этот день или записи не существует");
                        break;
                    case 2:

                        MessageBox.Show("Такой смены не существует");
                        break;
                    case 3:

                        MessageBox.Show("Такого сотрудника не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }

                conn.Close();
                textBox2.Text = "";
                textBox3.Text = "";
            }
            catch (System.FormatException) // System.Data.SqlClient.SqlException
            {
                MessageBox.Show("Введены не корректные данные!");
            }
        }

        private void button6_Click(object sender, EventArgs e)// Обновление оклада
        {
            if (textBox4.Text == "" )
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
            cmd.CommandText = "New_Salary";

            cmd.Parameters.Add("@Должность", SqlDbType.NVarChar).Value = comboBox1.SelectedItem;
            cmd.Parameters.Add("@Оклад", SqlDbType.Money).Value = textBox4.Text;
           

            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                dataGridView5.Rows.Clear();
                while (rdr.Read())
                    dataGridView5.Rows.Add(rdr["№Должности"].ToString(),
                     rdr["Название"].ToString().Trim(),
                     rdr["Оклад"].ToString().Trim(),
                     rdr["Рабочие часы"].ToString().Trim());
                rdr.Close();
                switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
                {
                    case 0:

                        MessageBox.Show("Операция прошла успешно");
                        break;
                    case 1:

                        MessageBox.Show("Такой должности не существует");
                        break;

                    default:

                        MessageBox.Show("Неизвестная ошибка");
                        break;
                }

                conn.Close();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Вы ввели не все значения!");
            }
            
            textBox4.Text = "";
            comboBox1.SelectedItem = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Должность";
            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            dataGridView5.Rows.Clear();
            while (rdr.Read())
                dataGridView5.Rows.Add(rdr["№Должности"].ToString(),
                 rdr["Название"].ToString().Trim(),
                 rdr["Оклад"].ToString().Trim(),
                 rdr["Рабочие часы"].ToString().Trim());
            rdr.Close();
            conn.Close();

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Заведующая");
            comboBox1.Items.Add("Заместитель заведующей");
            comboBox1.Items.Add("Бухгалтер");
            comboBox1.Items.Add("Логопед");
            comboBox1.Items.Add("Психолог");
            comboBox1.Items.Add("Музыкальный работник");
            comboBox1.Items.Add("Мед - работник");
            comboBox1.Items.Add("Воспитатель");
            comboBox1.Items.Add("Повар");
            comboBox1.Items.Add("Сторож");
            
            dataGridView7.Rows.Clear();
            SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
            conn.ConnectionString = ConfigurationManager.
            ConnectionStrings["Config"].ConnectionString;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Сотрудник.Фамилия,Сотрудник.Имя, Сотрудник.Отчество, Группа.Название, [Тип группы].Тип FROM [Ответственный за группу] JOIN Сотрудник ON [Ответственный за группу].Сотрудник = Сотрудник.[№Сотрудника] JOIN Группа ON [Ответственный за группу].Группа = Группа.[№Группы] JOIN [Тип группы] ON Группа.[Тип группы] = [Тип группы].[№Типа]";
            conn.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            dataGridView7.Rows.Clear();
            while (rdr.Read())
                dataGridView7.Rows.Add(rdr["Фамилия"].ToString(),
                rdr["Имя"].ToString().Trim(),
                rdr["Отчество"].ToString().Trim(),
                rdr["Название"].ToString().Trim(),
                rdr["Тип"].ToString().Trim());
            rdr.Close();
            conn.Close();
        }

        //обработчик будет «ловить» ошибки, связанные с нарушением типов данных, а также ограничением первичного ключа и not null-ограничения.
        private void dataGridView6_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.ThrowException = false;
        }
        public void SelectGroup() // Метод для получения данных из таблицы Группа
        {
            dataSet51.Группа.Clear();
            sqlDA4_Group.Fill(dataSet51.Группа);
        }

        private void Upd_Group_Click(object sender, EventArgs e)// Обновление таблицы Группа
        {
            SelectGroup();
        }

        private void Save_Group_Click(object sender, EventArgs e)// Сохранение изменений в таблице группа
        {
            // First process deletes.
            sqlDA4_Group.Update(dataSet51.Группа.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            sqlDA4_Group.Update(dataSet51.Группа.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            sqlDA4_Group.Update(dataSet51.Группа.Select(null, null, DataViewRowState.Added));
        }

        private void button8_Click(object sender, EventArgs e)// Добавление нового ответственного за группу
        {
            if (TB5_Ins.Text == "" || TB6_Ins.Text == "")
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
            cmd.CommandText = "New_Mentor";

            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = TB5_Ins.Text;
            cmd.Parameters.Add("@Группа", SqlDbType.NVarChar).Value = TB6_Ins.Text;


            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

            SqlDataReader rdr = cmd.ExecuteReader();
            dataGridView7.Rows.Clear();
            while (rdr.Read())
                dataGridView7.Rows.Add(rdr["Фамилия"].ToString(),
                 rdr["Имя"].ToString().Trim(),
                 rdr["Отчество"].ToString().Trim(),
                 rdr["Название"].ToString().Trim(),
                 rdr["Тип"].ToString().Trim());
            rdr.Close();
            switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
            {
                case 0:

                    MessageBox.Show("Операция прошла успешно");
                    break;
                case 1:

                    MessageBox.Show("Данный сотрудник уже отвечает за группу");
                    break;
                case 2:

                    MessageBox.Show("Данная группа не существует");
                    break;
                case 3:

                    MessageBox.Show("Данный сотрудник не является воспитателем, либо не существует");
                    break;
                default:

                    MessageBox.Show("Неизвестная ошибка");
                    break;
            }

            conn.Close();
            
            TB5_Ins.Text = "";
            TB6_Ins.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)// Удаление ответственного за группу
        {
            if (TB7_Del.Text == "" || TB8_Del.Text == "")
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
            cmd.CommandText = "Del_Mentor";

            cmd.Parameters.Add("@Сотрудник", SqlDbType.NVarChar).Value = TB7_Del.Text;
            cmd.Parameters.Add("@Группа", SqlDbType.NVarChar).Value = TB8_Del.Text;


            cmd.Parameters.Add("@Код", SqlDbType.Int);
            cmd.Parameters["@Код"].Direction = ParameterDirection.ReturnValue;

            SqlDataReader rdr = cmd.ExecuteReader();
            dataGridView7.Rows.Clear();
            while (rdr.Read())
                dataGridView7.Rows.Add(rdr["Фамилия"].ToString(),
                 rdr["Имя"].ToString().Trim(),
                 rdr["Отчество"].ToString().Trim(),
                 rdr["Название"].ToString().Trim(),
                 rdr["Тип"].ToString().Trim());
            rdr.Close();
            switch (Convert.ToInt32(cmd.Parameters["@Код"].Value))
            {
                case 0:

                    MessageBox.Show("Операция прошла успешно");
                    break;
                case 1:

                    MessageBox.Show("Такой строки не существует");
                    break;
                case 2:

                    MessageBox.Show("Данная группа не существует");
                    break;
                case 3:

                    MessageBox.Show("Данный сотрудник не является воспитателем, либо не существует");
                    break;
                default:

                    MessageBox.Show("Неизвестная ошибка");
                    break;
            }

            conn.Close();
          
            TB7_Del.Text = "";
            TB8_Del.Text = "";
        }

        private void button10_Click(object sender, EventArgs e) // Изменение пароля пользователем
        {
            if (textBox7.Text == "" || textBox6.Text == "" || textBox5.Text == "")
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

            cmd.Parameters.Add("@Логин", SqlDbType.NVarChar).Value = textBox7.Text;
            cmd.Parameters.Add("@Пароль", SqlDbType.NVarChar).Value = textBox6.Text;
            cmd.Parameters.Add("@Новый", SqlDbType.NVarChar).Value = textBox5.Text;
            if (textBox5.Text.Length < 15)
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
