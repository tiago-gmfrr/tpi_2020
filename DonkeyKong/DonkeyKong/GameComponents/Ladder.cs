/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DonkeyKong.GameComponents
{
    /// <summary>
    /// A ladder which will be used to trigger mario's climbing movement
    /// </summary>
    class Ladder : GenericSprite
    {
        int _nbSpritesInStack;

        public int NbSpritesInStack { get => _nbSpritesInStack; set => _nbSpritesInStack = value; }


        /// <param name="game">The game variable</param>
        public Ladder(Game game):base(game)
        {

        }

        /// <summary>
        /// Runs the ladder logic, where the collisions lies etc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            int collisionHeight = _texture.Height * NbSpritesInStack;
            
            hitbox = new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _texture.Width,
                    collisionHeight);
        }

        /// <summary>
        /// Draws the entire ladder, which is composed of mini ladder parts
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NbSpritesInStack; i++)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y + _texture.Height * i, _texture.Width, _texture.Height), Color.White);
            }
            
        }

    }
}
