using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    public class Wordlist
    {
        public List<String> Words;
        public string Path;

        //Constructor that uses a relative Path to the File
        public Wordlist()
        {
            Words = new List<String>();
            Path = "data\\words.txt";
            ReadListFromFile();
        }

        // Constructor that uses a CustomPath to the File

        public Wordlist(string path)
        {
            Words = new List<String>();
            Path = path;
            ReadListFromFile() ;
        }



        //Method to get comparative wordlist from File in Path
        public void ReadListFromFile()
        {
            StreamReader sr = new StreamReader(Path);
            while (!sr.EndOfStream)
            {
                string temp = sr.ReadLine().Trim();
                Words.Add(temp);
            }

        }


    }
}
