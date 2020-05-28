using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
    class Mario : MovingAnimatedSprite
    {

        private Rectangle _hitbox;

        public Rectangle Hitbox { get => _hitbox; set => _hitbox = value; }

        private GraphicsDevice _graphicsDevice;

        private int _width;
        private int _height;

        private int _currentFloor = 0;

        bool _inGame;
        bool _onGround;
        bool _jumping;
        Color[] personTextureData;
        Color[] brickTextureData;

        public Mario(Game game, Dictionary<string, Animation> animations, GraphicsDevice graphicsDevice, bool inGame = false) : base(game, animations)
        {
            _inGame = inGame;
            _graphicsDevice = graphicsDevice;
            _height = _animations.First().Value.FrameHeight;
            _width = _animations.First().Value.FrameWidth;

            personTextureData = new Color[_animations.First().Value.Texture.Width * _animations.First().Value.Texture.Height];
            _animations.First().Value.Texture.GetData(personTextureData);
        }


        protected override void SetAnimations()
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["WalkRight"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["WalkLeft"]);
            /*else if (Velocity.Y > 0)
                _animationManager.Play(_animations["WalkDown"]);
            else if (Velocity.Y < 0)
                _animationManager.Play(_animations["WalkUp"]);*/
            else _animationManager.Stop();
        }

        public void Move()
        {
            if (!_inGame)
            {
                if (Keyboard.GetState().IsKeyDown(Input.Up))
                    Velocity.Y = -Speed;
                if (Keyboard.GetState().IsKeyDown(Input.Down))
                    Velocity.Y = Speed;
            }
            else
            {
                Jump();
            }


            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;


        }

        public override void Update(GameTime gameTime)
        {
            Hitbox = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
            Move();

            ScreenCollisions();
            base.Update(gameTime);
        }

        public void Update(GameTime gameTime, Dictionary<string, List<Brick>> groundLayout)
        {
            Hitbox = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
            Move();
            ApplyPhysics(gameTime);
            

            foreach (Brick b in groundLayout["Level" + _currentFloor])
            {
                if (this.IsTouchingTop(b))
                {
                    _onGround = true;
                }
            }
            


            ScreenCollisions();
            base.Update(gameTime);


        }

        private void Jump()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _jumping = true;
        }

        private void ScreenCollisions()
        {
            if (Hitbox.Right >= _graphicsDevice.Viewport.Width)
                _position.X = _graphicsDevice.Viewport.Width - _width;
            if (Hitbox.Left <= 0)
                _position.X = 0;
            if (Hitbox.Top <= 0)
                _position.Y = 0;
            if (Hitbox.Bottom >= _graphicsDevice.Viewport.Height)
                _position.Y = _graphicsDevice.Viewport.Height - _height;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
        #region Collision
        protected bool IsTouchingLeft(GenericSprite sprite)
        {
            return this.Hitbox.Right + this.Velocity.X > sprite.hitbox.Left &&
              this.Hitbox.Left < sprite.hitbox.Left &&
              this.Hitbox.Bottom > sprite.hitbox.Top &&
              this.Hitbox.Top < sprite.hitbox.Bottom;
        }

        protected bool IsTouchingRight(GenericSprite sprite)
        {
            return this.Hitbox.Left + this.Velocity.X < sprite.hitbox.Right &&
              this.Hitbox.Right > sprite.hitbox.Right &&
              this.Hitbox.Bottom > sprite.hitbox.Top &&
              this.Hitbox.Top < sprite.hitbox.Bottom;
        }

        protected bool IsTouchingTop(GenericSprite sprite)
        {
            return this.Hitbox.Bottom + this.Velocity.Y > sprite.hitbox.Top &&
              this.Hitbox.Top < sprite.hitbox.Top &&
              this.Hitbox.Right > sprite.hitbox.Left &&
              this.Hitbox.Left < sprite.hitbox.Right;
        }

        protected bool IsTouchingBottom(GenericSprite sprite)
        {
            return this.Hitbox.Top + this.Velocity.Y < sprite.hitbox.Bottom &&
              this.Hitbox.Bottom > sprite.hitbox.Bottom &&
              this.Hitbox.Right > sprite.hitbox.Left &&
              this.Hitbox.Left < sprite.hitbox.Right;
        }

        #endregion

        public void ApplyPhysics(GameTime gameTime)
        {
            if (!_onGround)
                Velocity.Y += 1f;

            if (_onGround && _jumping)
            {
                Velocity.Y = -35f;
            }

            _onGround = false;
            _jumping = false;

            Position += Velocity;
        }

    }
}
