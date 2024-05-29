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

namespace DictionaryApp
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private Word wordToEdit;
        private HashSet<string> categoriesSet;
        private ObservableCollection<Word> wordCollection;
        public EditWindow(Word word, ObservableCollection<Word> wordCollection, HashSet<string> categoriesSet)
        {
            InitializeComponent();
            wordToEdit = word;
            this.wordCollection = wordCollection;
            this.categoriesSet = categoriesSet;
            LoadWordData();
            LoadImage();
        }
        private void LoadWordData()
        {
            wordTextBox.Text = wordToEdit.Name;
            descriptionTextBox.Text = wordToEdit.Description;
            categoryTextBox.Text = wordToEdit.Category;
        }
        private void LoadImage()
        {
            if (!string.IsNullOrEmpty(wordToEdit.ImagePath))
            {
                try
                {
                    BitmapImage imageSource = new BitmapImage(new Uri(wordToEdit.ImagePath));
                    uploadedImage.Source = imageSource;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading the image: " + ex.Message);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Word word in wordCollection)
            {
                if (word.Name == wordToEdit.Name)
                {
                    categoriesSet.Remove(word.Category);

                    word.Name = wordTextBox.Text;
                    word.Description = descriptionTextBox.Text;
                    word.Category = categoryTextBox.Text;
                    word.ImagePath = wordToEdit.ImagePath; 

                    categoriesSet.Add(categoryTextBox.Text);

                    DataManager.SaveWordsToJson(wordCollection);

                    MessageBox.Show("Word updated successfully.");

                    Close();

                    return;
                }
            }

            MessageBox.Show("Failed to update word. Word not found in collection.");
        }


        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = DataManager.UploadImage();

            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    BitmapImage imageSource = new BitmapImage(new Uri(imagePath));
                    uploadedImage.Source = imageSource;
                    wordToEdit.ImagePath = imagePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading the image: " + ex.Message);
                }
            }
        }

    }
}
