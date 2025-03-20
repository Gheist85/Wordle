using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    internal class WordlePresenter
    {
        public void AskForGuess()
        {
            
            Console.WriteLine("\n\tPlease Type your Next Guess");
        }

        public void DisplayGuess(MatchPair matchPair)
        {
            for (int i = 0; i < matchPair.Guess.Length;  i++)
            {
                Console.Write("\t");
                switch (matchPair.wordleMatches[i])
                {
                    case (WordleMatch)1:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(matchPair.Guess[i] + " ");
                        break;
                    case (WordleMatch)2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(matchPair.Guess[i] + " ");
                        break;
                    case (WordleMatch)3:
                    default:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(matchPair.Guess[i] + " ");
                        break;
                }
            }
            Console.ForegroundColor= ConsoleColor.White;
            Console.Write("\n\n");

        }

        public void DisplayAllGuesses(WordleLogic logic)
        {
            if (logic.CurrentWordMatches != null)
            {
                Console.WriteLine("\tYour tries so far:\n");
                for (int i = 0; i < logic.CurrentWordMatches.Count; i++)
                DisplayGuess(logic.CurrentWordMatches[i]);
            }
        }

        public void ShowSuccess(WordleLogic logic)
        {
            Console.Clear();            
            DisplayAllGuesses(logic);
            Console.WriteLine("\t Congratulations! You guessed correctly");
        }

        public void ShowFailure(WordleLogic logic)
        {
            Console.Clear();
            Console.WriteLine("\tClose run thing! Maybe next time!\n");
            Console.WriteLine("\tThe correct guess would have been:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\t");
            foreach (char c in logic.CurrentWord)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine();
            Console.ForegroundColor= ConsoleColor.White;
                          


            DisplayAllGuesses(logic);
        }

        public void AnyKey()
        {
            Console.WriteLine("Please press any Key to Continue.");
            Console.ReadKey();
        }

        public void Statistics(WordleSession session)
        {
            Console.Clear();
            Console.WriteLine("Your statistics:\n");
            int total = session.Solved.Words.Count + session.Fails.Words.Count;
            Console.WriteLine($"\tTotal tries: {total}\n");
            Console.WriteLine($"\tSuccessful tries: {session.Solved.Words.Count}\n");
            Console.WriteLine($"\tSuccessful tries: {session.Fails.Words.Count}\n");
            double successrate = (double)session.Solved.Words.Count / (double)total * 100;
            Console.WriteLine($"\tSuccessrate: {successrate}%");

            Console.WriteLine("\n\npress any key to go back to main menu");
            Console.ReadKey(intercept: true);
            Console.Clear();
        }
    }
}
