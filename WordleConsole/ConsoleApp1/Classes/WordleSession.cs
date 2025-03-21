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

// Session Class, represents a game Session and contains all data about the current Player. This is also what gets saved and loaded
namespace Wordle.Classes
{
    // Gave JSON names  to everything to make life easier
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


        //empty Constructor for deserialization

        public WordleSession() { }

        //Constructor for a new Session from the Main Menu
        public WordleSession(string name)
        {
            PlayerName = name;
            Dict = new Wordlist(1);
            Dict.ReadListFromFile();
            Fails = new Wordlist(1);
            Solved = new Wordlist(1);
            Difficulty = 6;
            Score = 0;
        }

        
        //Logic for each try, 2nd main piece of this Program
        public void StartTry()
        {
            //initialize variables
            bool success = false;
            int counter = 0;
            // initialize presenter. only needed in this method, so only initialized here
            WordlePresenter presenter = new WordlePresenter();
            // initialize new WordleLogic (which is where all the comparison happens)
            WordleLogic logic = new WordleLogic(this);
            // pick a new word
            logic.PickWordFromValidSelection();

            // Loop that runs until player either guesses correctly or hits the Difficulty restriction
            while (!logic.MatchesWord() && counter < Difficulty)
            {
                Console.Clear();
                // not on the first try: Show all guesses this try
                if (counter != 0)
                { presenter.DisplayAllGuesses(logic); }
                // in that order: ask for input, process input, compare input to word, raise counter
                presenter.AskForGuess();
                logic.AskForInput();
                logic.CompareTryToWord();
                counter++;
               

            }
            // if guessed correctly
            if (logic.MatchesWord())
            {
                //set bool for scoring, display success message, sort Word from and into dictionaries
                success = true;
                presenter.ShowSuccess(logic);
                Solved.Words.Add(logic.CurrentWord);
                Dict.Words.Remove(logic.CurrentWord);
                presenter.AnyKey();
            }
            //if no success
            else
            {
                // display failure message, sort Word from and into dictionaries
                presenter.ShowFailure(logic);
                Fails.Words.Add(logic.CurrentWord);
                Dict.Words.Remove(logic.CurrentWord);

            }
            // whatever happens, calculate, update and display score
            double tempscore = ScorePoints(logic.CurrentWord, counter, Difficulty, success);
            this.Score += (int)tempscore;
            Console.WriteLine($"\nYou scored {tempscore} points");
            //Update Highscore File
            UpdateHighscore();
            presenter.AnyKey();

        }

        //Save Function
        public void SaveGame()
        {
            // Ask for the Path
            Console.WriteLine("Please type the Path where you want your savefile located:\nFor example: c:\\wordle\\save\\mysave\nMake sure you can write into the directory");
            string path = Console.ReadLine();
            // Get File info and prepare jsonstring
            string directoryPath = Path.GetDirectoryName(path);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonsession = JsonSerializer.Serialize<WordleSession>(this, options);
           
            // Write file if directory exists. Right now, this doesn't check for file permission because i found it too unwieldy too include, is on the future to do though
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
            // if file can't be written, inform user and jump back
            else
            {
                Console.Clear();
                Console.WriteLine("\tSavegame can not be written, because Directory doesn't exists.");
                Console.WriteLine("\n\npress any key to go back to main menu");
                Console.ReadKey(intercept: true);
                Console.Clear();
            }


        }

        // Method for setting difficulty
        public void SetDifficulty()
        {
            InputIntKeyGetter inget = new InputIntKeyGetter();
            int value = 0;
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please Type in how many tries you want to guess a word.\nStandard difficulty is 6, you can choose a value between 4-8\nLOWER VALUE MEANS HIGHER DIFIICULTY!");
                value = inget.GetIntFromKey();
                if (value < 4 || value > 8)
                {
                    Console.WriteLine("Wrong Input, please choose a value between 4 and 8");
                    Console.WriteLine("Press Any Key to Continue");
                    Console.ReadKey(true);
                    continue;
                }
                break;

            }
            this.Difficulty = value;
        }

        //Method for choosing different dictionaries for playing. Right now has only two.
        //Right now Lists have to be textfiles with each word the same length and seperated by newline unless you modify wordlist to be able to cope with something else
        //Must be put in data\\here.txt to work with the current build
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

        // Method for showing current stats using the presenter
        public void ShowStats()
        {
            WordlePresenter presenter = new WordlePresenter();
            presenter.Statistics(this);
        }

        // Scoring Method. This is right now pretty basic and probably needs future work to be more motivating. 
        public double ScorePoints(string currentword, int tries, int difficulty, bool success)
        {
            double pointgain = 0;
            if (!success)
            {
                pointgain = -4;
            }
            else
            {
                pointgain = Math.Ceiling((10 + ((difficulty - tries + 1) * 2)) * (1 + 0.2 * (difficulty - 6)));
            }
            
            return pointgain;
        }

        // this is used to write every result to the Highscorefile
        public void UpdateHighscore()
        {
            //initialise variables
            HighscoreEntry info = new HighscoreEntry { Playername = this.PlayerName, Score = this.Score };
            string path = "data\\highscore";
            HighScore hs = new HighScore { HighscoreList = new List<HighscoreEntry>() };
            bool updated = false;


            // read the HighScore from the actul File
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
                        
            // Check if Playername is in the Highscore List
            for (int i = 0; i < hs.HighscoreList.Count; i++)
            {
                if (hs.HighscoreList[i].Playername == PlayerName)
                {
                    //Check if players Score is higher than his old score and, if yes, substitute with current score
                    if (info.Score > hs.HighscoreList[i].Score)
                    {
                        hs.HighscoreList[i] = info;
                    }
                    updated = true;
                    break;
                }
            }
            // if nothing of the above fits just add the player to the playerbase
            if (!updated)
            {
                hs.HighscoreList.Add(info);
            }

            // Sort the Highscore list highest Score to lowest Score so the presenter doesn't have to do actual work
            hs.HighscoreList.Sort((x, y) => y.Score.CompareTo(x.Score));


            // Ram it back into the file
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
