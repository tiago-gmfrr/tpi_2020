/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/

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
using Microsoft.Xna.Framework.Audio;
using DonkeyKong.GameComponents;

namespace DonkeyKong.States
{
    /// <summary>
    ///  The first page the user meets, they will be able to control Mario and press any of the buttons to activate their corresponding actions, such as playing the game, going to the info page or exiting the game
    ///  Inspiration : https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial013
    /// </summary>
    class HomeMenu : State
    {


        private MenuButton playButton;
        private MenuButton infoButton;
        private MenuButton exitButton;

        private Mario _mario;

        private List<AnimatedSprite> _menuBarrels;

        private AnimatedSprite _menuKong;

        private Dictionary<string, Animation> animationsMovementMario;

        private Song menuBackgroundMusic;
        private Song gameBackgroundMusic;


        private GenericSprite DKTitle;


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

        /// <summary>
        /// LoadContent will be called once per game to load all the content
        /// </summary>
        private void LoadContent()
        {
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
                { "Animated", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MenuBarrels"),2)},
            };

            var animationsMenuBarrels2 = new Dictionary<string, Animation>()
            {
                { "Animated", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MenuBarrels"),2)},
            };

            _menuBarrels = new List<AnimatedSprite>()
            {
                new AnimatedSprite(_game, animationsMenuBarrels1)
                {

                    Position = new Vector2(_graphicsDevice.Viewport.Width * 0.05f,_graphicsDevice.Viewport.Height * 0.35f),
                },
                new AnimatedSprite(_game, animationsMenuBarrels2)
                {

                    Position = new Vector2(_graphicsDevice.Viewport.Width * 0.82f,_graphicsDevice.Viewport.Height * 0.35f),
                },
            };

            var animationsMenuKong = new Dictionary<string, Animation>()
            {
                { "Animated", new Animation(_content.Load<Texture2D>( "Graphics/Animations/KongIdleAnimation"),3)},
            };


            _menuKong = new AnimatedSprite(_game, animationsMenuKong);

            _menuKong.Position = new Vector2(_graphicsDevice.Viewport.Width / 2 - _menuKong._width / 2, _graphicsDevice.Viewport.Height * 0.45f);


            gameBackgroundMusic = _game.Content.Load<Song>( "Sounds/Music/gameMusic");
            menuBackgroundMusic = _game.Content.Load<Song>( "Sounds/Music/menuMusic");


            MediaPlayer.Play(menuBackgroundMusic);
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;

            DKTitle.LoadContent("Graphics/DKTitle");
            playButton.LoadContent("Controls/PlayButton");
            infoButton.LoadContent("Controls/InfoButton");
            exitButton.LoadContent("Controls/ExitButton");
        }

        /// <summary>
        /// Ininializes all the necessary variables before the game starts
        /// </summary>
        private void Initialize()
        {

            DKTitle.Initialize(new Vector2(_graphicsDevice.Viewport.Width / 2 - DKTitle._texture.Width / 2, _graphicsDevice.Viewport.Height * 0.05f));
            playButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.25f, _graphicsDevice.Viewport.Height * 0.75f));
            playButton.Input = new Input()
            {
                Action = new List<Keys>() { Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N }
            };
            infoButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.45f, _graphicsDevice.Viewport.Height * 0.75f));
            infoButton.Input = new Input()
            {
                Action = new List<Keys>() { Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N }
            };
            exitButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.65f, _graphicsDevice.Viewport.Height * 0.75f));
            exitButton.Input = new Input()
            {
                Action = new List<Keys>() { Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N }
            };

        }

        /// <summary>
        /// Runs the state's logic, collisions, etc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param
        public override void Update(GameTime gameTime)
        {
            DKTitle.Update(gameTime);


            playButton.Update(gameTime, _mario.Hitbox);
            infoButton.Update(gameTime, _mario.Hitbox);
            exitButton.Update(gameTime, _mario.Hitbox);
            ChangeStateConditions();

            foreach (var sprite in _menuBarrels)
                sprite.Update(gameTime);

            _mario.Update(gameTime);
            _menuKong.Update(gameTime);


        }

        /// <summary>
        /// Conditions needed to go another state
        /// </summary>
        private void ChangeStateConditions()
        {
            if (playButton.ButtonPressed())
            {
                MediaPlayer.Play(gameBackgroundMusic);
                MediaPlayer.Volume = 0.5f;
                _mario.StopAllSoundInstances();
                _game.ChangeState(new DonkeyKong(_game, _graphicsDevice, _content));

            }
            else if (infoButton.ButtonPressed())
            {
                _mario.StopAllSoundInstances();
                MediaPlayer.Stop();
                _game.ChangeState(new InfoPage(_game, _graphicsDevice, _content));


            }
            else if (exitButton.ButtonPressed())
            {
                _game.Exit();
            }
        }

        /// <summary>
        /// All objects are drawn here
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
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

    }
}
