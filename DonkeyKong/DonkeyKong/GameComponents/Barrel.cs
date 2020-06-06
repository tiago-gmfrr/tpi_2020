using DonkeyKong.Models;
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
    class Barrel : MovingAnimatedSprite
    {

        int _height;
        int _width;

        GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Barrel constructor, creates a barrel object that moves independantly
        /// </summary>
        /// <param name="game">The game variable</param>
        /// <param name="animations">All barrel animations</param>
        /// <param name="graphicsDevice">Information about the screen used to display the game</param>
        public Barrel(Game game, Dictionary<string, Animation> animations, GraphicsDevice graphicsDevice) : base(game, animations)
        {

            //For the initial barrel position, in the beginning the hitbox will not be set, so we use these
            _height = _animations.First().Value.FrameHeight;
            _width = _animations.First().Value.FrameWidth;

            _graphicsDevice = graphicsDevice;

            Speed.X = 3f;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="groundLayout"></param>
        public void Update(GameTime gameTime, Dictionary<string, List<Brick>> groundLayout)
        {
            base.Update(gameTime);
            hitbox = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
            Move();
            CollisionBricks(groundLayout);
        }

        /// <summary>
        /// A barrel starts the game going right, then everytime it hits a wall the speed is inversed,
        /// </summary>
        private void Move()
        {
            if (hitbox.Right >= _graphicsDevice.Viewport.Width ||
                hitbox.Left <= 0)
            {
                Speed.X = -Speed.X;
            }
            Velocity.X = Speed.X;
            Velocity.Y = Speed.Y;

        }

        /// <summary>
        /// Stops the barrel from falling down if there is a brick
        /// </summary>
        /// <param name="groundLayout">All bricks in the game</param>
        private void CollisionBricks(Dictionary<string, List<Brick>> groundLayout)
        {
            foreach (List<Brick> lb in groundLayout.Values)
            {
                foreach (Brick b in lb)
                {
                    if (IsTouchingTop(b))
                    {
                        Velocity.Y = 0;
                    }
                }
            }

        }

        /// <summary>
        /// Checks if the barrel has reached the oil barrel.
        /// </summary>
        /// <param name="oilBarrel"></param>
        /// <returns></returns>
        public bool IsCollidingWithOilBarrel(AnimatedSprite oilBarrel)
        {
            return this.hitbox.Intersects(oilBarrel.hitbox);
        }

        /// <summary>
        /// Checks if the barrel is colliding with only the top part of a sprite
        /// </summary>
        /// <param name="sprite">A sprite object with an hitbox</param>
        protected bool IsTouchingTop(GenericSprite sprite)
        {
            return this.hitbox.Bottom + this.Velocity.Y > sprite.hitbox.Top &&
              this.hitbox.Top < sprite.hitbox.Top &&
              this.hitbox.Right > sprite.hitbox.Left &&
              this.hitbox.Left < sprite.hitbox.Right;
        }

    }
}
