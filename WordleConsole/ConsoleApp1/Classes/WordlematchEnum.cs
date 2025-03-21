using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// enum to have an easy handle on letter-match-classification 
namespace Wordle.Classes
{
    public enum WordleMatch
    {
        NotPartOfWord,
        PartOfWordButWrongIndex,
        PArtOfWOrdButCorrectIndex
    }
    
}
