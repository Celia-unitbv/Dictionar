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
using System.Windows.Navigation;

using System.IO;

namespace DictionaryApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Word> wordCollection;

        public MainWindow()
        {
            InitializeComponent();
            wordCollection = new ObservableCollection<Word>();
            LoadWordsFromJson();
        }

        private void LoadWordsFromJson()
        {
            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "emptyImage.png");
            DataManager.LoadWordsFromJson(wordCollection);

            foreach (Word word in wordCollection)
            {
                if (word.ImagePath == defaultImagePath)
                {
                    word.hasImage = false;
                }
            }
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Login adminWindow = new Login(wordCollection);
            adminWindow.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow(wordCollection);
            searchWindow.Show();
            this.Close();
        }

        private void EntertainmentButton_Click(object sender, RoutedEventArgs e)
        {
            EntertainmentWindow entertainmentWindow = new EntertainmentWindow(wordCollection);
            entertainmentWindow.Show();
            this.Close();
        }
    }
}
