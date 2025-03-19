using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    internal class InputIntKeyGetter
    {
        public int GetIntFromKey()
        {
            int input = 0;
            bool check = false;
            while (!check)
            {
                char keyChar = Console.ReadKey(intercept: true).KeyChar;
                check = int.TryParse(keyChar.ToString(), out input);
            }
            return input;
        }
    }
}
