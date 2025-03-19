using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    public class ProgramPresenter
    {
        public void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to wordle. Please make your selection from the list below.\n");

            Console.WriteLine("(1)\t Start new session");
            Console.WriteLine("(2)\t Load session");
            Console.WriteLine("(3)\t Show highscores");
            Console.WriteLine("(4)\t End program");
        }
        public void ShowSessionMenu(WordleSession session)
        {
            Console.Clear();
            Console.WriteLine(@$"Welcome {session.PlayerName}, please select what you want to do:");

            Console.WriteLine("(1)\t Start a new try");
            Console.WriteLine("(2)\t Save Session");
            Console.WriteLine("(3)\t Show your stats");
            Console.WriteLine("(4)\t End Session");
        }


    }
    

    
}
