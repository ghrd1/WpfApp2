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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private OleDbConnection connection;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=RentCar.accdb";

        public MainWindow()
        {
            InitializeComponent();
            connection = new OleDbConnection(connectionString);
            connection.Open();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Обробка авторизації користувача
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            string query = "SELECT * FROM Користувачі WHERE Логін = @Username AND Пароль = @Password";

            using (OleDbCommand cmd = new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("Авторизація успішна!");
                        if (username == "admin")
                        {
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            Console.WriteLine("Успішна регістрація");
                            UserWindow userWindow = new UserWindow();
                            userWindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невдача у авторизації. Перевірте введені дані.");
                    }
                }
            }
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
           RegWindow regWindow = new RegWindow();
           regWindow.Show();
           this.Close();
        }
    }
}
