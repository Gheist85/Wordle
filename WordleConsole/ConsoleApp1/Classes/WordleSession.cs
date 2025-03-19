using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wordle.Classes
{
    public class WordleSession
    {
        public string PlayerName { get; set; }
        public Wordlist Dict { get; set; }
        public Wordlist Fails { get; set; }
        public Wordlist Solved { get; set; }
        public int Difficulty { get; set; }

        public WordleSession (string name)
        {
            PlayerName = name;
            Dict = new Wordlist ();
            Dict.ReadListFromFile();
            Fails = new Wordlist ();
            Solved = new Wordlist ();
            Difficulty = 5;
        }
        public void SetDifficulty(int difficulty)
        { this.Difficulty = difficulty; }

        public void StartTry()
        {
            int counter = 0;
            WordlePresenter presenter = new WordlePresenter();
            WordleLogic logic = new WordleLogic(this);
            logic.PickWordFromValidSelection();
            while (!logic.MatchesWord() && counter < Difficulty)
            {
                
                presenter.AskForGuess();
                logic.AskForInput();
                logic.CompareTryToWord();
                counter++;
                Console.Clear();
                presenter.DisplayAllGuesses(logic);
                
            }
            if (logic.MatchesWord())
            {
                presenter.ShowSuccess(logic);
                Solved.Words.Add(logic.CurrentWord);
                Dict.Words.Remove(logic.CurrentWord);
                presenter.AnyKey();
            }
            else
            {
                presenter.ShowFailure(logic);
                Fails.Words.Add(logic.CurrentWord);
                Dict.Words.Remove(logic.CurrentWord);
                presenter.AnyKey();
            }
        }
        
        public void SaveGame()
        {
            Console.WriteLine("Please type the Path where you want your savefile located:\nFor example: c:\\wordle\\save\\mysave");

            string path = Console.ReadLine();
            string jsonsession = JsonSerializer.Serialize<WordleSession>(this);

            try
            {
                StreamWriter writer = new StreamWriter(path, false);

                writer.Write(jsonsession);

                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ShowStats()
        {
            WordlePresenter presenter = new WordlePresenter();
            presenter.Statistics(this);
        }

    }
}
