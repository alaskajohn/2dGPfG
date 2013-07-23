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

namespace simpleTiles
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Source Data
        cSpriteClass plains, forest, mountains, hills, water, player;

        //Destination Data
        private Vector2 playerMapPosition, playerScreenPosition;
        private Vector2 cameraPosition;
        private Vector2 cameraOffset;

        private double playerForwardVelocity;
        private double playerRotation;
        private double maxPlayerVelocity;

        //Game Constancts
        private const int SCREEN_W = 1280;
        private const int SCREEN_H = 720;

        private const int SPRITE_W = 32;
        private const int SPRITE_H = 32;

        private const int MAP_W = 256;
        private const int MAP_H = 256;

        //Game Map
        Texture2D mapTexture;
        private Color[] gameMap = new Color[MAP_W * MAP_H];

        private float fZoomLevel;
        private float fGoalZoomLevel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREEN_W;
            graphics.PreferredBackBufferHeight = SCREEN_H;
            //graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            plains = new cSpriteClass();
            forest = new cSpriteClass();
            mountains = new cSpriteClass();
            hills = new cSpriteClass();
            water = new cSpriteClass();
            player = new cSpriteClass();
        }

        protected override void Initialize()
        {
            playerMapPosition = new Vector2(MAP_W/2, MAP_H/2);
            cameraOffset = new Vector2(SCREEN_W/2.0f, SCREEN_H/2.0f);        

            fZoomLevel = 1.0f;

            this.IsFixedTimeStep = false;
            //this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, (int)Math.Round(1000.0f / 60.0f));

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


            // TODO: use this.Content to load your game content here
            plains.LoadContent(Content, "tiledSprites2", new Rectangle(0, 0, SPRITE_W, SPRITE_H), new Vector2(SPRITE_W / 2, SPRITE_H / 2));
            forest.LoadContent(Content, "tiledSprites2", new Rectangle(SPRITE_W, 0, SPRITE_W, SPRITE_H), new Vector2(SPRITE_W / 2, SPRITE_H / 2));
            hills.LoadContent(Content, "tiledSprites2", new Rectangle(SPRITE_W*2, 0, SPRITE_W, SPRITE_H), new Vector2(SPRITE_W / 2, SPRITE_H / 2));
            mountains.LoadContent(Content, "tiledSprites2", new Rectangle(0, SPRITE_H, SPRITE_W, SPRITE_H), new Vector2(SPRITE_W / 2, SPRITE_H / 2));
            water.LoadContent(Content, "tiledSprites2", new Rectangle(SPRITE_W, SPRITE_H, SPRITE_W, SPRITE_H), new Vector2(SPRITE_W / 2, SPRITE_H / 2));

            player.LoadContent(Content, "tiledSprites2", new Rectangle(SPRITE_W*2, SPRITE_H, SPRITE_W, SPRITE_H), new Vector2(SPRITE_W / 2, (SPRITE_H / 2)+2));

            mapTexture = Content.Load<Texture2D>("map01");
            mapTexture.GetData<Color>(gameMap);
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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            float playerRotationRate = 1.0f;

            //Player Rotation
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                playerRotation -= (playerRotationRate * gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                playerRotation += (playerRotationRate * gameTime.ElapsedGameTime.TotalSeconds);
            }

            //Player Acceleration
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (playerForwardVelocity <= maxPlayerVelocity)
                    playerForwardVelocity += 0.5f;

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (playerForwardVelocity >= (maxPlayerVelocity * -0.5f))
                    playerForwardVelocity -= 0.5f;
            }
            
            //playerForwardVelocity *= 0.95f;

            playerMapPosition.X += (float)(playerForwardVelocity * Math.Cos(playerRotation) * gameTime.ElapsedGameTime.TotalSeconds);
            playerMapPosition.Y += (float)(playerForwardVelocity * Math.Sin(playerRotation) * gameTime.ElapsedGameTime.TotalSeconds);

            if (playerMapPosition.X < 0)
                playerMapPosition.X = 0;
            if (playerMapPosition.X >= MAP_H - 2)
                playerMapPosition.X = MAP_H - 2;
            if (playerMapPosition.Y < 0)
                playerMapPosition.Y = 0;
            if (playerMapPosition.Y >= MAP_H - 2)
                playerMapPosition.Y = MAP_H - 2;

            playerScreenPosition.X = playerMapPosition.X * SPRITE_W;
            playerScreenPosition.Y = playerMapPosition.Y * SPRITE_H;



            int arrayLoc = (int)playerMapPosition.X + ((int)playerMapPosition.Y * 256);

            fGoalZoomLevel = 1.1f;
            player.mColor.A = 255;
            maxPlayerVelocity = 3.0f;

            if (gameMap[arrayLoc].R > 0)
            {
                fGoalZoomLevel = 1.0f;
                maxPlayerVelocity *= 0.35f;
            }

            if (gameMap[arrayLoc].R > 128)
            {
                fGoalZoomLevel = 0.9f;
                maxPlayerVelocity *= 0.15f;
            }

            if (gameMap[arrayLoc].B > 128)
            {
                fGoalZoomLevel = 1.4f;
                player.mColor.A = 200;
                maxPlayerVelocity *= 0.5f;
            }

            if (gameMap[arrayLoc].G > 128)
            {
                gameMap[arrayLoc].G = 0;
                maxPlayerVelocity *= 0.5f;
            }

            if (fZoomLevel < fGoalZoomLevel)
                fZoomLevel += 0.01f;
            if (fZoomLevel > fGoalZoomLevel)
                fZoomLevel -= 0.01f;

            cameraPosition = playerScreenPosition;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Vector2 drawLocation = cameraPosition - (cameraOffset/fZoomLevel);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                  BlendState.NonPremultiplied,
                  SamplerState.PointClamp,
                  DepthStencilState.Default,
                  RasterizerState.CullNone,
                  null,
                  Matrix.CreateScale(fZoomLevel));

            int xOffset = 23;
            int yOffset = 13;
            
            int iStart = (int)(playerMapPosition.X - xOffset);
            if (iStart < 0) iStart = 0;

            int iEnd = (int)(playerMapPosition.X + xOffset);
            if (iEnd >= MAP_W) iEnd = MAP_W - 1;

            int jStart = (int)(playerMapPosition.Y - yOffset);
            if (jStart < 0) jStart = 0;

            int jEnd = (int)(playerMapPosition.Y + yOffset);
            if (jEnd >= MAP_H) jEnd = MAP_H - 1;

            Vector2 screenLocation;
            Color mapLocation;

            for (int i = iStart; i < iEnd; i++)
                for (int j = jStart; j < jEnd; j++)
                {
                    screenLocation = new Vector2(i * SPRITE_W, j * SPRITE_H);

                    mapLocation = gameMap[i + (j * MAP_H)];

                    if (mapLocation.R > 128)
                        mountains.Draw(spriteBatch, screenLocation - drawLocation);
                    else if (mapLocation.R > 0)
                        hills.Draw(spriteBatch, screenLocation - drawLocation);
                    else if (mapLocation.G > 128)
                        forest.Draw(spriteBatch, screenLocation - drawLocation);
                    else if (mapLocation.B > 128)
                        water.Draw(spriteBatch, screenLocation - drawLocation);
                    else
                        plains.Draw(spriteBatch, screenLocation - drawLocation);

                }
            player.Draw(spriteBatch, playerScreenPosition - drawLocation, (float)playerRotation, 1.0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
