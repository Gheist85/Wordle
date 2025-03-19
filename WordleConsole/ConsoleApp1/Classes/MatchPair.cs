using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
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
