/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using System.Collections.Generic;
using DonkeyKong.Controls;
using DonkeyKong.GameComponents;
using DonkeyKong.Managers;
using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DonkeyKong.States
{
    /// <summary>
    /// Displays a page where a tutorial on how to play the game can be found and also the credits
    /// </summary>
    class InfoPage : State
    {

        SpriteFont arcadeClassic;

        GenericSprite DKTitle;

        GenericSprite joystickRightLeft;
        GenericSprite marioWalking;

        GenericSprite joystickUpDown;
        GenericSprite marioClimbing;

        GenericSprite arcadeButtons;
        GenericSprite marioJumping;

        MenuButton goBackButton;

        private Mario _mario;
        Dictionary<string, Animation> animationsMovementMario;

        public InfoPage(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

            DKTitle = new GenericSprite(_game);

            joystickRightLeft = new GenericSprite(_game);
            marioWalking = new GenericSprite(_game);

            joystickUpDown = new GenericSprite(_game);
            marioClimbing = new GenericSprite(_game);

            arcadeButtons = new GenericSprite(_game);
            marioJumping = new GenericSprite(_game);

            goBackButton = new MenuButton(_game);
            LoadContent();
            Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game to load all the content
        /// </summary>
        public void LoadContent()
        {

            arcadeClassic = _content.Load<SpriteFont>( "arcadeClassic");

            DKTitle.LoadContent( "Graphics/DKTitle");

            joystickRightLeft.LoadContent( "Graphics/JoystickRightLeft");
            marioWalking.LoadContent( "Graphics/MarioMovingExample");

            joystickUpDown.LoadContent( "Graphics/JoystickUpDown");
            marioClimbing.LoadContent( "Graphics/MarioClimbingExample");

            arcadeButtons.LoadContent( "Graphics/ArcadeButtons");
            marioJumping.LoadContent( "Graphics/MarioJumpingExample");

            goBackButton.LoadContent( "Controls/GoBackButton");


            animationsMovementMario = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkRight"),3)},
                { "WalkLeft", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkLeft"), 3)},
                { "WalkDown", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkRight"),3)},
                { "WalkUp", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkRight"), 3)},
            };

            Dictionary<string, SoundEffect> marioSOundEffects = new Dictionary<string, SoundEffect>()
            {
                {"Walking", _content.Load<SoundEffect>( "Sounds/SoundEffects/walking") },
                {"Jump", _content.Load<SoundEffect>( "Sounds/SoundEffects/jump") },
                {"Climbing", _content.Load<SoundEffect>( "Sounds/SoundEffects/marioClimb") },
            };

            _mario = new Mario(_game, animationsMovementMario, marioSOundEffects, _graphicsDevice)
            {
                Position = new Vector2(_graphicsDevice.Viewport.Width * 0.75f, _graphicsDevice.Viewport.Height * 0.85f),
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                }
            };
        }

        /// <summary>
        /// Ininializes all the necessary variables before the game starts
        /// </summary>
        public void Initialize()
        {
            DKTitle.Initialize(new Vector2(_graphicsDevice.Viewport.Width / 2 - DKTitle._texture.Width / 2, _graphicsDevice.Viewport.Height * 0.05f));
            joystickRightLeft.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.1f, _graphicsDevice.Viewport.Height * 0.3f));
            marioWalking.Initialize(new Vector2(joystickRightLeft._position.X + joystickRightLeft._texture.Width + _graphicsDevice.Viewport.Width * 0.1f, _graphicsDevice.Viewport.Height * 0.3f));
            
            marioClimbing.Initialize(new Vector2( _graphicsDevice.Viewport.Width * 0.8f, _graphicsDevice.Viewport.Height * 0.3f));
            joystickUpDown.Initialize(new Vector2(marioClimbing._position.X - joystickUpDown ._texture.Width - _graphicsDevice.Viewport.Width * 0.1f, _graphicsDevice.Viewport.Height * 0.3f - joystickUpDown._texture.Height / 7));

            arcadeButtons.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.1f - arcadeButtons._texture.Width / 4, _graphicsDevice.Viewport.Height * 0.5f));
            marioJumping.Initialize(new Vector2(arcadeButtons._position.X + arcadeButtons._texture.Width + _graphicsDevice.Viewport.Width * 0.1f, _graphicsDevice.Viewport.Height * 0.5f));

            goBackButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.85f, _graphicsDevice.Viewport.Height * 0.8f));
            goBackButton.Input = new Input()
            {
                Action = new List<Keys>() { Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N }
            };
        }


        /// <summary>
        /// Runs the state's logic, collisions, etc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            DKTitle.Update(gameTime);
            joystickRightLeft.Update(gameTime);
            marioWalking.Update(gameTime);

            marioClimbing.Update(gameTime);
            joystickUpDown.Update(gameTime);

            arcadeButtons.Update(gameTime);
            marioJumping.Update(gameTime);

            _mario.Update(gameTime);


            goBackButton.Update(gameTime, _mario.Hitbox);

            ChangeStateConditions();
        }

        /// <summary>
        /// Conditions needed to go another state
        /// </summary>
        private void ChangeStateConditions()
        {
            if (goBackButton.ButtonPressed())
            {
                _mario.StopAllSoundInstances();
                _game.ChangeState(new HomeMenu(_game, _graphicsDevice, _content));
            }
        }

        /// <summary>
        /// All objects are drawn here
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(arcadeClassic, "Original game by nintendo", new Vector2(_graphicsDevice.Viewport.Width * 0.05f , _graphicsDevice.Viewport.Height * 0.7f), Color.White);
            spriteBatch.DrawString(arcadeClassic, "Recreated by Tiago Gama", new Vector2(_graphicsDevice.Viewport.Width * 0.05f, _graphicsDevice.Viewport.Height * 0.7f + arcadeClassic.MeasureString("Original game by nintendo").Y), Color.White);

            _graphicsDevice.Clear(Color.Black);
            DKTitle.Draw(spriteBatch);

            joystickRightLeft.Draw(spriteBatch);
            marioWalking.Draw(spriteBatch);

            marioClimbing.Draw(spriteBatch);
            joystickUpDown.Draw(spriteBatch);

            arcadeButtons.Draw(spriteBatch);
            marioJumping.Draw(spriteBatch);

            goBackButton.Draw(spriteBatch);
            _mario.Draw(spriteBatch);
        }
    }
}
