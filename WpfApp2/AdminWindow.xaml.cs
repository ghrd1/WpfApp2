using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace WpfApp2
{
    public partial class AdminWindow : Window
    {
        private int currentCarIndex = 0;
        private int totalCarCount = 0;
        private OleDbConnection connection;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=RentCar.accdb";
        List <int> Number = new List <int>();

        public AdminWindow()
        {
            InitializeComponent();
            connection = new OleDbConnection(connectionString);
            connection.Open();
            totalCarCount = GetTotalCarCount();

            // Виклик методу для завантаження даних при завантаженні сторінки
            LoadCarData(currentCarIndex);
        }

        private int GetTotalCarCount()
        {
            try
            {
                // SQL-запит для підрахунку кількості записів в таблиці "Авто"
                string query = "SELECT COUNT(*) FROM Авто";

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    // ExecuteScalar для отримання одного значення (кількості записів)
                    int count = (int)cmd.ExecuteScalar();
                    return count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка отримання кількості записів: " + ex.Message);
                return 0;
            }
        }

        public void LoadCarData(int index)
        {
            try
            {
                string query = $"SELECT ID, Марка, Назва, Ціна, Фото, Розташування, Бронювання FROM Авто WHERE ID = {index + 1}";

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int carID = reader.GetInt32(0);
                        string carBrand = reader.GetString(1);
                        string carName = reader.GetString(2);
                        int carPrice = reader.GetInt32(3);
                        string imagePath = reader.GetString(4);
                        string carLocation = reader.GetString(5);
                        bool isBooked = reader.GetBoolean(6); 

                        carIDLabel.Content = carID.ToString();
                        carBrandLabel.Content = carBrand;
                        carNameLabel.Content = carName;
                        carPriceLabel.Content = carPrice.ToString();
                        carLocationLabel.Content = carLocation;

                        bronCheckBox.IsChecked = isBooked;


                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(imagePath);
                        bitmapImage.EndInit();

                        carImage.Source = bitmapImage;
                    }
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Помилка завантаження даних: " + ex.Message);
            }
        }


        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            Ціна.Visibility = Visibility.Visible;
            Марка.Visibility= Visibility.Visible;
            Модель.Visibility = Visibility.Visible;
            carPriceLabel.Visibility = Visibility.Hidden;
            carNameLabel.Visibility = Visibility.Hidden;
            carBrandLabel.Visibility = Visibility.Hidden;
            saveButton.Visibility = Visibility.Visible;
            changeButton.Visibility = Visibility.Hidden;
            carLocationTextBox.Visibility = Visibility.Visible;
            carLocationLabel.Visibility = Visibility.Hidden;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentCarIndex < totalCarCount - 1) 
            {
                currentCarIndex++;
                LoadCarData(currentCarIndex);
            }
            else
            {
                MessageBox.Show("Це останній автомобіль.");
            }
        }

        private void back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentCarIndex > 0)
            {
                currentCarIndex--; 
                LoadCarData(currentCarIndex);
            }
            else
            {
                MessageBox.Show("Це перший автомобіль.");
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string newCarPrice = Ціна.Text;
            string newCarBrand = Марка.Text;
            string newCarName = Модель.Text;
            string newLocation = carLocationTextBox.Text;
            bool isBooked = bronCheckBox.IsChecked ?? false; 

            if (!string.IsNullOrWhiteSpace(newCarPrice) &&
                !string.IsNullOrWhiteSpace(newCarBrand) &&
                !string.IsNullOrWhiteSpace(newCarName) &&
                !string.IsNullOrWhiteSpace(newLocation))
            {
                try
                {
                    string updateQuery = $"UPDATE Авто SET Ціна='{newCarPrice}', Марка='{newCarBrand}', Назва='{newCarName}', Розташування='{newLocation}', Бронювання={isBooked} WHERE ID={currentCarIndex + 1}";

                    using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            carPriceLabel.Content = newCarPrice;
                            carBrandLabel.Content = newCarBrand;
                            carNameLabel.Content = newCarName;
                            carLocationLabel.Content = newLocation;

                            Ціна.Visibility = Visibility.Hidden;
                            Марка.Visibility = Visibility.Hidden;
                            Модель.Visibility = Visibility.Hidden;
                            carLocationTextBox.Visibility = Visibility.Hidden;
                            carPriceLabel.Visibility = Visibility.Visible;
                            carBrandLabel.Visibility = Visibility.Visible;
                            carNameLabel.Visibility = Visibility.Visible;
                            carLocationLabel.Visibility = Visibility.Visible;

                            saveButton.Visibility = Visibility.Hidden;
                            changeButton.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("Помилка при оновленні даних в базі даних.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні даних: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть всі поля перед збереженням.");
            }

            Ціна.Text = string.Empty;
            Марка.Text = string.Empty;
            Модель.Text = string.Empty;
            carLocationTextBox.Text = string.Empty;
        }

        private void Add_Car_Click(object sender, RoutedEventArgs e)
        {
            Ціна.Visibility = Visibility.Visible;
            Марка.Visibility = Visibility.Visible;
            Модель.Visibility = Visibility.Visible;
            carPriceLabel.Visibility = Visibility.Hidden;
            carNameLabel.Visibility = Visibility.Hidden;
            carBrandLabel.Visibility = Visibility.Hidden;
            carLocationTextBox.Visibility = Visibility.Visible;
            carLocationLabel.Visibility = Visibility.Hidden;
            Save.Visibility = Visibility.Visible;
            Add_Car.Visibility = Visibility.Hidden;
            photo_Label.Visibility = Visibility.Visible;
            photo_TextBox.Visibility = Visibility.Visible;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string newCarPrice = Ціна.Text;
            string newCarBrand = Марка.Text;
            string newCarName = Модель.Text;
            string newLocation = carLocationTextBox.Text;
            string newPhoto = photo_TextBox.Text;
            bool isBooked = bronCheckBox.IsChecked ?? false;

            if (!string.IsNullOrWhiteSpace(newCarPrice) &&
                !string.IsNullOrWhiteSpace(newCarBrand) &&
                !string.IsNullOrWhiteSpace(newCarName) &&
                !string.IsNullOrWhiteSpace(newLocation) &&
                !string.IsNullOrWhiteSpace(newPhoto))  
            {
                try
                {
                    string insertQuery = $"INSERT INTO Авто (Марка, Назва, Ціна, Фото, Розташування, Бронювання) " +
                                         $"VALUES ('{newCarBrand}', '{newCarName}', '{newCarPrice}', '{newPhoto}', '{newLocation}', {isBooked})";

                    using (OleDbCommand cmd = new OleDbCommand(insertQuery, connection))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            currentCarIndex = GetTotalCarCount() - 1;
                            totalCarCount++;

                            LoadCarData(currentCarIndex);

                            MessageBox.Show("Новий автомобіль додано до бази даних.");
                        }
                        else
                        {
                            MessageBox.Show("Помилка при додаванні нового автомобіля до бази даних.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні нового автомобіля: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть всі поля перед збереженням.");
            }

            Ціна.Text = string.Empty;
            Марка.Text = string.Empty;
            Модель.Text = string.Empty;
            carLocationTextBox.Text = string.Empty;
            photo_TextBox.Text = string.Empty;

            carLocationTextBox.Visibility = Visibility.Hidden;
            Ціна.Visibility = Visibility.Hidden;
            Марка.Visibility = Visibility.Hidden;
            Модель.Visibility = Visibility.Hidden;
            Save.Visibility = Visibility.Hidden;
            Add_Car.Visibility = Visibility.Visible;
            photo_Label.Visibility = Visibility.Hidden;
            photo_TextBox.Visibility = Visibility.Hidden;
        }


    }

}
