using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
    class Ladder : GenericSprite
    {
        int _nbSpritesInStack;

        public int NbSpritesInStack { get => _nbSpritesInStack; set => _nbSpritesInStack = value; }

        public Ladder(Game game):base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            int collisionHeight = _texture.Height * NbSpritesInStack;
            
            hitbox = new Rectangle(
                    (int)_position.X,
                    (int)_position.Y /*- collisionHeight + _texture.Height*/,
                    _texture.Width,
                    collisionHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NbSpritesInStack; i++)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y + _texture.Height * i, _texture.Width, _texture.Height), Color.White);
            }
            
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
