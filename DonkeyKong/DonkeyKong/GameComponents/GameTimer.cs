using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
    /// <summary>
    /// Inspiration pour la classe https://www.youtube.com/watch?v=-2FeSrYT1KE
    /// </summary>
    class GameTimer : GameComponent
    {

        private string text;
        private float time;
        private bool started;
        private bool paused;
        private bool finished;

        public string Text { get => text; set => text = value; }
        public bool Started { get => started; set => started = value; }
        public bool Paused { get => paused; set => paused = value; }
        public bool Finished { get => finished; set => finished = value; }

        /// <summary>
        /// GameTimer constructor, creates a new game timer which starts unset and needs to be started
        /// </summary>
        /// <param name="game">The game variable</param>
        /// <param name="startTime">When does the timer start</param>
        public GameTimer(Game game, float startTime): base(game)
        {
            time = startTime;
            Started = false;
            Paused = false;
            Finished = false;
        }

        /// <summary>
        /// Runs the timer logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (started && !paused)
            {
                time += elapsed;
            }

            Text = ((int)time).ToString();


            base.Update(gameTime);
        }


    }
}
