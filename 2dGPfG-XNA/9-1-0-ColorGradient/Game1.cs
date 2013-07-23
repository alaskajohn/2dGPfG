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

namespace ColorGradient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pointTexture;
        Color backgroundColor = Color.Black;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 512;
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
            Color[] arrayOfColor = { Color.White };
            Rectangle pointRectangle = new Rectangle(0, 0, 1, 1);

            pointTexture = new Texture2D(GraphicsDevice, 1, 1);
            pointTexture.SetData<Color>(0, pointRectangle, arrayOfColor, 0, 1);

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
            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //    backgroundColor.R--;

            // TODO: Add your update logic here

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

            spriteBatch.Begin();
            for (int i=0; i<2; i++)
            {
                drawLocation = Vector2.Zero;
                drawColor = Color.White;

                for (int R = 0; R < 512; R+=1)
                {
                    if (R < 255)
                    {
                        drawColor.R = (byte)R;
                        drawColor.B = (byte)255;
                        drawLocation.X++;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            drawColor.R = (byte)255;
                            drawColor.B = (byte)(255 - R);
                            drawLocation.X++;
                        }
                    }
                    

                    for (int G = 0; G < 256; G+=1)
                    {
                        if (i == 0)
                        {
                            drawColor.G = (byte)G;
                            //drawColor.B = 255;
                            drawLocation.Y++;
                            if (drawLocation.Y > 255)
                                drawLocation.Y = 0;
                        }
                        else if (i == 1)
                        {
                            drawColor.B = (byte)(255-G);
                            //drawColor.G = 255;
                            drawLocation.Y++;
                            if (drawLocation.Y > 511)
                                drawLocation.Y = 256;
                        }
                        spriteBatch.Draw(pointTexture, drawLocation, drawColor);
                    }
                }
            }           

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
