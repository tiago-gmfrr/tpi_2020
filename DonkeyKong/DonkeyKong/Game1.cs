using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DonkeyKong.States;

namespace DonkeyKong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Variables 

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //Variables to use for the changing of states between gameState and MenuState
        private State _currentState;
        private State _nextState;
        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            IsMouseVisible = true;
            graphics.GraphicsProfile = GraphicsProfile.Reach;

            graphics.PreferredBackBufferWidth = 1280;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = 720;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new HomeMenu(this, graphics.GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);
            //_currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            _currentState.Draw(gameTime, spriteBatch);


            spriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// Method to change in which state we are in
        /// </summary>
        /// <param name="state">Game or Menu state</param>
        public void ChangeState(State state)
        {
            _nextState = state;
        }
    }
}
