using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonkeyKong.Controls;
using DonkeyKong.GameComponents;
using DonkeyKong.Managers;
using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DonkeyKong.States
{
    class InfoPage : State
    {

        GenericSprite DKTitle;
        GenericSprite joystick;
        GenericSprite joystickInfoText;
        GenericSprite arcadeButtons;
        GenericSprite arcadeButtonsInfoText;

        MenuButton goBackButton;

        private Mario _mario;
        Dictionary<string, Animation> animationsMovementMario;

        public InfoPage(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

            DKTitle = new GenericSprite(_game);
            joystick = new GenericSprite(_game);
            joystickInfoText = new GenericSprite(_game);
            arcadeButtons = new GenericSprite(_game);
            arcadeButtonsInfoText = new GenericSprite(_game);
            goBackButton = new MenuButton(_game);
            LoadContent();
            Initialize();

        }

        public void Initialize()
        {
            DKTitle.Initialize(new Vector2(_graphicsDevice.Viewport.Width / 2 - DKTitle._texture.Width / 2, _graphicsDevice.Viewport.Height * 0.05f));
            joystick.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.1f, _graphicsDevice.Viewport.Height * 0.25f));
            joystickInfoText.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.25f, _graphicsDevice.Viewport.Height * 0.3f));
            arcadeButtons.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.03f, _graphicsDevice.Viewport.Height * 0.6f));
            arcadeButtonsInfoText.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.25f, _graphicsDevice.Viewport.Height * 0.65f));
            goBackButton.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0.85f, _graphicsDevice.Viewport.Height * 0.75f));
        }

        public void LoadContent()
        {
            DKTitle.LoadContent("Graphics/DKTitle");
            joystick.LoadContent("Graphics/Joystick");
            joystickInfoText.LoadContent("Graphics/JoystickInfoText");
            arcadeButtons.LoadContent("Graphics/ArcadeButtons");
            arcadeButtonsInfoText.LoadContent("Graphics/ArcadeButtonsInfoText");
            goBackButton.LoadContent("Controls/GoBackButton");

            animationsMovementMario = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"),3)},
                { "WalkLeft", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkLeft"), 3)},
                { "WalkDown", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"),3)},
                { "WalkUp", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"), 3)},
            };

            _mario = new Mario(_game, animationsMovementMario, _graphicsDevice)
            {
                Position = new Vector2(_graphicsDevice.Viewport.Width * 0.75f, _graphicsDevice.Viewport.Height * 0.8f),
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            DKTitle.Update(gameTime);
            joystick.Update(gameTime);
            joystickInfoText.Update(gameTime);
            arcadeButtons.Update(gameTime);
            arcadeButtonsInfoText.Update(gameTime);
            _mario.Update(gameTime);

          
            goBackButton.Update(gameTime, _mario.Hitbox);
            


            if (goBackButton.ButtonPressed())
            {
                _game.ChangeState(new HomeMenu(_game, _graphicsDevice, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            DKTitle.Draw(spriteBatch);
            joystick.Draw(spriteBatch);
            joystickInfoText.Draw(spriteBatch);
            arcadeButtons.Draw(spriteBatch);
            arcadeButtonsInfoText.Draw(spriteBatch);
            goBackButton.Draw(spriteBatch);
            _mario.Draw(spriteBatch);
        }
    }
}
