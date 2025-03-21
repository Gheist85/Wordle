using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using System.Text.Json.Nodes;
using System.Text.Json;

namespace Wordle.Classes
{
    internal class ProgramLogic
    {
        // initialising ProgramPresenter once for all Methods
        public ProgramPresenter pres = new ProgramPresenter();

        //Logic for Console Menu, utilising the InputIntKeyGetter
        public void StartMenuLogic()
        {           
            int select = 0;
            InputIntKeyGetter keyget = new InputIntKeyGetter();
            while (select != 4)
            {
                pres.ShowMainMenu(); // ConsoleOutput
                select = keyget.GetIntFromKey();
                switch (select)
                {
                    case 1:
                        SessionLogic(StartNewSession()); //directly jump into the Session Menu with a new Session
                        break;
                    case 2:
                        SessionLogic(LoadSession()); // load save File, then jump into the session Menu
                        break;
                    case 3:
                        ShowHighscore();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        //Start Session Function with name Input
        public WordleSession StartNewSession()
        {
            Console.Clear();
            Console.WriteLine("Please type your name");
            WordleSession session = new WordleSession(Console.ReadLine());
            return session;
        }

        //Load function utilizing JsonSerializer
        public WordleSession LoadSession()
        {

            WordleSession session = null;

            Console.WriteLine("Please type the Absolute Path of your savegame and confirm with enter:\n(For example c:\\wordle\\save\\mysave.sav");
            string path = Console.ReadLine();
            try
            {
                StreamReader sr = new StreamReader(path);
                string jsonstring = sr.ReadToEnd();
                session = JsonSerializer.Deserialize<WordleSession>(jsonstring);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return session;
        }

        // HighScore display using Json serializer to load HighScore from file
        public void ShowHighscore()
        {
            Console.Clear();
            HighScore hs = new HighScore { HighscoreList = new List<HighscoreEntry>() };
            string path = "data\\highscore";
            string json = File.ReadAllText(path);
            try
            {
                hs = JsonSerializer.Deserialize<HighScore>(json);
            }
            catch (Exception ex)
            { }
            Console.WriteLine(hs.ToString());
            Console.WriteLine("\n\npress any key to go back to main menu");
            Console.ReadKey(intercept: true);
            Console.Clear();
        }



        // Logic for the session Menu, same structure as before
        public void SessionLogic(WordleSession session)
        {
            int select = 0;
            InputIntKeyGetter keyget = new InputIntKeyGetter();
            do
            {
                Console.Clear();
                pres.ShowSessionMenu(session); // ConsoleOutput
                select = keyget.GetIntFromKey();
                switch (select)
                {
                    case 1:
                        session.StartTry();                        
                        break;
                    case 2:
                        session.SaveGame();                        
                        break;
                    case 3:
                        session.ShowStats();                        
                        break;
                    case 4:
                        pres.ShowRules();
                        break;
                       case 5:
                        session.SetDifficulty();
                        break;
                    case 6:
                        session.ChooseDict();
                        break;
                    case 7:
                        StartMenuLogic();
                        return;
                    default:
                        StartMenuLogic();
                        break;
                }
            } while(select != 7);
        }    
                   
    }
}
