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

namespace PixelModCPU
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont myFont;
        Texture2D pixelsTexture1, pixelsTexture2, spriteSheet;
        Color backgroundColor = Color.Black;
        FrameRateCounter g_frameRate = new FrameRateCounter();
        int type = 2;
        //if 0, create gradient
        //if 1, invert snowman
        Vector2 pos = Vector2.Zero;
        Vector2 vel = new Vector2(1.0f, 1.5f);

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


            base.Initialize();
        }

        public void createGradient()
        {
                        
            int width = 256;
            int height = 256;


            //Create 2D array of colors
            Color[] arrayOfColor = new Color[width * height];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    arrayOfColor[j + (width * i)] = new Color(i, 0, j);
                }

            //Place color array into a texture
            pixelsTexture1 = new Texture2D(GraphicsDevice, width, height);
            pixelsTexture1.SetData<Color>(arrayOfColor);
        }

        public void invertSnowman()
        {
            
            int width = 256;
            int height = 256;
            

            //Get 2D array of colors from sprite sheet
            Color[] arrayOfColor = new Color[width * height];
            spriteSheet.GetData<Color>(0, new Rectangle(0,128, 256, 256), arrayOfColor, 0, (width * height));

            //Place color array into a texture
            pixelsTexture1 = new Texture2D(GraphicsDevice, width, height);
            pixelsTexture1.SetData<Color>(arrayOfColor);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int currentElement = j + (width * i);
                    arrayOfColor[currentElement].R = (byte)(255 - arrayOfColor[currentElement].R);
                    arrayOfColor[currentElement].G = (byte)(255 - arrayOfColor[currentElement].G);
                    arrayOfColor[currentElement].B = (byte)(255 - arrayOfColor[currentElement].B);
                }

            //Place color array into a texture
            pixelsTexture2 = new Texture2D(GraphicsDevice, width, height);
            pixelsTexture2.SetData<Color>(arrayOfColor);
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            myFont = Content.Load<SpriteFont>("myFont");
            spriteSheet = Content.Load<Texture2D>("snow_assets");

            if (type == 0)
                createGradient();
            if (type == 1)
                invertSnowman();
            if (type == 2)
                invertSnowman();

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
            g_frameRate.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (type == 2)
                modifyTexture2(gameTime);

            base.Update(gameTime);
        }

        public void updatePosition()
        {
            pos += vel;

            if ((pos.X < 0) || (pos.X > 255))
                vel.X *= -1f;
            MathHelper.Clamp(pos.X, 0, 255);

            if ((pos.Y < 0) || (pos.Y > 255))
                vel.Y *= -1f;
            MathHelper.Clamp(pos.Y, 0, 255);
        }


        public void modifyTexture2(GameTime gameTime)
        {
            updatePosition();


            int width = 256;
            int height = 256;

            //Get 2D array of colors from texture2
            Color[] arrayOfColor = new Color[width * height];
            pixelsTexture1.GetData<Color>(arrayOfColor);

            //Modify color array into a texture
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int currentElement = j + (width * i);
                    double distance = Math.Sqrt(Math.Pow(i-pos.X, 2) + Math.Pow(j-pos.Y, 2));
                    double radius = 50;// (Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 500.0f) * 50) + 50;
                    if (distance < radius)
                    {
                        arrayOfColor[currentElement].R = (byte)(255 - arrayOfColor[currentElement].R);
                        arrayOfColor[currentElement].G = (byte)(255 - arrayOfColor[currentElement].G);
                        arrayOfColor[currentElement].B = (byte)(255 - arrayOfColor[currentElement].B);
                        arrayOfColor[currentElement].A = 255;
                    }
                }


            //Place color array into a texture
            pixelsTexture2.SetData<Color>(arrayOfColor);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
 
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            if (type == 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(pixelsTexture1, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            if ((type == 1) || (type == 2))
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                spriteBatch.Draw(pixelsTexture1, Vector2.Zero, Color.White);
                spriteBatch.Draw(pixelsTexture2, new Vector2(256,0), Color.White);
                spriteBatch.End();
            }
            if ((type == 3))
            {

                RenderTarget2D tempRenderTarget = new RenderTarget2D(GraphicsDevice, 512,512);
                RenderTargetBinding[] tempBinding = GraphicsDevice.GetRenderTargets();
                GraphicsDevice.SetRenderTarget(tempRenderTarget);

                // Render a simple scene.
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                for (int i = 0; i<2; i++)
                    for (int j = 0; j< 2; j++)
                    spriteBatch.Draw(spriteSheet, new Vector2(i*256, j*256), new Rectangle(0, 128, 256, 256), Color.White);
                spriteBatch.End();

                GraphicsDevice.SetRenderTargets(tempBinding);

                //Get 2D array of colors from sprite sheet
                int scr_width = 512;
                int scr_height = 512;
                Color[] arrayOfColor = new Color[scr_width * scr_height];

                tempRenderTarget.GetData<Color>(arrayOfColor);

                for (int i = 0; i < scr_width; i++)
                    for (int j = 0; j < scr_height; j++)
                    {
                        int currentElement = j + (scr_width * i);
                        arrayOfColor[currentElement].R = (byte)(255 - arrayOfColor[currentElement].R);
                        arrayOfColor[currentElement].G = (byte)(255 - arrayOfColor[currentElement].G);
                        arrayOfColor[currentElement].B = (byte)(255 - arrayOfColor[currentElement].B);
                    }

                //Place color array into a texture
                Texture2D newTexture = new Texture2D(GraphicsDevice, 512, 512) ;
                newTexture.SetData<Color>(arrayOfColor);

                spriteBatch.Begin();
                spriteBatch.Draw(newTexture, Vector2.Zero, Color.White);
                spriteBatch.End();
            }




            //g_frameRate.Draw(gameTime, spriteBatch, myFont);

            base.Draw(gameTime);
        }
    }
}
