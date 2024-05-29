using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DictionaryApp
{
    public class DataManager
    {
        public static void SaveWordsToJson(ObservableCollection<Word> wordCollection)
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionar.json");

            try
            {
                string jsonString = JsonSerializer.Serialize(wordCollection);

                File.WriteAllText(jsonFilePath, jsonString);

                MessageBox.Show("Words have been saved to Words.json.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving words to JSON file: " + ex.Message);
            }
        }

        public static void LoadWordsFromJson(ObservableCollection<Word> wordCollection)
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionar.json");
            try
            {
                if (File.Exists(jsonFilePath))
                {
                    string jsonString = File.ReadAllText(jsonFilePath);
                    wordCollection.Clear();
                    var loadedWords = JsonSerializer.Deserialize<ObservableCollection<Word>>(jsonString);

                    foreach (var word in loadedWords)
                    {
                        wordCollection.Add(word);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading words from JSON file: " + ex.Message);
            }
        }

        public static string UploadImage()
        {
            string imagePath = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            string resourcesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

            if (!Directory.Exists(resourcesDirectory))
            {
                Directory.CreateDirectory(resourcesDirectory);
            }

            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                string imageName = Path.GetFileName(filename);
                string destinationPath = Path.Combine(resourcesDirectory, imageName);

                try
                {
                    File.Copy(filename, destinationPath, true);
                    imagePath = destinationPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while copying the image: " + ex.Message);
                }
            }

            return imagePath;
        }
    }
}
