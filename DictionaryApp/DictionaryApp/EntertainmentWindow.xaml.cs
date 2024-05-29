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
    public partial class EntertainmentWindow : Window
    {
        private ObservableCollection<Word> wordCollection;
        int currentIndex;

        private List<Word> randomWords;
        private List<bool> displayModes;
        int correctAnswersCount;

        private List<string> userAnswers;

        public EntertainmentWindow(ObservableCollection<Word> words)
        {
            InitializeComponent();
            wordCollection = words;
            currentIndex = -1;
            displayModes = new List<bool>();
            correctAnswersCount = 0;
            userAnswers = new List<string>();

            
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            startButton.Visibility = Visibility.Hidden;
            randomWords = ChooseRandomWords(wordCollection, 5);

            Random random = new Random();
            displayModes.Clear(); 
            for (int i = 0; i < randomWords.Count; i++)
            {
                displayModes.Add(random.Next(2) == 0);
            }

            hintTextBlock.Visibility = Visibility.Visible;
            hintTextBox.Visibility = Visibility.Visible;
            answerTextBox.Visibility = Visibility.Visible;
            answerTextBlock.Visibility = Visibility.Visible;
            image.Visibility = Visibility.Visible;
            nextButton.Visibility = Visibility.Visible;

            previousButton.Visibility = Visibility.Hidden;


            DisplayNextWord();
        }

        private void DisplayNextWord()
        {
            if (currentIndex != 4)
            {
                currentIndex++;
                if(currentIndex==0)previousButton.Visibility = Visibility.Hidden;
                else previousButton.Visibility = Visibility.Visible;
            }
            else
            {
                nextButton.Visibility = Visibility.Hidden;
                finishButton.Visibility = Visibility.Visible;

            }
            if (currentIndex < randomWords.Count)
            {
                Word currentWord = randomWords[currentIndex];

                DescriptionOrImage(currentWord, displayModes[currentIndex]);

                answerTextBox.Text = "";
                answerTextBox.Visibility = Visibility.Visible;
                answerTextBlock.Visibility = Visibility.Visible;
                nextButton.Visibility = Visibility.Visible;
                //previousButton.Visibility = Visibility.Visible;
            }
            if (currentIndex == 4)
            {
                nextButton.Visibility = Visibility.Hidden;
                finishButton.Visibility = Visibility.Visible;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            userAnswers.Add(answerTextBox.Text);

            DisplayNextWord();

        }

        private List<Word> ChooseRandomWords(ObservableCollection<Word> sourceList, int count)
        {
            List<Word> result = new List<Word>();
            Random random = new Random();

            while (result.Count < count)
            {
                Word randomWord = sourceList[random.Next(sourceList.Count)];
                if (!result.Contains(randomWord))
                {
                    result.Add(randomWord);
                }
            }

            return result;
        }
        private void DescriptionOrImage(Word currentWord, bool displayDescription)
        {
            if (displayDescription || currentWord.hasImage==false)
            {
                hintTextBox.Text = currentWord.Description;
                image.Visibility = Visibility.Collapsed;
                hintTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                image.Source = new BitmapImage(new Uri(currentWord.ImagePath, UriKind.RelativeOrAbsolute));
                hintTextBox.Visibility = Visibility.Collapsed;
                image.Visibility = Visibility.Visible;
            }
        }

        private void DisplayPreviousWord()
        {
            if (currentIndex != 4)
            {
                nextButton.Visibility = Visibility.Visible;
                finishButton.Visibility = Visibility.Hidden;
                
            }
            if (currentIndex != 0)
            {
                currentIndex--;
                if (currentIndex == 0) previousButton.Visibility = Visibility.Hidden;
            }


                if (userAnswers.Count > 0)
            {
                userAnswers.RemoveAt(userAnswers.Count - 1);
            }


            if (currentIndex >= 0 && currentIndex < randomWords.Count)
            {
                Word currentWord = randomWords[currentIndex];
                DescriptionOrImage(currentWord, displayModes[currentIndex]);

            }
            if (currentIndex < 4)
            {
                nextButton.Visibility = Visibility.Visible;
                finishButton.Visibility = Visibility.Hidden;
            }

        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayPreviousWord();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            userAnswers.Add(answerTextBox.Text);

            for (int i = 0; i < randomWords.Count; i++)
            {
                if (randomWords[i].Name.ToLower() == userAnswers[i].ToLower())
                {
                    correctAnswersCount++;
                }
            }


            MessageBox.Show($"Ai răspuns corect la {correctAnswersCount} cuvinte.");

            currentIndex = -1;
            correctAnswersCount = 0;
            userAnswers.Clear();
            startButton.Visibility = Visibility.Visible;
            finishButton.Visibility = Visibility.Hidden;

            hintTextBlock.Visibility = Visibility.Hidden;
            hintTextBox.Visibility = Visibility.Hidden;
            answerTextBox.Visibility = Visibility.Hidden;
            answerTextBlock.Visibility = Visibility.Hidden;
            image.Visibility = Visibility.Hidden;
            nextButton.Visibility = Visibility.Hidden;
            previousButton.Visibility = Visibility.Hidden;
        }
        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


    }
}
