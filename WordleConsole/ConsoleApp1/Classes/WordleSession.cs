using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;


namespace Wordle.Classes
{

    public class WordleSession
    {
        [JsonPropertyName("PlayerName")]
        public string PlayerName { get; set; }
        [JsonPropertyName("Dict")]

        public Wordlist Dict { get; set; }
        [JsonPropertyName("Fails")]
        public Wordlist Fails { get; set; }
        [JsonPropertyName("Solved")]
        public Wordlist Solved { get; set; }
        [JsonPropertyName("Difficulty")]
        public int Difficulty { get; set; }
        [JsonPropertyName("Score")]
        public int Score { get; set; }


        //Constructor for deserialization

        public WordleSession() { }
        public WordleSession(string name)
        {
            PlayerName = name;
            Dict = new Wordlist(1);
            Dict.ReadListFromFile();
            Fails = new Wordlist(1);
            Solved = new Wordlist(1);
            Difficulty = 5;
            Score = 0;
        }
        public void SetDifficulty()

        {
            InputIntKeyGetter inget = new InputIntKeyGetter();
            int value = 0;
            Console.WriteLine("Please Type in how many tries you want to guess a word.\nStandard difficulty is 5, you can choose a value between 3-7");
            while (value < 3 || value > 7)
            {

                value = inget.GetIntFromKey();
                if (value < 3 || value > 7)
                {
                    Console.WriteLine("Wrong Input, please choose a value between 3 and 7");
                }

            }
            this.Difficulty = value;
        }

        public void StartTry()
        {
            bool success = false;
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
                success = true;
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

            }

            Console.WriteLine($"\nYou scored {ScorePoints(logic.CurrentWord, counter, Difficulty, success)} points");
            UpdateHighscore();
            presenter.AnyKey();

        }

        public void SaveGame()
        {
            Console.WriteLine("Please type the Path where you want your savefile located:\nFor example: c:\\wordle\\save\\mysave\nMake sure you can write into the directory");

            string path = Console.ReadLine();
            string directoryPath = Path.GetDirectoryName(path);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonsession = JsonSerializer.Serialize<WordleSession>(this, options);
            if (Directory.Exists(directoryPath))

            {
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
            else
            {
                Console.Clear();
                Console.WriteLine("\tSavegame can not be written, because Directory doesn't exists.");
                Console.WriteLine("\n\npress any key to go back to main menu");
                Console.ReadKey(intercept: true);
                Console.Clear();
            }


        }

        public void ChooseDict()
        {
            WordlePresenter presenter = new WordlePresenter();
            InputIntKeyGetter intKeyGetter = new InputIntKeyGetter();
            presenter.AskForDict();
            int value = intKeyGetter.GetIntFromKey();
            switch (value)
            {
                case 1:
                    this.Dict.Path = "data\\words.txt";
                    this.Fails = new Wordlist(1);
                    this.Solved = new Wordlist(1);
                    this.Dict.ReadListFromFile();
                    break;
                case 2:
                    this.Dict.Path = "data\\words2.txt";
                    this.Fails = new Wordlist(1);
                    this.Solved = new Wordlist(1);
                    this.Dict.ReadListFromFile();
                    break;
                case 3:
                    break;
                default:
                    break;
            }

        }


        public void ShowStats()
        {
            WordlePresenter presenter = new WordlePresenter();
            presenter.Statistics(this);
        }

        public double ScorePoints(string currentword, int tries, int difficulty, bool success)
        {
            double pointgain = 0;
            if (!success)
            {
                pointgain = -5;
            }
            else
            {
                pointgain = Math.Ceiling((10 + (currentword.Length - tries + 1 * 2)) * (1 + 0.2 * (difficulty - 5)));
            }
            Score += (int)pointgain;
            return pointgain;
        }

        public void UpdateHighscore()
        {
            HighscoreEntry info = new HighscoreEntry { Playername = this.PlayerName, Score = this.Score };
            string path = "data\\highscore";


            HighScore hs = new HighScore { HighscoreList = new List<HighscoreEntry>() };

            try
            {

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    hs = JsonSerializer.Deserialize<HighScore>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading highscore file: {ex.Message}");
            }

            bool updated = false;


            for (int i = 0; i < hs.HighscoreList.Count; i++)
            {
                if (hs.HighscoreList[i].Playername == PlayerName)
                {
                    if (info.Score > hs.HighscoreList[i].Score)
                    {
                        hs.HighscoreList[i] = info;
                    }
                    updated = true;
                    break;
                }
            }


            if (!updated)
            {
                hs.HighscoreList.Add(info);
            }


            hs.HighscoreList.Sort((x, y) => y.Score.CompareTo(x.Score));

            try
            {

                string json = JsonSerializer.Serialize(hs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing highscore file: {ex.Message}");
            }
        }
    }

    }
