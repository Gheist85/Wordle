using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace Wordle.Classes
{
    public class HighScore
    {
        [JsonPropertyName("HighscoreList")]
        public List<HighscoreEntry> HighscoreList { get; set; }
        public HighScore() { }

        public override string ToString()
        {
            if (HighscoreList == null || HighscoreList.Count == 0)
            {
                return "There is no Highscore yet";
            }

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < Math.Min(HighscoreList.Count, 5); i++)
            {
                output.AppendLine($"\t{i + 1}.\t{HighscoreList[i].Playername}\t{HighscoreList[i].Score} Pts.");
            }

            return output.ToString();
        }

    }

    public class HighscoreEntry
    {
        [JsonPropertyName("PlayerName")]
    public string Playername {  get; set; }
        [JsonPropertyName("Score")]
        public int Score { get; set; }  
        public HighscoreEntry() { }

    }
}
