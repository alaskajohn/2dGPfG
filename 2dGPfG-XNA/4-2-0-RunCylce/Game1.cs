using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace runCycle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Source Data
        private Texture2D runCycleTexture;
        private Rectangle currentCelLocation;

        //Destination Data
        private Vector2 runnerPosition;

        //Animation Data
        private int currentCel;
        private int numberOfCels;

        private int msUntilNextCel;		//in miliseconds
        private int msPerCel;			//in miliseconds

        private bool bIsRunning;
        private SpriteEffects eRunnerSprEff;

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
            // TODO: Add your initialization logic here
            numberOfCels = 12;
            currentCel = 0;

            msPerCel = 50;
            msUntilNextCel = msPerCel;

            currentCelLocation.X = 0;
            currentCelLocation.Y = 0;
            currentCelLocation.Width = 128;		//sprite width
            currentCelLocation.Height = 128;	//sprite height

            runnerPosition = new Vector2(100, 100);

            eRunnerSprEff = SpriteEffects.None;
            
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

            runCycleTexture = Content.Load<Texture2D>("run_cycle");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            msUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;

            bIsRunning = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                bIsRunning = true;
                runnerPosition.X--;
                eRunnerSprEff = SpriteEffects.FlipHorizontally;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                bIsRunning = true;
                runnerPosition.X++;
                eRunnerSprEff = SpriteEffects.None;
            }


            if ((msUntilNextCel <= 0) && (bIsRunning)) 
            {
                currentCel++;
                msUntilNextCel = msPerCel;
            }

            if (currentCel >= numberOfCels)
                currentCel = 0;

            currentCelLocation.X = currentCelLocation.Width * currentCel;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(runCycleTexture,
                            runnerPosition,
                            currentCelLocation,
                            Color.White,
                            0.0f,           //Rotation
                            Vector2.Zero,   //Origin
                            1.0f,           //scale
                            eRunnerSprEff,
                            1.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
