using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    internal class WordleLogic
    {
        public WordleSession Session { get; set; }
        public InputStringGetter Get { get; set; }
        public string CurrentWord { get; set; }
        public Dictionary<char,int> Lettercount { get; set; }
        public string CurrentTry { get; set; }
        public List<MatchPair> CurrentWordMatches { get; set; }

        public WordleLogic (WordleSession session)
        {  
            Session = session;
            Get = new InputStringGetter();
            CurrentWordMatches = new List<MatchPair>();
        }

        public void AskForInput()
        {
            string input;
            bool repeat = false;
            
            while (true)
            {
                repeat = false;
                input= Get.GetInputString().ToUpper();
                
                if (!IsValidWord(input, CurrentWord.Length))
                    {                        
                    continue;
                    }


                if (!Session.Dict.Words.Contains(input, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Word not found in currently used Dictionary");
                    continue;
                }
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
                
                break;
            }
            CurrentTry = input;


        }

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


        public void PickWordFromValidSelection()
        {
            Random r = new Random();
            CurrentWord = Session.Dict.Words[r.Next(0, Session.Dict.Words.Count)];
            Lettercount = new Dictionary<char, int>();

            //initialize lettercount Dictionary for other methods
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

        public bool MatchesWord()
        {
            if (CurrentWord == CurrentTry)
            { return true; }
            else 
            { return false; }
        }

        public void CompareTryToWord()
        {
            
            Dictionary<char,int> RepeatMatchdetector = new Dictionary<char,int>();
            WordleMatch[] matches = new WordleMatch[CurrentWord.Length];
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
            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (matches[i] != (WordleMatch)3 && matches[i] != (WordleMatch)2)
                    {

                    matches[i] = (WordleMatch)1;

                    }
            }
            MatchPair match = new MatchPair(CurrentTry, matches);
            CurrentWordMatches.Add(match); 
        }


    }
}
