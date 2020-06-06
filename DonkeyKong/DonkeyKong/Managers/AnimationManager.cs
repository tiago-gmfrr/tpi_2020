/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/

using DonkeyKong.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DonkeyKong.Managers
{
    /// <summary>
    /// This class contains all the logic for the animations and also displays it
    /// Inspiration : https://github.com/Oyyou/MonoGame_Tutorials
    /// </summary>
    class AnimationManager
    {
        public Animation _animation;

        private float _timer;

        public Vector2 Position { get; set; }

        /// <summary>
        /// Starts the animation manager with one animation to manage
        /// </summary>
        /// <param name="animation">A texture with multiple frames to be cycled through</param>
        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        /// <summary>
        /// Runs all the animation logic, such as how long each frame appears and the cycling of frames
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }


        /// <summary>
        /// If the animations isnt already playing, play it and at the end stop doing it, so it doesnt do it forever.
        /// </summary>
        /// <param name="animation">A texture with multiple frames to be cycled through</param>
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            Stop();
        }

        /// <summary>
        /// Stop the animation
        /// </summary>
        public void Stop()
        {
            _timer = 0f;

            _animation.CurrentFrame = 1;
        }

        /// <summary>
        /// Draws the current frame
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture,
                             Position,
                             new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                           0,
                                           _animation.FrameWidth,
                                           _animation.FrameHeight),
                             Color.White);
        }
    }
}
