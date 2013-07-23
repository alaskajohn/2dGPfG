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

namespace cameraWithZoom
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
        private Vector2   runnerCelOrigin;

        private Texture2D snowmanTexture;
        private Rectangle snowmanCelLocation;
        private Vector2   snowmanCelOrigin;

        //Destination Data
        private Vector2 runnerPosition;
        private Vector2 cameraPosition;
        private Vector2 cameraOffset;
        private Vector2[] snowmenPositions = new Vector2[10];

        //Animation Data
        private int currentCel;
        private int numberOfCels;

        private int msUntilNextCel;		//in miliseconds
        private int msPerCel;			//in miliseconds

        private bool bIsRunning;
        private SpriteEffects eRunnerSprEff;

        private float fZoomLevel;

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

            currentCelLocation = new Rectangle(0, 0, 128, 128);
            runnerCelOrigin = new Vector2(64, 64);

            snowmanCelLocation = new Rectangle(0, 128, 256, 256);
            snowmanCelOrigin = new Vector2(128, 128);

            runnerPosition = new Vector2(100, 100);
            cameraOffset = new Vector2(400, 240);
            cameraPosition = runnerPosition;

            for (int i = 0; i < 10; i++)
                snowmenPositions[i] = new Vector2(200 * i, 200);

            eRunnerSprEff = SpriteEffects.None;

            fZoomLevel = 1.0f;

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
            snowmanTexture = Content.Load<Texture2D>("snow_assets");
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

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                fZoomLevel += 0.01f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                fZoomLevel -= 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                bIsRunning = true;
                runnerPosition.X -= 20; ;
                eRunnerSprEff = SpriteEffects.FlipHorizontally;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                bIsRunning = true;
                runnerPosition.X += 20;
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

            //cameraPosition = runnerPosition - cameraOffset;
            //Vector2 goalCameraPosition = runnerPosition - cameraOffset;

            const float MULTIPLIER = 0.05f;

            if (cameraPosition.X < runnerPosition.X)
            {
                cameraPosition.X -= ((cameraPosition.X - runnerPosition.X) * MULTIPLIER);
            }
            else if (cameraPosition.X > runnerPosition.X)
            {
                cameraPosition.X += ((cameraPosition.X - runnerPosition.X) * -MULTIPLIER);
            }

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
            Vector2 drawLocation = cameraPosition - (cameraOffset/fZoomLevel);

            
   //         Matrix scaleMatrix = Matrix.CreateScale(fZoomLevel);
            spriteBatch.Begin(SpriteSortMode.Deferred, 
                              BlendState.NonPremultiplied, 
                              SamplerState.AnisotropicClamp, 
                              DepthStencilState.Default, 
                              RasterizerState.CullNone, 
                              null, 
                              Matrix.CreateScale(fZoomLevel));

//            spriteBatch.Begin();
            spriteBatch.Draw(runCycleTexture,
                            runnerPosition - drawLocation,
                            currentCelLocation,
                            Color.White,
                            0.0f,           //Rotation
                            runnerCelOrigin,   //Origin
                            1.0f,           //scale
                            eRunnerSprEff,
                            0.0f);
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.Draw(snowmanTexture,
                    snowmenPositions[i] - drawLocation,
                    snowmanCelLocation,
                    Color.White,
                    0.0f,           //Rotation
                    snowmanCelOrigin,   //Origin
                    1.0f,           //scale
                    SpriteEffects.None,
                    0.0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
