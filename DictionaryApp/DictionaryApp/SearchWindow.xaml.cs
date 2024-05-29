using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;

namespace DictionaryApp
{
    public partial class SearchWindow : Window
    {
        private ObservableCollection<Word> wordCollection;
        private HashSet<string> categoriesSet;
        public HashSet<string> CategoriesSet
        {
            get { return categoriesSet; }
            set { categoriesSet = value; }
        }
        public SearchWindow(ObservableCollection<Word> words)
        {
            InitializeComponent();
            wordCollection = words;

            DataContext = this;

            DataManager.LoadWordsFromJson(wordCollection);

            CategoriesSet = new HashSet<string>(wordCollection.Select(word => word.Category));

        }

        private void wordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string searchText = wordTextBox.Text.ToLower();


            string selectedCategory = (string)categoryComboBox.SelectedItem;

            var filteredWords = wordCollection
                .Where(word =>
                    (selectedCategory == null || word.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)) &&
                    word.Name.ToLower().StartsWith(searchText))
                .Select(word => word.Name)
                .ToList();

            wordListBox.ItemsSource = filteredWords;

            wordListBox.Height = filteredWords.Count * 22;

            wordListBox.Visibility = filteredWords.Any() ? Visibility.Visible : Visibility.Collapsed;
        }





        private void wordListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (wordListBox.SelectedItem != null)
            {
                wordTextBox.Text = wordListBox.SelectedItem.ToString();
                wordListBox.Visibility = Visibility.Collapsed;
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoryComboBox.SelectedItem != null)
            {

                string selectedCategory = categoryComboBox.SelectedItem.ToString();

                var filteredWords = wordCollection
                    .Where(word => word.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase))
                    .Select(word => word.Name)
                    .ToList();

                wordListBox.ItemsSource = filteredWords;

                wordListBox.Height = filteredWords.Count * 22;
            }
        }

        private void wordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(wordTextBox.Text))
            {
                wordListBox.Height = 1;
                wordListBox.Visibility = Visibility.Visible;
            }
        }

        private void wordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            wordListBox.Visibility = Visibility.Collapsed;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = wordTextBox.Text.ToLower();

            Word foundWord = wordCollection.FirstOrDefault(word => word.Name.ToLower() == searchText);

            if (foundWord != null)
            {

                categoryTextBox.Text = foundWord.Category;
                descriereTextBox.Text = foundWord.Description;

                if (File.Exists(foundWord.ImagePath))
                {
                    BitmapImage imageSource = new BitmapImage(new Uri(foundWord.ImagePath));
                    imageView.Source = imageSource;
                }
                else
                { 
                    imageView.Source = null;
                }
            }
            else
            {
                MessageBox.Show("Cuvântul nu a fost găsit.");
            }
        }
        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }


}
