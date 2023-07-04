using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace KURS
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection();   //Подключаемся к БД с помощью конфигурационного файла
                conn.ConnectionString = ConfigurationManager.
                ConnectionStrings["Config"].ConnectionString;


                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Login_Users";
                cmd.Parameters.Add("@login", SqlDbType.NVarChar).Value = login.Text;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password.Text;

                int result = Convert.ToInt32(cmd.ExecuteScalar());// получаем первый столбец первой строки результирующего набора и преобразуем тип данных
                if (result == 1 || result == 2) // Если должность - Заведующая или заместитель
                {
                    Form3ZAV newForm3 = new Form3ZAV();
                    newForm3.ShowDialog();
                    login.Text = "";
                    password.Text = "";
                }
                else if (result == 8) // Если должность - Воспитатель
                {
                    Form6MENTOR newForm6 = new Form6MENTOR();
                    newForm6.ShowDialog();
                    login.Text = "";
                    password.Text = "";
                }
                else if (result == 7) // Если должность - Мед работник
                {
                    Form7MED form7 = new Form7MED();
                    form7.ShowDialog();
                    login.Text = "";
                    password.Text = "";
                }
                else if (result == 4 || result == 5 || result == 6) // Если должность - Логопед или Психолог или Музыкант
                {
                    Form8SPEC form8 = new Form8SPEC();
                    form8.ShowDialog();
                    login.Text = "";
                    password.Text = "";
                }
                else // Если пользователь не зарегистрирован или не верно ввел данные
                {
                    MessageBox.Show("Вы не можете войти в свой профиль, неверный логин или пароль.", "Ошибка авторизации",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                conn.Close();
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Проверьте соединение с сервером!", "Ошибка доступа",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
