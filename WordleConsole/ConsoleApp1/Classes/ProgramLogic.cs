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
        public ProgramPresenter pres = new ProgramPresenter();

        public void StartMenuLogic()
        {
            pres.ShowMainMenu();
            int select = 0;
            InputIntKeyGetter keyget = new InputIntKeyGetter();
            select = keyget.GetIntFromKey();
            switch (select)
            {
                case 1:
                    SessionLogic(StartNewSession());
                    break;
                case 2:
                    SessionLogic(LoadSession());
                    break;
                //case 3:
                //    ShowHighscore();
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        public void SessionLogic(WordleSession session)
        {
            Console.Clear();
            pres.ShowSessionMenu(session);
            int select = 0;
            InputIntKeyGetter keyget = new InputIntKeyGetter();
            select = keyget.GetIntFromKey();
            switch (select)
            {
                case 1:
                    session.StartTry();
                    SessionLogic(session);
                    break;

                case 2:
                    session.SaveGame();
                    SessionLogic(session);
                    break;
                case 3:
                    session.ShowStats();
                    SessionLogic(session);
                    break;
                case 4:
                    StartMenuLogic();
                    break;
                default:
                    StartMenuLogic();
                    break;
            }
        }


        public WordleSession StartNewSession()
        {
            Console.Clear();
            Console.WriteLine("Please type your name");
            WordleSession session = new WordleSession(Console.ReadLine());
            return session;
        }
        
        public WordleSession LoadSession()
        {
            
            WordleSession session = null;
                
                Console.WriteLine("Please type the Absolute Path of your savegame and confirm with enter:\n(For example c:\\wordle\\save\\mysave.sav");
                string path = Console.ReadLine();
                try
                {
                     session = JsonSerializer.Deserialize<WordleSession>(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            return session;
        }
                

             
            
        
        
    }
}
