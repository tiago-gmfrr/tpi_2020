/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using DonkeyKong.Managers;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DonkeyKong.Controls
{

    /// <summary>
    /// A sprite that changes color when colliding with an object
    /// </summary>
    class MenuButton : GenericSprite
    {
        bool collisionWithMario;

        public Input Input;

        public MenuButton(Game game) : base(game)
        {
            collisionWithMario = false;
        }


        /// <summary>
        /// Runs the button logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="collider">An object that can collide with the button</param>
        public void Update(GameTime gameTime, Rectangle collider)
        {

            base.Update(gameTime);
            CollisionWithCollider(collider);

        }

        /// <summary>
        /// Checks if the collider (in this case Mario) is colliding with the button.
        /// </summary>
        /// <param name="collider">An object that can collide with the button</param>
        private void CollisionWithCollider(Rectangle collider)
        {
            if (collider.Intersects(hitbox))
            {
                collisionWithMario = true;
            }
            else
            {
                collisionWithMario = false;
            }
        }

        /// <summary>
        /// Returns if any of the action inputs were pressed while mario is on top of the button.
        /// </summary>
        public bool ButtonPressed()
        {
            bool pressed = false;
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (Keys k in Input.Action)
            {
                if (collisionWithMario && keyboardState.IsKeyDown(k))
                {
                    pressed = true;
                }
            }
            return pressed;
        }

        /// <summary>
        /// Draws the button with the color white or if its colliding with something draw it with the color grey to represent it.
        /// </summary>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw the sprite
            if (collisionWithMario)
            {
                spriteBatch.Draw(_texture, hitbox, Color.Gray);
            }
            else
            {
                spriteBatch.Draw(_texture, hitbox, Color.White);
            }

        }


    }
}
