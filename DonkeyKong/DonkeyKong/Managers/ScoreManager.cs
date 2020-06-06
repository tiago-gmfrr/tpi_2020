/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using DonkeyKong.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DonkeyKong.Managers
{
    /// <summary>
    /// Manages all the saving and loading highscores process
    /// Inspiration : https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial015
    /// </summary>
    public class ScoreManager
    {
        // Since we don't give a path, this'll be saved in the "bin" folder
        private static string _fileName = "scores.xml"; 

        public List<Score> Highscores { get; private set; }

        public List<Score> Scores { get; private set; }

        public ScoreManager()
          : this(new List<Score>())
        {

        }

        public ScoreManager(List<Score> scores)
        {
            Scores = scores;

            UpdateHighscores();
        }

        /// <summary>
        /// Adds a new score to the list and updates the highscore
        /// </summary>
        /// <param name="score">Player's points</param>
        public void Add(Score score)
        {
            Scores.Add(score);

            // Orders the list so that the lower scores are first ("lower" in terms of number, the lower the score the better)
            Scores = Scores.OrderBy(c => c.Value).ToList(); 

            UpdateHighscores();
        }

        /// <summary>
        /// Loads the highscore from the file
        /// </summary>
        /// <returns></returns>
        public static ScoreManager Load()
        {
            // If there isn't a file to load - create a new instance of "ScoreManager"
            if (!File.Exists(_fileName))
                return new ScoreManager();

            // Otherwise we load the file
            using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));

                var scores = (List<Score>)serializer.Deserialize(reader);

                return new ScoreManager(scores);
            }
        }

        /// <summary>
        /// Since the list is ordered, gets the first member of the list which is the best score
        /// </summary>
        public void UpdateHighscores()
        {
            Highscores = Scores.Take(1).ToList(); // Takes the first 1 elements
        }

        /// <summary>
        /// Writes the xml file with the highscore
        /// </summary>
        /// <param name="scoreManager"></param>
        public static void Save(ScoreManager scoreManager)
        {
            // Overrides the file if it alreadt exists
            using (var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));

                serializer.Serialize(writer, scoreManager.Highscores);
            }
        }
    }
}
