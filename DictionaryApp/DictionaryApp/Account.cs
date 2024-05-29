using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp
{
    internal class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public static List<Account> ReadFromFile(string filename)
        {
            var accounts = new List<Account>();
            using(var reader = new StreamReader(filename))
            {
                string line;
                while((line = reader.ReadLine()) !=null)
                {
                    var part = line.Split(';');
                    if(part.Length == 2)
                    {
                        accounts.Add(new Account
                        {
                            Username = part[0],
                            Password = part[1]
                        });
                       
                    }
                }
            }
            return accounts;
        }
    }
}
