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

namespace SinMotion
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D circleTexture;
        Texture2D whiteCircleTexture;

        Color backgroundColor = Color.Black;
        SpriteFont mySpriteFont;

        float rotation;
        double xPosition;
        double yPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 720;
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
            circleTexture = Content.Load<Texture2D>("circle");
            whiteCircleTexture = Content.Load<Texture2D>("whiteCircle");

            mySpriteFont = Content.Load<SpriteFont>("myFont");
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

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                backgroundColor.R++;

            rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (rotation < (Math.PI * -2))
                rotation = 0;
            
            xPosition = Math.Cos(rotation);
            yPosition = Math.Sin(rotation);

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

            Vector2 drawLocation = new Vector2(0, 0);
            Color drawColor = Color.White;

            double scaledX = (720 / 2) + (xPosition * 256);
            double scaledY = (720 / 2) + (yPosition * 256);
            
            string xString = String.Format("x = cos({0:000}) = {1:0.00}", (rotation / Math.PI * -180), xPosition);
            string yString = String.Format("y = sin({0:000}) = {1:0.00}", (rotation / Math.PI * -180), -yPosition);

            spriteBatch.Begin();

            spriteBatch.Draw(circleTexture, new Vector2(720 / 2, 720 / 2), null, Color.White, rotation, new Vector2(256, 256), 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(whiteCircleTexture, new Vector2(720 - 50, (float)scaledY), null, Color.White, 0.0f, new Vector2(64, 64), 0.1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(whiteCircleTexture, new Vector2((float)scaledX, 720 - 50), null, Color.White, 0.0f, new Vector2(64, 64), 0.1f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(mySpriteFont, yString, new Vector2(720 - 50 - mySpriteFont.MeasureString(yString).X, 50), Color.White);
            spriteBatch.DrawString(mySpriteFont, xString, new Vector2(100, 720 - 50 + mySpriteFont.MeasureString(xString).Y/2), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
