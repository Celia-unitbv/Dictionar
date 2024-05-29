using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DictionaryApp
{
    public partial class AdministrativWindow : Window
    {
        public ObservableCollection<Word> wordCollection { get; set; }
        private string? imagePath;
        private HashSet<string> categoriesSet;

        public AdministrativWindow(ObservableCollection<Word> words)
        {
            InitializeComponent();
            wordCollection = words;
            DataContext = this;
            categoriesSet = new HashSet<string>(wordCollection.Select(word => word.Category));
        }

        

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            imagePath = DataManager.UploadImage();

            if (!string.IsNullOrEmpty(imagePath))
            {
                BitmapImage imageSource = new BitmapImage(new Uri("file:///" + imagePath.Replace("\\", "/")));
                uploadedImage.Source = imageSource;
            }
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            string wordText = wordTextBox.Text;
            string description = descriptionTextBox.Text;
            string category = categoryTextBox.Text;

            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(wordText))
            {
                MessageBox.Show("Please fill in the word, description, and category!");
                return;
            }

            if (!string.IsNullOrEmpty(imagePath))
            {

                try
                {
                    Word newWord = new Word(wordText, description, imagePath, category);
                    newWord.hasImage = true;
                    wordCollection.Add(newWord);

                    MessageBox.Show("Cuvântul a fost adăugat cu succes!");
              
                    imagePath = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "emptyImage.png");
                imagePath = defaultImagePath;

                try
                {
                    categoriesSet.Add(category);

                    BitmapImage imageSource = new BitmapImage(new Uri(defaultImagePath));
                    uploadedImage.Source = imageSource;
                    Word newWord = new Word(wordText, description, imagePath, category);
                   
                    wordCollection.Add(newWord);

                    MessageBox.Show("Cuvântul a fost adăugat cu succes!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            wordTextBox.Text = string.Empty;
            descriptionTextBox.Text = string.Empty;
            categoryTextBox.Text = string.Empty;
            uploadedImage.Source = null;
            imagePath = null;
            categoryListBox.ItemsSource = null;
            categoryListBox.Visibility = Visibility.Collapsed;

            DataManager.SaveWordsToJson(wordCollection);

        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (wordListBox.SelectedItem != null)
            {
                Word selectedWord = wordListBox.SelectedItem as Word;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the word \"{selectedWord.Name}\"?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    wordCollection.Remove(selectedWord);
                    categoriesSet.Remove(selectedWord.Category);

                    DataManager.SaveWordsToJson(wordCollection);

                    MessageBox.Show("Word deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a word to delete.");
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (wordListBox.SelectedItem != null)
            {
                Word selectedWord = wordListBox.SelectedItem as Word;

                EditWord(selectedWord, wordCollection);

            }
            else
            {
                MessageBox.Show("Please select a word to edit.");
            }
        }

        private void EditWord(Word word, ObservableCollection<Word> wordCollection)
        {
            EditWindow editWindow = new EditWindow(word, wordCollection,categoriesSet);

            editWindow.ShowDialog();

            wordListBox.ItemsSource = null; 
            wordListBox.ItemsSource = wordCollection;
        }

        private void categoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = categoryTextBox.Text.ToLower();

            var filteredCategories = categoriesSet.Where(category => category.ToLower().StartsWith(searchText)).ToList();

            categoryListBox.ItemsSource = filteredCategories;

            categoryListBox.Height = filteredCategories.Count * 22; 

            categoryListBox.Visibility = filteredCategories.Any() ? Visibility.Visible : Visibility.Collapsed;
        }



        private void categoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoryListBox.SelectedItem != null)
            {
                categoryTextBox.Text = categoryListBox.SelectedItem.ToString();
                categoryListBox.Visibility = Visibility.Collapsed;
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
