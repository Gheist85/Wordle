using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    // Class that gets initiated when a new try is started. Does all the comparing and saves it so the Wordlepresenter can display it.
    internal class WordleLogic
    {
        public WordleSession Session { get; set; }
        public InputStringGetter Get { get; set; }
        public string CurrentWord { get; set; }
        public Dictionary<char,int> Lettercount { get; set; }
        public string CurrentTry { get; set; }
        public List<MatchPair> CurrentWordMatches { get; set; }

        //Constructor
        public WordleLogic (WordleSession session)
        {  
            Session = session;
            Get = new InputStringGetter();
            CurrentWordMatches = new List<MatchPair>();
        }

        //Class that purely checks for misinputs as per WORDLE rules and doesn't allow any to go through
        public void AskForInput()
        {
            // initialise variables
            string input;
            bool repeat = false;
            
            // loop  that only breaks when input is correct
            while (true)
            {
                repeat = false;
                input= Get.GetInputString().ToUpper();
                
                // check for word length and if all chars are actually letters. Only Works for the english Keyboard right now 
                if (!IsValidWord(input, CurrentWord.Length))
                    {                        
                    continue;
                    }

                // check if input is in the currently used Dictionary to prevent people solving WORDLE by just typing random letter sequences
                if (!Session.Dict.Words.Contains(input, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Word not found in currently used Dictionary");
                    continue;
                }
                // check if input matches a previous guess to help players out to not make repeat guesses
                for (int i = 0; i < CurrentWordMatches.Count; i++)
                {
                    if (CurrentWordMatches[i].Guess == input)
                    {
                        repeat = true;
                    }
                }
                if (repeat == true)
                {
                    Console.WriteLine("You already typed in that word. Make another guess.");
                    continue;
                }
                // breaks and escapes loop when word is valid and saves to CurrentTry
                break;
            }
            CurrentTry = input;


        }



        // Help Function to keep the above function shorter, checks for input length and char-quality
        private bool IsValidWord(string word, int length)
        {
            if (word.Length != length)
            {
                Console.WriteLine($"Wrong input. Input must be a {length}-letter Word\nPlease Try again.");
                return false;
            }
            if (!word.All(char.IsLetter))
            {
                Console.WriteLine("Wrong input. Word must be completely made of letters a-z\nPlease try again");
                return false;
            }
            return true;
        }

        // Method for randomly selecting a new word from the currently used dictionary
        public void PickWordFromValidSelection()
        {
            Random r = new Random();
            CurrentWord = Session.Dict.Words[r.Next(0, Session.Dict.Words.Count)];
            Lettercount = new Dictionary<char, int>();

            //initialize lettercount Dictionary for the CompareTrytoWord Logic
            foreach (char letter in CurrentWord)
            {
                if (!Lettercount.ContainsKey(letter))
                {
                    Lettercount.Add(letter, 1);
                }
                else
                {
                    Lettercount[letter]++;
                }
            }
        }
       
        // Method that compares they Players input to the sought after solution. Main component of this Program
        public void CompareTryToWord()
        {
            //Initialise a 2nd Dictionary to enable marking repeat letters correctly
            Dictionary<char,int> RepeatMatchdetector = new Dictionary<char,int>();

            // initialise a new WordleMatch[] to be saved into a match Pair later
            WordleMatch[] matches = new WordleMatch[CurrentWord.Length];

            // first loop - checks for all perfect matches (correct letter in correct position) marks them in the array and adds them to the repeatmatchdetector
            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (CurrentTry[i] == CurrentWord[i])
                {
                    matches[i] = (WordleMatch)3;
                    if (RepeatMatchdetector.TryGetValue(CurrentTry[i], out int currentValue))
                    {
                        RepeatMatchdetector[CurrentTry[i]] = currentValue + 1;
                    }
                    else
                    {
                        RepeatMatchdetector.Add(CurrentTry[i], 1);
                    }

                }
            }
            // first loop - checks for all general matches (correct letter wrong position) marks them in the array and adds them to the repeatmatchdetector.
            // This ignores aforegone perfect matches as well as repeats
            for (int i = 0; i < CurrentWord.Length; i++)
            {

                if (CurrentTry[i] != CurrentWord[i]
                    && CurrentWord.Contains(CurrentTry[i])
                    && RepeatMatchdetector.GetValueOrDefault(CurrentTry[i]) < Lettercount.GetValueOrDefault(CurrentTry[i])
                    && matches[i] != (WordleMatch)3)
                {
                    matches[i] = (WordleMatch)2;
                    if (RepeatMatchdetector.TryGetValue(CurrentTry[i], out int currentValue))
                    {
                        RepeatMatchdetector[CurrentTry[i]] = currentValue + 1;
                    }
                    else
                    {
                        RepeatMatchdetector.Add(CurrentTry[i], 1);
                    }
                }
            }

            //Last loop. Basically everything that didn't get marked by the first two loops gets marked now
            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (matches[i] != (WordleMatch)3 && matches[i] != (WordleMatch)2)
                    {

                    matches[i] = (WordleMatch)1;

                    }
            }
            // Save the MatchPair for display and later comparison
            MatchPair match = new MatchPair(CurrentTry, matches);
            CurrentWordMatches.Add(match); 
        }

        //Help function for the above logic
        public bool MatchesWord()
        {
            if (CurrentWord == CurrentTry)
            { return true; }
            else
            { return false; }
        }


    }
}
