/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using DonkeyKong.GameComponents;
using DonkeyKong.Managers;
using DonkeyKong.Models;
using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DonkeyKong.States
{
    /// <summary>
    /// Game state, this is where the game will take place
    /// </summary>
    class DonkeyKong : State
    {

        //Brick variables
        Dictionary<string, List<Brick>> ground;
        List<Brick> lineOfBricks;
        //Examplary brick used to spawn all the others
        Brick brick;

        //Ladder variables
        Ladder ladder;
        List<Ladder> allLadders;

        //Barrel variables
        List<Barrel> allBarrels;
        List<Barrel> toBeRemovedBarrels;

        //Mario variables
        Mario _mario;
        Dictionary<string, Animation> animationsMovementMario;
        Dictionary<string, SoundEffect> marioSoundEffects;
        int _livesLeft;
        List<GenericSprite> allLives; 

        //Kong variables
        Kong _kong;
        Dictionary<string, Animation> animationsKong;
        Dictionary<string, SoundEffect> kongSoundEffects;

        //Princess variables
        Dictionary<string, Animation> princessAnimations;
        AnimatedSprite _princess;

        //Oil barrel variables
        Dictionary<string, Animation> oilBarrelAnimations;
        AnimatedSprite oilBarrel;

        GenericSprite stackedBarrels;

        //Fonts
        SpriteFont arcadeClassic;
        SpriteFont arcadeClassicBig;

        //Game variables
        GameTimer _gameTimer;
        string score;
        ScoreManager _scoreManager;
        bool gameWon;
        bool gameOver;
        bool scoreSaved;
        float timer;

        //Game sounds
        SoundEffect gameOverSound;
        SoundEffectInstance gameOverInstance;
        SoundEffect gameWonSound;
        SoundEffectInstance gameWonInstance;


        /// <param name="game">The game variable</param>
        /// <param name="graphicsDevice">Information about the screen used to display the game</param>
        /// <param name="content"></param>
        /// <param name="livesLeft">How many lives does mario have left</param>
        /// <param name="gameDuration">How long has the game been going</param>
        public DonkeyKong(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int livesLeft = 3, float gameDuration = 0f) : base(game, graphicsDevice, content)
        {
            //Initialize the game's variables at the start of the game
            _livesLeft = livesLeft;
            _gameTimer = new GameTimer(game, gameDuration);
            _gameTimer.Started = true;
            brick = new Brick(_game);
            stackedBarrels = new GenericSprite(_game);
            gameWon = false;
            gameOver = false;
            scoreSaved = false;

            _scoreManager = ScoreManager.Load();
            LoadContent();
            Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game to load all the content
        /// </summary>
        public void LoadContent()
        {
            brick.LoadContent( "Graphics/Ground");
            stackedBarrels.LoadContent( "Graphics/StackedBarrels");

            arcadeClassic = _content.Load<SpriteFont>( "arcadeClassic");
            arcadeClassicBig = _content.Load<SpriteFont>( "arcadeClassicBig");
            animationsMovementMario = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkRight"),3)},
                { "WalkLeft", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkLeft"), 3)},
                { "WalkDown", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioWalkRight"),3)},
                { "Climb", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioClimb"), 2)},
                { "JumpRight", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioJumpingRight"), 2)},
                { "JumpLeft", new Animation(_content.Load<Texture2D>( "Graphics/Animations/MarioJumpingLeft"), 2)},
            };

            marioSoundEffects = new Dictionary<string, SoundEffect>()
            {
                { "Walking", _content.Load<SoundEffect>( "Sounds/SoundEffects/walking") },
                { "Jump", _content.Load<SoundEffect>( "Sounds/SoundEffects/jump") },
                { "Climbing", _content.Load<SoundEffect>( "Sounds/SoundEffects/marioClimb") },
            };



            animationsKong = new Dictionary<string, Animation>()
            {
                { "Idle", new Animation(_content.Load<Texture2D>( "Graphics/Animations/KongIdleAnimationSmall"),3)},
                { "GrabBarrel", new Animation(_content.Load<Texture2D>( "Graphics/Animations/KongBarrelAnimation"), 3)},
            };

            kongSoundEffects = new Dictionary<string, SoundEffect>()
            {
                {"Idle", _content.Load<SoundEffect>( "Sounds/SoundEffects/kongStomp") },
            };

            oilBarrelAnimations = new Dictionary<string, Animation>()
            {
                { "Animation", new Animation(_content.Load<Texture2D>( "Graphics/Animations/oilBarrelAnimation"), 2) }
            };

            princessAnimations = new Dictionary<string, Animation>()
            {
                { "Animation", new Animation(_content.Load<Texture2D>( "Graphics/Animations/PrincessAnimation"), 2) }
            };

            gameOverSound = _content.Load<SoundEffect>( "Sounds/SoundEffects/gameOver");
            gameOverInstance = gameOverSound.CreateInstance();
            gameWonSound = _content.Load<SoundEffect>( "Sounds/SoundEffects/gameWon");
            gameWonInstance = gameWonSound.CreateInstance();




        }


        /// <summary>
        /// Ininializes all the necessary variables before the game starts
        /// </summary>
        public void Initialize()
        {
            ground = new Dictionary<string, List<Brick>>();
            
            allLadders = new List<Ladder>();
            allLives = new List<GenericSprite>();

            brick.Initialize(new Vector2(_graphicsDevice.Viewport.Width * 0f, _graphicsDevice.Viewport.Height - brick._texture.Height));
            lineOfBricks = new List<Brick>();
            allBarrels = new List<Barrel>();
            toBeRemovedBarrels = new List<Barrel>();
            GroundLayoutSpawn();
            LadderSpawn();

            _mario = new Mario(_game, animationsMovementMario, marioSoundEffects, _graphicsDevice, true)
            {
                Position = new Vector2(_graphicsDevice.Viewport.Width * 0.1f, _graphicsDevice.Viewport.Height * 0.95f),
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                    Action = new List<Keys>() { Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N }
                },
                Speed = new Vector2(2f, 1.5f),

            };

            stackedBarrels._position = new Vector2(_graphicsDevice.Viewport.Width * 0f + 1, ground["Level5"][0]._position.Y - stackedBarrels._texture.Height);

            LivesLeftDisplay();

            _kong = new Kong(_game, animationsKong, kongSoundEffects);
            _kong.Position = new Vector2(stackedBarrels._texture.Width + (stackedBarrels._texture.Width / 10), ground["Level5"][0]._position.Y - _kong._height);

            _princess = new AnimatedSprite(_game, princessAnimations);
            _princess.Position = new Vector2(ground["Level6"][0]._position.X, ground["Level6"][0]._position.Y - _princess._height);

            oilBarrel = new AnimatedSprite(_game, oilBarrelAnimations);
            oilBarrel.Position = new Vector2(_graphicsDevice.Viewport.Width * 0.01f, brick._position.Y - oilBarrel._height);
        }

        /// <summary>
        /// Creates the layout for game's platforms.
        /// Will adapt to different screen sizes.
        /// </summary>
        private void GroundLayoutSpawn()
        {
            //First layer, generates a straight line of bricks
            int nbBricksTotal = _graphicsDevice.Viewport.Width / brick._texture.Width;
            for (int i = 0; i <= nbBricksTotal; i++)
            {
                Brick b = (Brick)brick.Clone();
                b._position.X = brick._position.X + brick._texture.Width * i;
                b._position.Y = brick._position.Y;
                lineOfBricks.Add(b);
            }

            ground.Add("Level0", lineOfBricks);
            lineOfBricks = new List<Brick>();

            //Layer 1 to 5
            //Gets the amount of bricks needed to fill the height wise
            int bricksHeight = brick._texture.Height * 7;
            int spaceLeft = _graphicsDevice.Viewport.Height - bricksHeight;
            int spacePerBrick = spaceLeft / 7;

            //Inclination to be used for the platforms
            int totalInclination = (int)(_graphicsDevice.Viewport.Height * 0.05);

            //Variable to alternate between sticking to the right or left
            bool stickToTheRight = false;

            //How many bricks can (85% of) a line fit
            int nbBricksPerLine = (int)(_graphicsDevice.Viewport.Width * 0.85f) / brick._texture.Width;
            //Inclination for each brick individually
            int inclinationPerBlock = totalInclination / nbBricksPerLine;

            //Generates the next 5 platforms
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j <= nbBricksPerLine; j++)
                {
                    //Clone a new brick
                    Brick b = (Brick)brick.Clone();
                    //If we're sticking to the right, start from the right of the screen and substract each block added
                    if (stickToTheRight)
                    {
                        b._position.X = _graphicsDevice.Viewport.Width - brick._texture.Width * (j + 1);
                    }
                    else // Start from 0 and add each brick
                    {
                        b._position.X = brick._position.X + brick._texture.Width * j;

                    }

                    if (i == 5) // The 5th platform is special, the inclination isnt for every brick, but only the last few
                    {
                        //1.3 was the number I decided after playing a bit with the numbers, no other specific reason
                        int inclinedBricks = (int)(nbBricksPerLine / 1.3f);

                        if (j > inclinedBricks) // If we've reached the bricks to incline the incline them
                        {
                            int totalInclinationFirstLine = (int)(_graphicsDevice.Viewport.Height * 0.03);
                            int inclinationPerBlockFirstLine = totalInclinationFirstLine / inclinedBricks;

                            b._position.Y = brick._position.Y - brick._texture.Height * i - spacePerBrick * i + inclinationPerBlockFirstLine * (j - inclinedBricks);
                        }
                        else // Otherwise just spawn them in a straight line
                        {
                            b._position.Y = brick._position.Y - brick._texture.Height * i - spacePerBrick * i;
                        }

                    }
                    else //If its not the 5th just incline the entire line
                    {
                        b._position.Y = brick._position.Y - brick._texture.Height * i - spacePerBrick * i + inclinationPerBlock * j;
                    }
                    lineOfBricks.Add(b);
                }

                ground.Add("Level" + i.ToString(), lineOfBricks);
                lineOfBricks = new List<Brick>();

                stickToTheRight = !stickToTheRight;
            }

            //Layer 6
            //For the platform get 1/4 of the total bricks per screen 
            //And create a straight platform for the princess to sit, this were the game will end if Mario reaches it
            int oneFourthOfTotalBricksPerLine = nbBricksTotal / 4;

            for (int i = 0; i < oneFourthOfTotalBricksPerLine; i++)
            {
                Brick b = (Brick)brick.Clone();
                b._position.X = _graphicsDevice.Viewport.Width * 0.4f + brick._position.X + brick._texture.Width * i;
                b._position.Y = _graphicsDevice.Viewport.Height * 0.15f;
                lineOfBricks.Add(b);
            }
            ground.Add("Level6", lineOfBricks);
        }

        /// <summary>
        /// Spawns all the ladders, each ladder will be spawned in the 4th (counting from the end of a line) brick of each line and go down untill it meets a brick.
        /// </summary>
        private void LadderSpawn()
        {
            //Go from the last line to the first
            for (int i = 6; i > 0; i--)
            {
                ladder = new Ladder(_game);
                ladder.LoadContent("Graphics/Ladder");

                Brick closestBrick = null;
                //Start the minimum with the max distance possible
                float minDist = _graphicsDevice.Viewport.Width;
                int startingBrickListPos;

                //If its the princess line use the last brick instead of the 4th counting from the end
                if (i == 6)
                {
                     startingBrickListPos = ground["Level" + i.ToString()].Count - 1;
                }
                else
                {
                     startingBrickListPos = ground["Level" + i.ToString()].Count - 4;
                }
                
                //For each brick in the line below
                foreach (Brick b in ground["Level" + (i - 1).ToString()])
                {
                    //Get the difference between their X positions
                    float dist = Math.Abs(ground["Level" + i.ToString()][startingBrickListPos]._position.X - b._position.X);

                    //And if its smaller than the current minimum distance 
                    if (dist < minDist)
                    {
                        //Set the new min distance and set the closest brick
                        minDist = dist;
                        closestBrick = b;
                    }
                }
                //Get the vertical space between both bricks
                float spaceBetweenPlatforms = brick._texture.Height + closestBrick._position.Y - ground["Level" + i.ToString()][startingBrickListPos]._position.Y;
                //Divide it by the ladder height, to get how many mini ladders we need to form a complete ladder
                ladder.NbSpritesInStack = (int)(spaceBetweenPlatforms / ladder._texture.Height);
                //Place the ladder at the starting brick position
                ladder._position = new Vector2(ground["Level" + i.ToString()][startingBrickListPos]._position.X, ground["Level" + i.ToString()][startingBrickListPos]._position.Y);
                //Add it to the ladder list
                allLadders.Add(ladder);
            }


        }

        /// <summary>
        /// Spawns a barrel when conditions are met
        /// </summary>
        private void BarrelSpawn()
        {
            if (_kong.CanSpawnBarrel())
            {
                //Create a new animation for each barrel, otherwise animations will speed up
                Dictionary<string, Animation> bAnimation = new Dictionary<string, Animation>()
                { { "Animation", new Animation(_content.Load<Texture2D>("Graphics/Animations/barrelAnimation"), 4)}};

                Barrel ba = new Barrel(_game, bAnimation, _graphicsDevice)
                {
                    Position = new Vector2(_kong.hitbox.Right, _kong.Position.Y +  _kong.hitbox.Height / 2),
                    Speed = new Vector2(4f, 3f)
                };

                //Adds the barrel to the barrel list
                allBarrels.Add(ba);

            }

        }

        /// <summary>
        /// Updates the visual mario lives in the upper left corner
        /// </summary>
        private void LivesLeftDisplay()
        {
            for (int i = 0; i < _livesLeft; i++)
            {
                GenericSprite lifeSprite = new GenericSprite(_game);
                lifeSprite.LoadContent("Graphics/marioLife");
                lifeSprite._position = new Vector2(stackedBarrels._position.X + lifeSprite._texture.Width * i, 0);
                allLives.Add(lifeSprite);
            }
        }

        /// <summary>
        /// Runs all the game logic, all the collisions, animations, sounds, etc
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //If the game isnt over
            if (!gameOver)
            {
                //Pressing any of the upper arcade buttons will return to the menu
                if (Keyboard.GetState().IsKeyDown(Keys.D6) ||
                    Keyboard.GetState().IsKeyDown(Keys.D7) ||
                    Keyboard.GetState().IsKeyDown(Keys.D8) ||
                    Keyboard.GetState().IsKeyDown(Keys.D9) ||
                    Keyboard.GetState().IsKeyDown(Keys.D0) ||
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    GoBackToMenu();
                }

                //Update the game timer which is used for the score
                _gameTimer.Update(gameTime);
                score = _gameTimer.Text;
                int scoreLength = score.Length;
                //All display at least 4 digits
                for (int i = 0; i < 4 - scoreLength; i++)
                {
                    score = "0" + score;
                }


                brick.Update(gameTime);
                foreach (List<Brick> lb in ground.Values)
                {
                    foreach (Brick b in lb)
                    {
                        b.Update(gameTime);
                    }
                }

                
                BarrelLogic(gameTime);

                foreach (Ladder l in allLadders)
                {
                    l.Update(gameTime);
                }

                stackedBarrels.Update(gameTime);
                _princess.Update(gameTime);

                foreach (GenericSprite marioLives in allLives)
                {
                    marioLives.Update(gameTime);
                }

                _kong.Update(gameTime);
                _mario.Update(gameTime, ground, allLadders, allBarrels);

                MarioBarrelCollision();

                WinCondition();
            }
            else //If the game is over
            {
                if (gameWon) //And you won it
                {
                    //Save your score unless you already did it
                    if (!scoreSaved)
                    {
                        _scoreManager.Add(new Score()
                        {
                            PlayerName = "NONAME",
                            Value = score,
                        });
                        ScoreManager.Save(_scoreManager);
                        scoreSaved = true;
                    }

                    //Play the win game music
                    gameWonInstance.Play();
                }
                else //Or you lost it
                {
                    //Play the lose game music
                    gameOverInstance.Play();
                }

                //Start a timer and when it reaches 0 go back to the menu
                //Timer's length is equal to the audio length
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer -= elapsed;
                if (timer < 0)
                {              
                    GoBackToMenu();
                }
            }

        }

        /// <summary>
        /// Checks whenever mario hits a barrel, if he has enough lives to keep playing or if it is a game over.
        /// </summary>
        private void MarioBarrelCollision()
        {
            if (_mario.IsMarioDead() == true)
            {
                _livesLeft--;
                if (_livesLeft > 0)
                {
                    _game.ChangeState(new DonkeyKong(_game, _graphicsDevice, _content, _livesLeft, Convert.ToInt32(score)));
                }
                else
                {
                    gameOver = true;
                    //The 3s represent the losing music audio length
                    timer = 3;
                }

            }
        }

        /// <summary>
        /// Checkes for when Mario reaches the princess and wins
        /// </summary>
        private void WinCondition()
        {
            if (_mario.Hitbox.Intersects(_princess.hitbox))
            {
                gameOver = true;
                gameWon = true;
                //The 3s represent the winning music audio length
                timer = 5.5f;//Audio length
            }
        }

        /// <summary>
        /// Changes states and goes to the menu
        /// </summary>
        private void GoBackToMenu()
        {
            //Stops the sound from carrying over to the menu; 
            _mario.StopAllSoundInstances();
            _game.ChangeState(new HomeMenu(_game, _graphicsDevice, _content));            
        }

        /// <summary>
        /// All barrels updates are here and checks for when a barrel needs to be removed from the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private void BarrelLogic(GameTime gameTime)
        {
            oilBarrel.Update(gameTime);

            foreach (Barrel ba in allBarrels)
            {
                ba.Update(gameTime, ground);

                //If the barrel reaches the oil barrel at the end
                if (ba.IsCollidingWithOilBarrel(oilBarrel))
                {
                    //Add it to the being removed list
                    toBeRemovedBarrels.Add(ba);

                }
            }

            //Remove all barrels who reached the end from the barrel list
            foreach (Barrel b in toBeRemovedBarrels)
            {
                allBarrels.Remove(b);
            }           

            toBeRemovedBarrels.Clear();

            BarrelSpawn();
        }

        /// <summary>
        /// All game components are drawn here.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="spriteBatch">Helper class for drawing strings and sprites</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            brick.Draw(spriteBatch);

            foreach (Ladder l in allLadders)
            {
                l.Draw(spriteBatch);
            }


            foreach (List<Brick> lb in ground.Values)
            {
                foreach (Brick b in lb)
                {
                    b.Draw(spriteBatch);
                }
            }

            foreach (Barrel ba in allBarrels)
            {
                ba.Draw(spriteBatch);
            }
            _princess.Draw(spriteBatch);
            stackedBarrels.Draw(spriteBatch);

            foreach (GenericSprite gs in allLives)
            {
                gs.Draw(spriteBatch);
            }
            oilBarrel.Draw(spriteBatch);
            _kong.Draw(spriteBatch);
            _mario.Draw(spriteBatch);


            spriteBatch.DrawString(arcadeClassic, "HIGHSCORE  " + _scoreManager.Highscores[0].Value, new Vector2(_graphicsDevice.Viewport.Width - arcadeClassic.MeasureString("HIGHSCORE  " + _scoreManager.Highscores[0].Value).X, 0), Color.White);
            spriteBatch.DrawString(arcadeClassic, "SCORE  " + score, new Vector2(_graphicsDevice.Viewport.Width - arcadeClassic.MeasureString("SCORE  " + score).X, arcadeClassic.MeasureString("HIGHSCORE  " + _scoreManager.Highscores.Select(c => c.Value)).Y), Color.White);
            EndGameDisplays(spriteBatch);

        }

        /// <summary>
        /// When the game is over either display a You Win + the score or display Game Over
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void EndGameDisplays(SpriteBatch spriteBatch)
        {
            if (gameWon)
            {
                spriteBatch.DrawString(arcadeClassicBig, "YOU WIN", new Vector2(_graphicsDevice.Viewport.Width / 2 - arcadeClassicBig.MeasureString("YOU WIN").X / 2, _graphicsDevice.Viewport.Height * 0.3f), Color.Red);
                spriteBatch.DrawString(arcadeClassicBig, score, new Vector2(_graphicsDevice.Viewport.Width / 2 - arcadeClassicBig.MeasureString(score).X / 2, _graphicsDevice.Viewport.Height * 0.3f + arcadeClassicBig.MeasureString("YOU WIN").Y), Color.White);
            }
            else if (gameOver)
            {
                spriteBatch.DrawString(arcadeClassicBig, "GAME OVER", new Vector2(_graphicsDevice.Viewport.Width / 2 - arcadeClassicBig.MeasureString("GAME OVER").X / 2, _graphicsDevice.Viewport.Height * 0.3f), Color.Red);
            }
        }
    }
}
