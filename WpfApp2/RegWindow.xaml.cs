using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class RegWindow : MainWindow
    {
        private OleDbConnection connection;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=RentCar.accdb";
        public RegWindow()
        {
            InitializeComponent();
            connection = new OleDbConnection(connectionString);
            connection.Open();
        }

        private void RegisterUserButton_Click(object sender, RoutedEventArgs e)
        {
            string registerUsername = registerUsernameTextBox.Text;
            string registerPassword = registerPasswordBox.Password;

            string insertQuery = "INSERT INTO Користувачі (Логін, Пароль) VALUES (@Username, @Password)";

            using (OleDbCommand cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@Username", registerUsername);
                cmd.Parameters.AddWithValue("@Password", registerPassword);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Реєстрація успішна! Ви можете увійти зараз.");
                }
                else
                {
                    MessageBox.Show("Помилка під час реєстрації. Спробуйте ще раз.");
                }
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
