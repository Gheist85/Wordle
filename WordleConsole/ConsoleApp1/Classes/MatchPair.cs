using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    //Match Pair class. Basically used like a two dimensional array during the rest of the code. To connect words to solution patterns and save them
    public  class MatchPair
    {
        public string Guess;
        public WordleMatch[] wordleMatches;

        public MatchPair(string guess, WordleMatch[] matches)
        {
            this.Guess = guess;
            this.wordleMatches = matches;
        }
    }
}
