using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp
{
    public class Word
    {
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string ImagePath { get; set; } 
        public string Category { get; set; } 

        public bool hasImage { get; set; }


        // Constructor
        public Word(string name, string description, string imagePath, string category)
        {
            Name = name;
            Description = description;
            ImagePath = imagePath;
            Category = category;
            hasImage = true;
        }
        


    }
}
