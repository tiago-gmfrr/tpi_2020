/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Unit tests for the Donkey Kong program
***/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DonkeyKong.Models;

namespace DonkeyKong.Managers.Tests
{
    [TestClass()]
    public class ScoreManagerTests
    {
        /// <summary>
        /// Make sure the adding method actually adds a score to the score list
        /// </summary>
        [TestMethod()]
        public void AddTest()
        {
            ScoreManager scoreManager = new ScoreManager();
            Score s = new Score()
            {
                PlayerName = "NONAME",
                Value = "0025",
            };

            scoreManager.Add(s);

            CollectionAssert.Contains(scoreManager.Scores,s);
        }

        /// <summary>
        /// Make sure you can save and load scores
        /// </summary>
        [TestMethod()]
        public void SaveAndLoadTest()
        {
            ScoreManager scoreManager = new ScoreManager();
            Score s = new Score()
            {
                PlayerName = "NONAME",
                Value = "0014",
            };

            scoreManager.Add(s);

            ScoreManager.Save(scoreManager);
            scoreManager = ScoreManager.Load();

            Assert.AreEqual(s.Value, scoreManager.Scores[0].Value);

        }

        /// <summary>
        /// Make sure the highscore is updated
        /// </summary>
        [TestMethod()]
        public void UpdateHighscoresTest()
        {
            ScoreManager scoreManager = new ScoreManager();
            Score s = new Score()
            {
                PlayerName = "NONAME",
                Value = "0025",
            };
            scoreManager.Add(s);

            Score currentHighscore = scoreManager.Highscores[0];

            s = new Score()
            {
                PlayerName = "NONAME",
                Value = "0014",
            };
            scoreManager.Add(s);

            Score newHighscore = scoreManager.Highscores[0];

            //The UpdateHighscores method is called when adding a new score, so no need to call it again

            Assert.AreNotEqual(currentHighscore.Value, newHighscore.Value);
        }


    }
}