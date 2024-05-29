using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.IO;


namespace DictionaryApp
{
    public partial class Login : Window
    {
        private ObservableCollection<Word> wordCollection;
        public Login(ObservableCollection<Word> words)
        {
            InitializeComponent();
            wordCollection = words;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e )
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            List<Account> accounts = new List<Account>();
            accounts = Account.ReadFromFile("C:\\Users\\Celia\\Documents\\AN2SEM2\\MVP\\DictionaryApp\\DictionaryApp\\credentials.txt");

            bool authenticationSuccessful = false;
            foreach (Account account in accounts)
            {
                if (account.Username == username && account.Password == password)
                {
                    AdministrativWindow adminWindow = new AdministrativWindow(wordCollection);
                    adminWindow.Show();
                    this.Close(); 
                    authenticationSuccessful = true;
                    break; 
                }
            }

            if (authenticationSuccessful)
            {
                MessageBox.Show("Autentificare reușită!");
            }
            else
            {
                MessageBox.Show("Nume de utilizator sau parolă incorectă!");
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
