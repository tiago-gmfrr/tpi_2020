/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using DonkeyKong.Managers;
using DonkeyKong.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DonkeyKong.Sprites
{
    /// <summary>
    /// A sprite with animations that can be moved
    /// Inspiration : https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial011
    /// </summary>
    class MovingAnimatedSprite : AnimatedSprite
    {
        public Input Input;

        public MovingAnimatedSprite(Game game, Dictionary<string, Animation> animations):base(game, animations)
        {
            
        }

        public MovingAnimatedSprite(Game game, Texture2D texture):base(game, texture)
        {

        }

        /// <summary>
        /// Class to be overriden, sets the current animation and sound
        /// </summary>
        protected virtual void SetAnimationsAndSounds(){}


        /// <summary>
        /// Basic logic for a moving animated sprite, update its animations, change them when necessary and update its position
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public virtual void Update(GameTime gameTime)
        {
            SetAnimationsAndSounds();

            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
            
        }
    }
}
