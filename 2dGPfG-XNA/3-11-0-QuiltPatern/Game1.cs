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

namespace QuiltPattern
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game2 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Sprite Source Data
        Texture2D patternSprite;
        Rectangle patternLocation   = new Rectangle(0,0,32,32);
        Vector2 patternOrigin       = new Vector2 (16,16);

        //Sprite Destination Data
        Vector2 drawLocation = new Vector2(16,16);
        Color drawColor     = Color.NavajoWhite;
        float drawScale     = 1.0f;
        float drawRotation  = 0.0f;
        float offsetRotation = 0.0f;

        int keyboardTimer = 0;

        public Game2()
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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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

            patternSprite = Content.Load<Texture2D>("quilt_piece");

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

            keyboardTimer--;
            if (keyboardTimer <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    offsetRotation += (float)(Math.PI * 0.5);
                    keyboardTimer = 16;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    offsetRotation -= (float)(Math.PI * 0.5);
                    keyboardTimer = 16;
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Vector2 drawPosition;
            drawRotation = 0.0f + offsetRotation;
            for (int x = 16; x < 1280; x += 32)
            {
                drawPosition.X = x;
                for (int y = 16; y < 720; y += 32)
                {
                    drawPosition.Y = y;

                    if (((y - 16) % 256) == 0)
                    {
                        drawRotation += (float)(Math.PI * 0.5);
                    }
                    
                    if ((((y - 16) % 128) == 0) || (((y - 16) % 128) == 32))
                    {
                        drawColor = Color.Navy;
                       // drawRotation += (float)(Math.PI * 0.5);
                    }
                    else
                    {
                        drawColor = Color.Orchid;
                        //drawRotation += (float)(Math.PI * 0.5);
                    }

                    if (((x - 16) % 32) == 0)
                    {
                        drawRotation += (float)(Math.PI * 0.5);
                    }
                    else
                    {
                        drawRotation += (float)(Math.PI * 1.0f);
                    }

                    if ((((x - 16) % 128) == 0) || (((x - 16) % 128) == 32))
                    {
                        drawColor = Color.Multiply(drawColor, 0.75f);
                        //drawRotation += (float)(Math.PI * 0.5);
                    }
                    else
                    {
                        drawColor = Color.Multiply(drawColor, 0.85f);
                        //drawRotation += (float)(Math.PI * 0.5);
                    }

                    spriteBatch.Draw(patternSprite,
                                drawPosition,
                                patternLocation,
                                drawColor,
                                drawRotation,
                                patternOrigin,
                                drawScale,
                                SpriteEffects.None,     //Flip sprite?
                                1.0f);                  //Depth
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
