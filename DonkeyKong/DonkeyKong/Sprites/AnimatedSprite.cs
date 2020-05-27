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
    class AnimatedSprite : GenericSprite
    {
        #region Fields

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;


        float timer = 3;         //Initialize a 3 second timer
        const float TIMER = 3;

        #endregion

        #region Properties

        public Input Input;

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
        

        public float Speed = 2f;

        public Vector2 Velocity;

        #endregion

        #region Methods


        public AnimatedSprite(Game game, Dictionary<string, Animation> animations) : base(game)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
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

        protected virtual void SetAnimations()
        {
            
            _animationManager.Play(_animations["Animated"]);
        }


        public virtual void Update(GameTime gameTime, List<AnimatedSprite> sprites)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                //Timer expired, execute action
                timer = TIMER;   //Reset Timer

                //SetAnimations();
            }

            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        #endregion
    }
}
