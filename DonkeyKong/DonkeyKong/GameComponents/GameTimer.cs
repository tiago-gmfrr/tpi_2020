using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
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

        public GameTimer(Game game, float startTime): base(game)
        {
            time = startTime;
            Started = false;
            Paused = false;
            Finished = false;
        }

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
