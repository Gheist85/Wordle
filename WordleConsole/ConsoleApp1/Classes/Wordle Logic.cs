using System;
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
            CurrentTry = Get.GetInputString().ToLower();
        }

        public void PickWordFromValidSelection()
        {
            Random r = new Random();
            CurrentWord = Session.Dict.Words[r.Next(0, Session.Dict.Words.Count)];
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
            WordleMatch[] matches = new WordleMatch[CurrentWord.Length];
            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (CurrentTry[i] != CurrentWord[i] && !CurrentWord.Contains(CurrentTry[i]))
                {
                    matches[i] = (WordleMatch)1;
                }
                else if (CurrentTry[i] != CurrentWord[i] && CurrentWord.Contains(CurrentTry[i]))
                {
                    matches[i] = (WordleMatch)2;
                }
                else if (CurrentTry[i] == CurrentWord[i])
                {
                    matches[i] = (WordleMatch)3;
                }

            }
            MatchPair match = new MatchPair(CurrentTry, matches);
            CurrentWordMatches.Add(match); 
        }


    }
}
