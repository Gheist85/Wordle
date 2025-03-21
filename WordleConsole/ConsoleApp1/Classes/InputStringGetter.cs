using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    //Getter for strings, designed with length value. This is probably largely unnecessary as it never gets used with the length, but has practice purposes.
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
                    throw new ArgumentException("Input to long, please try again");
                }
                else if (MaxLength != 0 && input.Length < MaxLength)
                { 
                    throw new ArgumentException("Input too short, please try again");
                }
                else
                {
                    LastString = input;
                    return input.ToUpper();
                }

            }
            else
            { throw new ArgumentException("Input can't be empty!"); }
        }
            




        
    }
}
