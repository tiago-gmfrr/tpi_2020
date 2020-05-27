using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.Controls
{
    class MenuButton : GenericSprite
    {
        bool collisionWithMario;
        public MenuButton(Game game) : base(game)
        {
            collisionWithMario = false;
        }


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

        public void Update(GameTime gameTime, Rectangle collider)
        {
            //Draw the sprite
            base.Update(gameTime);

            

            if (collider.Intersects(hitbox))
            {
                collisionWithMario = true;
            }
            else
            {
                collisionWithMario = false;
            }

            
        }

        public bool StartGame()
        {
            bool start = false;
            KeyboardState keyboardState = Keyboard.GetState();

            if (collisionWithMario && keyboardState.IsKeyDown(Keys.V))
            {
                start = true;
            }

            return start;
        }
    }
}
