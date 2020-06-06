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
    class AnimatedSprite : GenericSprite
    {
        #region Fields

        public AnimationManager _animationManager;

        public Dictionary<string, Animation> _animations;

        


        float timer = 3;         //Initialize a 3 second timer
        const float TIMER = 3;

        #endregion

        #region Properties

        

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

        public int height;
        public int width;


        public Vector2 Speed = new Vector2(2f, 2f);

        public Vector2 Velocity;

        #endregion

        #region Methods


        public AnimatedSprite(Game game, Dictionary<string, Animation> animations) : base(game)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            height = _animations.First().Value.FrameHeight;
            width = _animations.First().Value.FrameWidth;
        }

        public AnimatedSprite(Game game, Texture2D texture) : base(game)
        {
            _texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (_texture != null)
                spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("This ain't right..!");
        }

        


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

        #endregion
    }
}
