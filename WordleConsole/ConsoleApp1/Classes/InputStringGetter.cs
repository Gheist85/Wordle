using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    internal class InputStringGetter
    {
        public string LastString { get; set; }
        public int MaxLength { get; set; }

        public InputStringGetter()
        { 
             MaxLength = 0;
        }

        public InputStringGetter(int maxlength)
        {
            MaxLength = maxlength;
        }

        public string GetInputString()
        {
            string input = Console.ReadLine();
            if (input != null)
            {
                if (MaxLength != 0 && input.Length > MaxLength)
                {
                    throw new ArgumentException("Eingabe ist zu lang, bitte erneut versuchen");
                }
                else
                {
                    LastString = input;
                    return input;
                }

            }
            else
            { throw new ArgumentException("Eingabe darf nicht leer sein"); }
        }
            




        
    }
}
