
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using DonkeyKong.Sprites;
using DonkeyKong.Models;
using DonkeyKong.Managers;
using Microsoft.Xna.Framework.Input;
using DonkeyKong.Controls;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using DonkeyKong.GameComponents;

namespace DonkeyKong.States
{
    /// <summary>
    ///  Just a menu with Play and Exit buttons
    ///  Original source : https://github.com/Oyyou/MonoGame_Tutorials
    /// </summary>
    class HomeMenu : State
    {

        #region Variables

        MenuButton playButton;
        MenuButton infoButton;
        MenuButton exitButton;

        private Mario _mario;

        private List<AnimatedSprite> _menuBarrels;

        private AnimatedSprite _menuKong;

        Dictionary<string, Animation> animationsMovementMario;

        Song backgroundMusic;

        GenericSprite DKTitle;

        #endregion

        #region Constructor

        public HomeMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {

            DKTitle = new GenericSprite(_game);
            playButton = new MenuButton(_game);
            infoButton = new MenuButton(_game);
            exitButton = new MenuButton(_game);

            LoadContent();
            Initialize();
            
            
        }

        private void Initialize()
        {
          
            DKTitle.Initialize(new Vector2(_graphicsDevice.Viewport.Width / 2 - DKTitle._texture.Width / 2, _graphicsDevice.Viewport.Height * 0.05f));
            playButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.25f, _graphicsDevice.Viewport.Height * 0.75f));
            infoButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.45f, _graphicsDevice.Viewport.Height * 0.75f));
            exitButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.65f, _graphicsDevice.Viewport.Height * 0.75f));


        }

        private void LoadContent()
        {
            animationsMovementMario = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"),3)},
                { "WalkLeft", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkLeft"), 3)},
                { "WalkDown", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"),3)},
                { "WalkUp", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"), 3)},
            };

            _mario = new Mario(_game, animationsMovementMario, _graphicsDevice)
            {
                Position = new Vector2(100, _graphicsDevice.Viewport.Height * 0.8f),
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                }
            };

            var animationsMenuBarrels1 = new Dictionary<string, Animation>()
            {
                { "Animated", new Animation(_content.Load<Texture2D>("Graphics/Animations/MenuBarrels"),2)},
            };

            var animationsMenuBarrels2 = new Dictionary<string, Animation>()
            {
                { "Animated", new Animation(_content.Load<Texture2D>("Graphics/Animations/MenuBarrels"),2)},
            };

            _menuBarrels = new List<AnimatedSprite>()
            {
                new AnimatedSprite(_game, animationsMenuBarrels1)
                {
                    
                    Position = new Vector2(_graphicsDevice.Viewport.Width * 0.05f,_graphicsDevice.Viewport.Height * 0.3f),
                },
                new AnimatedSprite(_game, animationsMenuBarrels2)
                {

                    Position = new Vector2(_graphicsDevice.Viewport.Width * 0.85f,_graphicsDevice.Viewport.Height * 0.3f),
                },
            };

            var animationsMenuKong = new Dictionary<string, Animation>()
            {
                { "Animated", new Animation(_content.Load<Texture2D>("Graphics/Animations/KongIdleAnimation"),3)},
            };


            _menuKong = new AnimatedSprite(_game, animationsMenuKong)
            {

                Position = new Vector2(_graphicsDevice.Viewport.Width * 0.4f, _graphicsDevice.Viewport.Height * 0.3f),
                SoundEffect = _game.Content.Load<SoundEffect>("Sounds/SoundEffects/jumpbar")
            };

            backgroundMusic = _game.Content.Load<Song>("Sounds/Music/backmusic");

            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(backgroundMusic);


            DKTitle.LoadContent("Graphics/DKTitle");
            playButton.LoadContent("Controls/PlayButton");
            infoButton.LoadContent("Controls/InfoButton");
            exitButton.LoadContent("Controls/ExitButton");
        }
        #endregion

        #region Updates + Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            DKTitle.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
            infoButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);

            _mario.Draw(spriteBatch);

            foreach (var sprite in _menuBarrels)
                sprite.Draw(spriteBatch);
            
            _menuKong.Draw(spriteBatch);
            
        }

        public override void Update(GameTime gameTime)
        {
            DKTitle.Update(gameTime);

            
            playButton.Update(gameTime, _mario.Hitbox);
            infoButton.Update(gameTime, _mario.Hitbox);
            exitButton.Update(gameTime, _mario.Hitbox);

            if (playButton.ButtonPressed())
            {
                _game.ChangeState(new DonkeyKong(_game, _graphicsDevice, _content));
                MediaPlayer.Stop();
            }
            else if (infoButton.ButtonPressed())
            {
                _game.ChangeState(new InfoPage(_game, _graphicsDevice, _content));
                MediaPlayer.Stop();
            }
            else if (exitButton.ButtonPressed())
            {
                _game.Exit();
            }

            foreach (var sprite in _menuBarrels)
                sprite.Update(gameTime);

            _mario.Update(gameTime);
            _menuKong.Update(gameTime);


        }
        #endregion

    }
}
