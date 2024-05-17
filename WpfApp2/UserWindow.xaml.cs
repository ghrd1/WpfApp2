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
    public partial class UserWindow : Window
    {
        private int currentCarIndex = 0;
        private int totalCarCount = 0;
        private OleDbConnection connection;
        private List<int> searchResults = new List<int>();
        private int currentResultIndex = 0;

        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=RentCar.accdb";

        public UserWindow()
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

        private void LoadCarData(int index)
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
                currentCarIndex--; // Зменшити індекс для переходу до попереднього запису
                LoadCarData(currentCarIndex); // Перезавантаження даних для попереднього запису
            }
            else
            {
                MessageBox.Show("Це перший автомобіль.");
            }
        }

        private void price_Button_Click(object sender, RoutedEventArgs e)
        {
            Розташування_Label.Visibility = Visibility.Hidden;
            Ціна_Label.Visibility = Visibility.Hidden;
            Марка_Label.Visibility = Visibility.Hidden;
            Модель_Label.Visibility = Visibility.Hidden;
            Номер_Label.Visibility = Visibility.Hidden;
            carBrandLabel.Visibility = Visibility.Hidden;
            carIDLabel.Visibility = Visibility.Hidden;
            carLocationLabel.Visibility = Visibility.Hidden;
            carNameLabel.Visibility = Visibility.Hidden;
            carPriceLabel.Visibility = Visibility.Hidden;
            bronCheckBox.Visibility = Visibility.Hidden;
            price_Button.Visibility = Visibility.Hidden;
            Дні.Visibility = Visibility.Visible;
            Години.Visibility = Visibility.Visible;
            count_Button.Visibility = Visibility.Visible;
            Дні_textBox.Visibility = Visibility.Visible;
            Години_textBox.Visibility = Visibility.Visible;
        }

        private void count_Button_Click(object sender, RoutedEventArgs e)
        {
            int price = ((int.Parse(Дні_textBox.Text) * 24) + int.Parse(Години_textBox.Text)) * int.Parse(carPriceLabel.Content.ToString());
            MessageBox.Show($"Ціна за ваш час оренди: {price.ToString()}");

            Розташування_Label.Visibility = Visibility.Visible;
            Ціна_Label.Visibility = Visibility.Visible;
            Марка_Label.Visibility = Visibility.Visible;
            Модель_Label.Visibility = Visibility.Visible;
            Номер_Label.Visibility = Visibility.Visible;
            carBrandLabel.Visibility = Visibility.Visible;
            carIDLabel.Visibility = Visibility.Visible;
            carLocationLabel.Visibility = Visibility.Visible;
            carNameLabel.Visibility = Visibility.Visible;
            carPriceLabel.Visibility = Visibility.Visible;
            bronCheckBox.Visibility = Visibility.Visible;
            price_Button.Visibility = Visibility.Visible;
            Дні.Visibility = Visibility.Hidden;
            Години.Visibility = Visibility.Hidden;
            count_Button.Visibility = Visibility.Hidden;
            Дні_textBox.Visibility = Visibility.Hidden;
            Години_textBox.Visibility = Visibility.Hidden; 
            Дні_textBox.Text = string.Empty;
            Години_textBox.Text = string.Empty;

            price = 0;
        }

        private void bron_Button_Click(object sender, RoutedEventArgs e)
        {

            if (bronCheckBox.IsChecked == true)
            {
                MessageBox.Show("Авто вже заброньовано");
            }
            else
            {
                MessageBox.Show("Ваша заявка передана адміністратору. Очікуйте дзвінка.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchBrand = searchTextBox.Text;

                string query = "SELECT ID FROM Авто Запрос WHERE Марка = @Brand";

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    // Параметр для марки авто
                    cmd.Parameters.AddWithValue("@Brand", searchBrand);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        searchResults.Clear();

                        while (reader.Read())
                        {
                            searchResults.Add(reader.GetInt32(0)-1);
                        }

                        if (searchResults.Count > 0)
                        {
                            // Якщо є результати пошуку, вивести перший знайдений запис
                            currentResultIndex = 0;
                            LoadCarData(searchResults[currentResultIndex]);
                        }
                        else
                        {
                            // Якщо нічого не знайдено
                            MessageBox.Show("Автомобіль з введеною маркою не знайдено.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при виконанні пошуку: " + ex.Message);
            }
        }

        private void next_serButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchResults.Count > 0 && currentResultIndex < searchResults.Count - 1)
            {
                currentResultIndex++;
                LoadCarData(searchResults[currentResultIndex]);
            }
            else
            {
                MessageBox.Show("Немає більше результатів пошуку.");
            }
        }

        private void back_serButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchResults.Count > 0 && currentResultIndex > 0)
            {
                currentResultIndex--;
                LoadCarData(searchResults[currentResultIndex]);
            }
            else
            {
                MessageBox.Show("Це перший результат пошуку.");
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void price_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный текст числом
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Отменяем ввод, если не является числом
            }
        }

        private void SearchByPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchPrice = price_TextBox.Text;

                string query = "SELECT ID FROM Авто Ціна WHERE Ціна = @Price";

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    // Параметр для марки авто
                    cmd.Parameters.AddWithValue("@Price", searchPrice);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        searchResults.Clear();

                        while (reader.Read())
                        {
                            searchResults.Add(reader.GetInt32(0) - 1);
                        }

                        if (searchResults.Count > 0)
                        {
                            // Якщо є результати пошуку, вивести перший знайдений запис
                            currentResultIndex = 0;
                            LoadCarData(searchResults[currentResultIndex]);
                        }
                        else
                        {
                            // Якщо нічого не знайдено
                            MessageBox.Show("Автомобіль з введеною ціною не знайдено.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при виконанні пошуку: " + ex.Message);
            }
        }

        private void SearchByCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchCity = city_TextBox.Text;

                string query = "SELECT ID FROM Авто Розташування WHERE Розташування = @City";

                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    // Параметр для марки авто
                    cmd.Parameters.AddWithValue("@City", searchCity);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        searchResults.Clear();

                        while (reader.Read())
                        {
                            searchResults.Add(reader.GetInt32(0) - 1);
                        }

                        if (searchResults.Count > 0)
                        {
                            // Якщо є результати пошуку, вивести перший знайдений запис
                            currentResultIndex = 0;
                            LoadCarData(searchResults[currentResultIndex]);
                        }
                        else
                        {
                            // Якщо нічого не знайдено
                            MessageBox.Show("Автомобіль з введеною ціною не знайдено.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при виконанні пошуку: " + ex.Message);
            }
        }
    }
}

