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

namespace SinBounce
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D whiteCircleTexture;

        float counter;

        float sinusodalY;

        float  linearY;
        float   direction = 1;
        float   speed = 2.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
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

            whiteCircleTexture = Content.Load<Texture2D>("whiteCircle");

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


            counter += ((float)gameTime.ElapsedGameTime.TotalSeconds * direction * speed);


            if (counter > (Math.PI * 0.5f))
            {
                counter = (float)(Math.PI * 0.5f);
                direction *= -1;
            }
            if (counter < -(Math.PI * 0.5f))
            {
                counter = -(float)(Math.PI * 0.5f);
                direction *= -1;
            }

           
            linearY = counter / (float)(Math.PI * 0.5f);
            sinusodalY = (float)Math.Sin(counter);
            linearY = -Math.Abs(linearY);
            sinusodalY = -Math.Abs(sinusodalY);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            float scaledSinY = (sinusodalY * 256) + 300;
            float scaledLin2 = (linearY * 256) + 300;
            
            spriteBatch.Begin();

            spriteBatch.Draw(whiteCircleTexture, new Vector2(360, (float)scaledSinY), Color.White);
            spriteBatch.Draw(whiteCircleTexture, new Vector2(790, (float)scaledLin2), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
