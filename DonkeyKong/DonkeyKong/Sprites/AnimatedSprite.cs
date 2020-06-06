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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.Sprites
{

    /// <summary>
    /// A sprite with animations that cannot be moved
    /// Inspiration : https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial011
    /// </summary>
    class AnimatedSprite : GenericSprite
    {
        #region Fields

        public AnimationManager _animationManager;

        public Dictionary<string, Animation> _animations;

       

        #endregion

        #region Properties

        
        /// <summary>
        /// When setting the position property, also set the animation position
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public int _height;
        public int _width;


        public Vector2 Speed = new Vector2(2f, 2f);

        public Vector2 Velocity;

        #endregion

        #region Methods


        public AnimatedSprite(Game game, Dictionary<string, Animation> animations) : base(game)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);

            //For the initial animatedSprite position, in the beginning the hitbox will not be set, so we use these
            _height = _animations.First().Value.FrameHeight;
            _width = _animations.First().Value.FrameWidth;
        }

        public AnimatedSprite(Game game, Texture2D texture) : base(game)
        {
            _texture = texture;
        }


        /// <summary>
        /// Runs the position and hitbox logic, also updates the animation manager
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {

            _animationManager.Update(gameTime);
            hitbox = new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _animations.First().Value.FrameWidth,
                    _animations.First().Value.FrameHeight);

            

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Draws the animated sprite
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            if (_texture != null)
                spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            
        }
        #endregion
    }
}
