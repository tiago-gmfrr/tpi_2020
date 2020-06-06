/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace DonkeyKong.GameComponents
{
    /// <summary>
    /// A Mario which can be moved with the keys given to the Input property.
    /// Mario has different sets of animations and sounds.
    /// He checks for collisions with ladders bricks and barrels and behaves differently when colliding with each of these objects
    /// </summary>
    class Mario : MovingAnimatedSprite
    {
        #region Variables and properties
        private Rectangle _hitbox;
        private GraphicsDevice _graphicsDevice;

        private bool _inGame;
        private bool _isGoingRight;
        private bool _marioCollidedWithBarrel;

        private bool _onGround;
        private bool _hasJumped;
        private bool _onLaddder;

        private Dictionary<string, SoundEffect> _soundEffects;
        private SoundEffectInstance _soundInstanceJump;
        private SoundEffectInstance _soundInstanceWalking;
        private SoundEffectInstance _soundInstanceClimbing;


        public Rectangle Hitbox { get => _hitbox; set => _hitbox = value; }

        #endregion

        
        /// <param name="game">The game variable</param>
        /// <param name="animations">All mario animations</param>
        /// <param name="soundEffects">All mario sound effects</param>
        /// <param name="graphicsDevice">Information about the screen used to display the game</param>
        /// <param name="inGame">Is mario in the game</param>
        public Mario(Game game, Dictionary<string, Animation> animations, Dictionary<string, SoundEffect> soundEffects, GraphicsDevice graphicsDevice, bool inGame = false) : base(game, animations)
        {
            _inGame = inGame;
            _graphicsDevice = graphicsDevice;

            //For the initial mario position, in the beginning the hitbox will not be set, so we use these
            _height = _animations.First().Value.FrameHeight;
            _width = _animations.First().Value.FrameWidth;

            //All mario sound effects
            _soundEffects = soundEffects;
            _soundInstanceWalking = _soundEffects["Walking"].CreateInstance();
            _soundInstanceWalking.Volume = 0.1f;
            _soundInstanceJump = _soundEffects["Jump"].CreateInstance();
            _soundInstanceJump.Volume = 0.5f;
            _soundInstanceClimbing = _soundEffects["Climbing"].CreateInstance();

            //Initializw mario variables at the start of the game
            _isGoingRight = true;
            _hasJumped = true;
            _onGround = true;
            _onLaddder = false;
            _marioCollidedWithBarrel = true;

        }

        #region Updates
        /// <summary>
        /// Runs the Mario logic he is not in the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            Hitbox = new Rectangle((int)_position.X, (int)_position.Y, _animations.First().Value.FrameWidth, _animations.First().Value.FrameHeight);
            Move();
            ScreenCollisions();
            SetAnimationsAndSounds();
            base.Update(gameTime);
        }

        /// <summary>
        /// Runs the game logic for Mario, checks for collisions, gathers inputs, sets the animations and play audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="groundLayout">All bricks in the game, used to know when mario can walk</param>
        /// <param name="allLadders">All ladders in the game, used to know when mario can climb</param>
        /// <param name="allBarrels">All barrels in the game, used to know when mario dies</param>
        public void Update(GameTime gameTime, Dictionary<string, List<Brick>> groundLayout, List<Ladder> allLadders, List<Barrel> allBarrels)
        {
            //Updates Mario's hitbox
            Hitbox = new Rectangle((int)_position.X, (int)_position.Y, _animations.First().Value.FrameWidth, _animations.First().Value.FrameHeight);

            //Updates mario's sounds and animations
            SetAnimationsAndSounds();
            _animationManager.Update(gameTime);

            //Makes sure Mario doesn't go off screen
            ScreenCollisions();

            //Checks Mario's collision with barrels and ladders 
            BarrelCollision(allBarrels);
            LadderCollision(allLadders, groundLayout);

            //If Mario is not on a ladder
            if (!_onLaddder)
            {
                //He is able to move normally
                Move();
                GroundCollision(groundLayout);
                ApplyPhysics(gameTime);
            }
            else
            {
                //He can only move upwards or downwards until he is no longer in a ladder
                LadderMovement();
            }


            //Updates Mario's position with his current velocity
            Position += Velocity;

        }

        #endregion

        #region Methods

        #region Movement
        /// <summary>
        /// Mario's walking and jumping are controlled through here.
        /// </summary>
        private void Move()
        {
            //If he is not in the game. He is in one of the menus 
            if (!_inGame)
            {
                //Allow him to go up and down freely
                if (Keyboard.GetState().IsKeyDown(Input.Up))
                    Velocity.Y = -Speed.Y;
                if (Keyboard.GetState().IsKeyDown(Input.Down))
                    Velocity.Y = Speed.Y;
            }
            else//If he is in the game he will be able to jump
            {
                Jump();
            }

            //Sets the Mario velocity considering which input was given
            if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Velocity.X = -Speed.X;
                _isGoingRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Velocity.X = Speed.X;
                _isGoingRight = true;
            }
            else
                Velocity.X = 0;


        }

        /// <summary>
        /// If Mario is on the ground he will be able to jump
        /// </summary>
        private void Jump()
        {

            if (_onGround)
            {
                foreach (Keys k in Input.Action)
                {
                    if (Keyboard.GetState().IsKeyDown(k) && _hasJumped == false)
                    {
                        _position.Y -= 5f;
                        Velocity.Y = -4f;
                        _hasJumped = true;
                    }
                }
            }


        }

        /// <summary>
        /// If Mario is on a ladder he can move upwards or downwards but cannot move sideways
        /// </summary>
        private void LadderMovement()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed.Y;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed.Y;
            else
                Velocity.Y = 0;

            Velocity.X = 0;
        }

        /// <summary>
        /// Adds gravity to mario
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private void ApplyPhysics(GameTime gameTime)
        {

            if (_hasJumped == true || _onGround == false)
            {
                float i = 1;
                Velocity.Y += 0.15f * i;
            }

            if (_onGround == true)
            {
                Velocity.Y = 0;
                _hasJumped = false;
            }

        }

        #endregion

        #region All collisions

        #region Game objects collisions
        /// <summary>
        /// Checks if Mario's collision with the ground, and since the ground is tilted moves mario upwards ito compensate it
        /// </summary>
        /// <param name="groundLayout"></param>
        private void GroundCollision(Dictionary<string, List<Brick>> groundLayout)
        {
            _onGround = false;

            foreach (List<Brick> lb in groundLayout.Values)
            {

                for (int i = 0; i < lb.Count; i++)
                {
                    //If standing on a brick
                    if (this.IsTouchingTop(lb[i]) && !IsTouchingBottom(lb[i]))
                    {
                        
                        if (i > 0 && _hasJumped == false)
                        {
                            //If going left or right and colliding with one of the bricks sides
                            if (this.IsTouchingRight(lb[i - 1]) ||
                            this.IsTouchingLeft(lb[i - 1]))
                            {
                                _position.Y = lb[i - 1]._position.Y - _height;
                            }
                        }

                        //Set the mario game variables
                        _hasJumped = false;
                        _onGround = true;
                        _onLaddder = false;

                    }
                }

            }
        }

        /// <summary>
        /// Checks if Mario's hitbox collided with any of the barrels hitboxes
        /// </summary>
        /// <param name="allBarrels">All barrels in the game</param>
        private void BarrelCollision(List<Barrel> allBarrels)
        {
            _marioCollidedWithBarrel = false;
            foreach (Barrel b in allBarrels)
            {
                if (Hitbox.Intersects(b.hitbox))
                {
                    _marioCollidedWithBarrel = true;

                    StopAllSoundInstances();
                }

            }
        }

        /// <summary>
        /// Checks if Mario's hitbox collided with any of the ladders hitboxes
        /// </summary>
        /// <param name="allLadders">All ladders in the game</param>
        /// <param name="groundLayout">All bricks in the game</param>
        private void LadderCollision(List<Ladder> allLadders, Dictionary<string, List<Brick>> groundLayout)
        {
            _onLaddder = false;

            foreach (Ladder l in allLadders)
            {
                if (Hitbox.Intersects(l.hitbox))
                {
                    //Mario has to be in the center of the ladder to be able to use it
                    if (((IsTouchingRight(l) && Position.X + _width / 2 <= l._position.X + l._texture.Width / 2)
                        || (IsTouchingLeft(l) && Position.X + _width / 2 >= l._position.X + l._texture.Width / 2))
                        && (Keyboard.GetState().IsKeyDown(Input.Up) || Keyboard.GetState().IsKeyDown(Input.Down)))
                    {
                        _onLaddder = true;

                        //If he is going downwards make sure he isnt in the ground
                        if (Keyboard.GetState().IsKeyDown(Input.Down))
                        {
                            GroundCollision(groundLayout);
                        }
                    }

                }
            }
        }


        #endregion

        /// <summary>
        /// Stop Mario from going out of the screen
        /// </summary>
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

        #region Collision checks

        /// <summary>
        /// Checks if Mario is going to collide with the left of a sprite and he is not past the sprite's left 
        /// </summary>
        /// <param name="sprite">A sprite object with an hitbox</param>
        private bool IsTouchingLeft(GenericSprite sprite)
        {
            return this.Hitbox.Right + this.Velocity.X > sprite.hitbox.Left &&
              this.Hitbox.Left < sprite.hitbox.Left;
        }

        /// <summary>
        /// Checks if Mario is going to collide with the right of an sprite and he is not past the sprite's right 
        /// </summary>
        /// <param name="sprite">A sprite object with an hitbox</param>
        private bool IsTouchingRight(GenericSprite sprite)
        {
            return this.Hitbox.Left + this.Velocity.X <= sprite.hitbox.Right &&
              this.Hitbox.Right > sprite.hitbox.Right;
        }

        /// <summary>
        /// Checks if Mario is colliding with only the top part of a sprite
        /// </summary>
        /// <param name="sprite">A sprite object with an hitbox</param>
        private bool IsTouchingTop(GenericSprite sprite)
        {
            return this.Hitbox.Bottom + this.Velocity.Y >= sprite.hitbox.Top &&
              this.Hitbox.Top < sprite.hitbox.Top &&
              this.Hitbox.Right > sprite.hitbox.Left &&
              this.Hitbox.Left < sprite.hitbox.Right;
        }

        /// <summary>
        /// Checks if Mario is colliding with only the bottom part of a sprite
        /// </summary>
        /// <param name="sprite">A sprite object with an hitbox</param>
        private bool IsTouchingBottom(GenericSprite sprite)
        {
            return this.Hitbox.Top + this.Velocity.Y < sprite.hitbox.Bottom &&
              this.Hitbox.Bottom > sprite.hitbox.Bottom &&
              this.Hitbox.Right > sprite.hitbox.Left &&
              this.Hitbox.Left < sprite.hitbox.Right;
        }
        #endregion

        #endregion
        /// <summary>
        /// Returns if mario collided with one of the barrels
        /// </summary>
        public bool IsMarioDead()
        {
            return _marioCollidedWithBarrel;
        }

        #region Mario's animations and sounds
        /// <summary>
        /// All mario animations and sounds are controlled through here.
        /// Jumping, walking, climbing ladders or being idle.
        /// </summary>
        protected override void SetAnimationsAndSounds()
        {
            //If mario is moving upwords or downwards in ladder, he is climbing 
            if ((Velocity.Y < 0 || Velocity.Y > 0) && _inGame && _onLaddder)
            {
                //Play the climbing animation
                _animationManager.Play(_animations["Climb"]);
                //If the climbing sound is not already playing, play it
                if (_soundInstanceClimbing.State != SoundState.Playing)
                {
                    _soundInstanceClimbing.Play();
                }
                //If the walking sound is playing stop it
                if (_soundInstanceWalking.State == SoundState.Playing)
                {
                    _soundInstanceWalking.Stop();
                }
                //If the jumping sound is playing stop it
                if (_soundInstanceJump.State == SoundState.Playing)
                {
                    _soundInstanceJump.Stop();
                }


            }//If mario is Jumping
            else if (Velocity.Y < 0 && _inGame)
            {
                //Check if he last was going right or left to start the correct animation
                if (_isGoingRight)
                {
                    _animationManager.Play(_animations["JumpRight"]);
                }
                else
                {
                    _animationManager.Play(_animations["JumpLeft"]);
                }

                //If the walking sound is playing stop it
                if (_soundInstanceWalking.State == SoundState.Playing)
                {
                    _soundInstanceWalking.Stop();
                }
                //If the jumping sound is not already playing, play it
                if (_soundInstanceJump.State != SoundState.Playing)
                {
                    _soundInstanceJump.Play();
                }

            }//If going right
            else if (Velocity.X > 0 && _onGround)
            {
                //Start the walking right animation
                _animationManager.Play(_animations["WalkRight"]);

                //If the walking sound is not already playing, play it
                if (_soundInstanceWalking.State != SoundState.Playing && _onGround)
                {
                    _soundInstanceWalking.Play();
                }

            }//If going left
            else if (Velocity.X < 0 && _onGround)
            {
                //Start the walking left animation
                _animationManager.Play(_animations["WalkLeft"]);

                //If the walking sound is not already playing, play it
                if (_soundInstanceWalking.State != SoundState.Playing && _onGround)
                {
                    _soundInstanceWalking.Play();
                }

            }//If movement is stopped, aka mario is idle
            else if (Velocity.X == 0 && _onGround)
            {
                //If he last was going right
                if (_isGoingRight)
                {
                    _animationManager.Play(_animations["WalkRight"]);
                }
                else
                {
                    _animationManager.Play(_animations["WalkLeft"]);
                }

                //Stop the animation at the second frame
                _animationManager.Stop();

                //If the walking sound is playing stop it
                if (_soundInstanceWalking.State == SoundState.Playing)
                {
                    _soundInstanceWalking.Stop();
                }
            }
            else // In case mario is not in one of the other conditions, but should not occur
            {
                _animationManager.Stop();
            }




        }


        /// <summary>
        /// Used to reset sound instances.
        /// For example when Mario dies we want the sounds to stop so they dont carry over to his next life.
        /// </summary>
        public void StopAllSoundInstances()
        {
            _soundInstanceJump.Stop();
            _soundInstanceWalking.Stop();
            _soundInstanceClimbing.Stop();
        }
        #endregion

        #endregion

    }
}
