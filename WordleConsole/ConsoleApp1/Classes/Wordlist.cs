using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    // Class used to save all Dictionaries as well as the Solved and Failed Attributes from each player session. 
    public class Wordlist
    {
        [JsonPropertyName("Words")]
        public List<String> Words { get; set; }
        [JsonPropertyName("Path")]
        public string Path { get; set; }

        //Constructor for deserialization
        public Wordlist()
        { }

        //Constructor that uses a relative Path to the File -- the int is awkward right now, but in that moment i had my attention on something else, will probably fix this in the future
        public Wordlist(int dummy)
        {
            Words = new List<String>();
            Path = "data\\words.txt";
           
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
                string temp = sr.ReadLine().Trim().ToUpper();
                Words.Add(temp);
            }

        }


    }
}
