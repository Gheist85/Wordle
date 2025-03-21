using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    // PresenterClass for Main Menu, Session Menu and Rules. Just Contains Methods generate static Console Output
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
            Console.WriteLine("(4)\t View the rules");
            Console.WriteLine("(5)\t Set the difficulty");
            Console.WriteLine("(6)\t Choose Dictionary");
            Console.WriteLine("(7)\t End Session");
        }

        public void ShowRules()
        {
            Console.Clear();
            Console.WriteLine("\tWordle Rules:");
            Console.WriteLine("\n\tYou have five tries to guess a random five letter word");
            Console.WriteLine("\n\tAfter each try, wordle will inform you about your success in the following manner:");
            Console.WriteLine("\n\t1: a grey letter");
            Console.ForegroundColor = ConsoleColor.DarkGray;Console.WriteLine("\t\tg r e y ");Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" \tindicates that the letter isn't part of the word at all");
            Console.WriteLine("\n\n\t2: a yellow letter");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("\t\ty e l l o w "); Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" \tindicates that the letter is part of the word, but not in the right place a the moment");
            Console.WriteLine("\n\n\t3: a green letter");
            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("\t\tg r e e n"); Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" \tindicates that the letter is part of the word as well as in the right position");
            Console.WriteLine("\n\n\tWordle will show you all tries so far to help you guess the right word. Good luck!!");
            Console.WriteLine("\n\n\nPress any key to return to menu");
            Console.ReadKey(intercept: true);
        }


    }
    

    
}
