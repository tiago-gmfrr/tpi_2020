
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

        private List<MovingAnimatedSprite> _mario;

        private List<AnimatedSprite> _menuBarrels;

        private List<AnimatedSprite> _menuKong;

        Dictionary<string, Animation> animationsMovementMario;


        Game1 _game;

        GenericSprite DKTitle;

        GraphicsDevice _graphicsDevice;
        #endregion

        #region Constructor

        public HomeMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;

            DKTitle = new GenericSprite(game);
            playButton = new MenuButton(game);

            LoadContent();
            Initialize();
            
            
        }

        private void Initialize()
        {
          
            DKTitle.Initialize(new Vector2(_graphicsDevice.Viewport.Width / 2 - DKTitle._texture.Width / 2, _graphicsDevice.Viewport.Height * 0.05f));
            playButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.45f, _graphicsDevice.Viewport.Height * 0.7f));
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

            _mario = new List<MovingAnimatedSprite>()
            {
                new MovingAnimatedSprite(_game, animationsMovementMario)
                {
                    Position = new Vector2(100,_graphicsDevice.Viewport.Height * 0.8f),
                    Input = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                    }
                },
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

            _menuKong = new List<AnimatedSprite>()
            {
                new AnimatedSprite(_game, animationsMenuKong)
                {

                    Position = new Vector2(_graphicsDevice.Viewport.Width * 0.4f,_graphicsDevice.Viewport.Height * 0.3f),
                },
            };


            DKTitle.LoadContent("Graphics/DKTitle");
            playButton.LoadContent("Controls/PlayButton");
        }
        #endregion

        #region Updates + Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            DKTitle.Draw(spriteBatch);
            playButton.Draw(spriteBatch);

            foreach (var sprite in _mario)
                sprite.Draw(spriteBatch);

            foreach (var sprite in _menuBarrels)
                sprite.Draw(spriteBatch);

            foreach (var sprite in _menuKong)
                sprite.Draw(spriteBatch);
            
        }


        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            DKTitle.Update(gameTime);
            playButton.Update(gameTime, new Rectangle( (int)_mario[0].Position.X, (int)_mario[0].Position.Y, animationsMovementMario.First().Value.Texture.Width /3 , animationsMovementMario.First().Value.Texture.Height));

            if (playButton.StartGame())
            {
                _game.Exit();
            }

            foreach (var sprite in _menuBarrels)
                sprite.Update(gameTime, _menuBarrels);


            foreach (var sprite in _mario)
                sprite.Update(gameTime, _mario);


            foreach (var sprite in _menuKong)
                sprite.Update(gameTime, _menuKong);


        }
        #endregion

        #region Button events
        /// <summary>
        /// Starts the game in survival mode, which is represented has "level 0"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayGameButton_Click(object sender, EventArgs e)
        {
            // _game.ChangeState(new DonkeyKong(_game, _graphicsDevice, _content));

        }
        /// <summary>
        /// Exits the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
        #endregion
    }
}
