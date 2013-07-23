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

namespace Foreground
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
        private Texture2D snowAssetsTexture;
        private Rectangle currentCelLocation;

        private Texture2D blockTexture;

        private Texture2D snowBGTexture;

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
            snowBGTexture = Content.Load<Texture2D>("snow_bg");
            blockTexture = Content.Load<Texture2D>("blocks");
            snowAssetsTexture = Content.Load<Texture2D>("blurred_snowman");
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
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(snowBGTexture, Vector2.Zero, Color.White);
            
            for (int i=0; i<10; i++)
                spriteBatch.Draw(blockTexture,
                                new Rectangle(i * blockTexture.Width, (int)runnerPosition.Y + 106, 128, 64),
                                new Rectangle(0, 0, 128, 64),
                                Color.White);
            for (int i = 0; i < 10; i++)
                spriteBatch.Draw(blockTexture,
                                new Rectangle(i * blockTexture.Width - 64, (int)runnerPosition.Y + 106 + 64, 128, 64),
                                new Rectangle(0, 0, 128, 64),
                                Color.White);


            spriteBatch.Draw(runCycleTexture,
                runnerPosition,
                currentCelLocation,
                Color.White,
                0.0f,           //Rotation
                Vector2.Zero,   //Origin
                1.0f,           //scale
                eRunnerSprEff,
                1.0f);

            for (int i = 0; i < 10; i++)
                spriteBatch.Draw(blockTexture,
                                new Rectangle((int)(i * blockTexture.Width), (int)runnerPosition.Y-64, 64, 64),
                                new Rectangle(0, 64, 64, 64),
                                Color.White);
            spriteBatch.Draw(snowAssetsTexture,
                            new Rectangle(-280, 0, 512, 512),
                            new Rectangle(0, 0, 512, 512),
                            Color.White);

            spriteBatch.Draw(snowAssetsTexture,
                            new Rectangle(530, 00, 512, 512),
                            new Rectangle(0, 0 , 512, 512),
                            Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
