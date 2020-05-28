using DonkeyKong.GameComponents;
using DonkeyKong.Managers;
using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.States
{
    class DonkeyKong : State
    {
        Dictionary<string, List<Brick>> ground;
        List<Brick> lineOfBricks;
        Brick brick;

        private Mario _mario;
        Dictionary<string, Animation> animationsMovementMario;

        public DonkeyKong(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

            brick = new Brick(_game);
            ground = new Dictionary<string, List<Brick>>();

            LoadContent();
            Initialize();
        }

        public void LoadContent()
        {
            brick.LoadContent("Graphics/Ground");

            animationsMovementMario = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"),3)},
                { "WalkLeft", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkLeft"), 3)},
                { "WalkDown", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"),3)},
                { "WalkUp", new Animation(_content.Load<Texture2D>("Graphics/Animations/MarioWalkRight"), 3)},
            };

            _mario = new Mario(_game, animationsMovementMario, _graphicsDevice, true)
            {
                Position = new Vector2(_graphicsDevice.Viewport.Width * 0.75f, _graphicsDevice.Viewport.Height * 0.87f),
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                }
            };
        }

        public void Initialize()
        {
            brick.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0f, _graphicsDevice.Viewport.Height - brick._texture.Height));
            lineOfBricks = new List<Brick>();
            GroundLayoutSpawn();

        }

        private void GroundLayoutSpawn()
        {          

            int nbBricksTotal = _graphicsDevice.Viewport.Width/ brick._texture.Width;
            for (int i = 0; i <= nbBricksTotal; i++)
            {
                Brick b = (Brick)brick.Clone();
                b._position.X = brick._position.X + brick._texture.Width * i;
                b._position.Y = brick._position.Y;
                lineOfBricks.Add(b);

            }

            ground.Add("Level0", lineOfBricks);
            lineOfBricks = new List<Brick>();

            int bricksHeight = brick._texture.Height * 7;
            int spaceLeft = _graphicsDevice.Viewport.Height - bricksHeight;
            int spacePerBrick = spaceLeft / 7;

            bool stickToTheRight = false;

            int nbBricks = (int)(_graphicsDevice.Viewport.Width * 0.85f) / brick._texture.Width;
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j <= nbBricks; j++)
                {
                    Brick b = (Brick)brick.Clone();
                    if (stickToTheRight)
                    {
                        b._position.X = _graphicsDevice.Viewport.Width - brick._texture.Width * (j+1) ;
                    }
                    else
                    {
                        b._position.X = brick._position.X + brick._texture.Width * j;
                        
                    }
                    
                    b._position.Y = brick._position.Y - brick._texture.Height * i - spacePerBrick * i;
                    lineOfBricks.Add(b);
                }

                ground.Add("Level" + i.ToString(), lineOfBricks);
                lineOfBricks = new List<Brick>();

                stickToTheRight = !stickToTheRight;
            }
        }

        public override void Update(GameTime gameTime)
        {
            brick.Update(gameTime);

            foreach (List<Brick> lb in ground.Values)
            {
                foreach (Brick b in lb)
                {
                    b.Update(gameTime);
                }
            }

            _mario.Update(gameTime, ground);


        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            brick.Draw(spriteBatch);

            foreach (List<Brick> lb in ground.Values)
            {
                foreach (Brick b in lb)
                {
                    b.Draw(spriteBatch);
                }
            }

            _mario.Draw(spriteBatch);
        }


    }
}
