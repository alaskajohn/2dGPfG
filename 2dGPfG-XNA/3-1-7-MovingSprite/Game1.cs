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

namespace movingSprite
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Sprite Source Data
        Texture2D snowAssetTexture;
        Rectangle snowmanSourceLocation;
        Vector2 snowmanSourceOrigin;

        //Sprite Destination Data
        Vector2 firstSnowmanLocation;
        
        Vector2 secondSnowmanLocation;
        Color secondSnowmanColor;
        float secondSnowmanScale;
        float secondSnowmanRotation;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            firstSnowmanLocation = new Vector2(200,500);

            secondSnowmanLocation = new Vector2(400, 500);
            secondSnowmanRotation = 0.0f;
            secondSnowmanColor = Color.Plum;
            secondSnowmanScale = 0.5f;


            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            Color[] arrayOfColor = { Color.White };
            Rectangle pointRectangle = new Rectangle(0, 0, 1, 1);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            snowAssetTexture = Content.Load<Texture2D>("snow_assets");
            snowmanSourceLocation = new Rectangle(0, 128, 256, 256);
            snowmanSourceOrigin = new Vector2(128, 192);
            
            // TODO: use this.Content to load your game content here
            
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                secondSnowmanLocation.X--;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                secondSnowmanLocation.X++;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                secondSnowmanLocation.Y--;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                secondSnowmanLocation.Y++;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(snowAssetTexture,
                        firstSnowmanLocation,
                        snowmanSourceLocation,
                        Color.White,            // Color
                        0.0f,                   // Rotation
                        snowmanSourceOrigin,
                        1.0f,                   // Scale
                        SpriteEffects.None,     // Flip sprite?
                        firstSnowmanLocation.Y / 720.0f);                  // Depth
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(snowAssetTexture, 
                        secondSnowmanLocation, 
                        snowmanSourceLocation,
                        secondSnowmanColor, 
                        secondSnowmanRotation,
                        snowmanSourceOrigin, 
                        secondSnowmanScale, 
                        SpriteEffects.None,     //Flip sprite?
                        secondSnowmanLocation.Y / 720.0f);                  //Depth
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
