using DonkeyKong.Managers;
using DonkeyKong.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.Sprites
{
    class MovingAnimatedSprite : AnimatedSprite
    {
        public Input Input;

        public MovingAnimatedSprite(Game game, Dictionary<string, Animation> animations):base(game, animations)
        {
            
        }

        public MovingAnimatedSprite(Game game, Texture2D texture):base(game, texture)
        {
        }

        protected virtual void SetAnimationsAndSounds()
        {
            _animationManager.Play(_animations["Animation"]);
        }



        public virtual void Update(GameTime gameTime)
        {
            SetAnimationsAndSounds();

            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
            
        }
    }
}
