using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
    class Kong : AnimatedSprite
    {
        private float timer;
        private Random rnd = new Random();
        private bool _firstTimeOnAnimation;

        private bool isDoneWithThrowingBarrel = false;


        Dictionary<string, SoundEffect> _soundEffects;
        SoundEffectInstance _soundInstance;

        /// <summary>
        /// Kong constructor with animations and sound effects
        /// </summary>
        /// <param name="game">The game variable</param>
        /// <param name="animations">All kong animations</param>
        /// <param name="soundEffects">All kong sound effects</param>
        public Kong(Game game, Dictionary<string, Animation> animations, Dictionary<string, SoundEffect> soundEffects) : base(game, animations)
        {
            //Change the default frame speed
            foreach (Animation a in animations.Values)
            {
                a.FrameSpeed = 0.5f;
            }

            //Kong sound effects
            _soundEffects = soundEffects;
            _soundInstance = _soundEffects["Idle"].CreateInstance();

            //Initializing kong variables at the start of the game
            _firstTimeOnAnimation = false;
            timer = 1;
            rnd = new Random();
        }

        /// <summary>
        /// Every time the timer reaches 0 start the grabBarrel animation
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        protected void SetAnimations(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;

            //Timer expired, execute action
            if (timer < 0)
            {
                //Reset Timer between 2s and 4s
                timer = rnd.Next(2, 5);   
                _animationManager.Play(_animations["GrabBarrel"]);

            }                        
        }

        /// <summary>
        /// Runs the kong logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            SetAnimations(gameTime);
            AnimationLogic();

            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if its the first time on an animation
        /// </summary>
        private void AnimationLogic()
        {
            if (_animationManager._animation == _animations["GrabBarrel"] && _animations["GrabBarrel"].CurrentFrame == 1)
            {
                _firstTimeOnAnimation = true;
            }
            else if (_animationManager._animation == _animations["GrabBarrel"] && _animations["GrabBarrel"].CurrentFrame == 2)
            {
                _firstTimeOnAnimation = false;
            }

            if (_animationManager._animation == _animations["GrabBarrel"] && _soundInstance.State != SoundState.Playing)
            {
                _soundInstance.Play();
            }
        }

        /// <summary>
        /// If its the first time on the grabBarrel animation return that you can now spawn a barrel.
        /// If its not than start the idle animation
        /// </summary>
        public bool CanSpawnBarrel()
        {
            bool canSpawnBarrel = false;
            if (_animationManager._animation == _animations["GrabBarrel"] && _animations["GrabBarrel"].CurrentFrame == 2 && _firstTimeOnAnimation == true)
            {
                canSpawnBarrel = true;
                isDoneWithThrowingBarrel = canSpawnBarrel;

            }


            if (isDoneWithThrowingBarrel == true && _animationManager._animation == _animations["GrabBarrel"] && _animations["GrabBarrel"].CurrentFrame == 0)
            {
                isDoneWithThrowingBarrel = false;
                _animationManager.Play(_animations["Idle"]);


            }
            return canSpawnBarrel;
        }


        
        
    }
}
