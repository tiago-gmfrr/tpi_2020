/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.Sprites
{
    /// <summary>
    /// A sprite with generic attributes such as a position, hitbox and a texture, mainly meant to be used for heritage
    /// </summary>
    class GenericSprite
    {
        #region Variables
        protected Game _game;

        public Vector2 _position;

        public Texture2D _texture;
        public Rectangle hitbox;


        #endregion

        #region Constructor + Initialize + LoadContent
        /// <summary>
        /// Constructor, requires the game to be able to load content
        /// Usually not used by anyone but its children
        /// </summary>
        public GenericSprite(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Initializes the object in a certain position
        /// </summary>
        /// <param name="position">Position in the screen</param>
        public virtual void Initialize(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// Loads the file texture into a variable
        /// </summary>
        /// <param name="texture">Name of the file</param>
        public void LoadContent(string texture)
        {
            _texture = _game.Content.Load<Texture2D>(texture);
        }

        #endregion

        #region Update + Draw

        /// <summary>
        /// Updates the position and hitbox of the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public virtual void Update(GameTime gameTime)
        {


            hitbox = new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _texture.Width,
                    _texture.Height);
        }
        /// <summary>
        /// Draws the sprite with a texture an hitbox and keeps its original color
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw the sprite
            spriteBatch.Draw(_texture, hitbox, Color.White);
        }

        #endregion
    }
}
