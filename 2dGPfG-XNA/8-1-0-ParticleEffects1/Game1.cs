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

namespace ParticleEffects
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        cEffectManager myEffectsManager;
        int keyboardDelayCounter = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            myEffectsManager = new cEffectManager();
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

            myEffectsManager.LoadContent(Content);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (keyboardDelayCounter > 0)
            {
                keyboardDelayCounter -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    myEffectsManager.AddEffect(eEffectType.explosion);
                    keyboardDelayCounter = 300;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    myEffectsManager.AddEffect(eEffectType.fire);
                    keyboardDelayCounter = 300;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    myEffectsManager.AddEffect(eEffectType.snow);
                    keyboardDelayCounter = 300;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    myEffectsManager.AddEffect(eEffectType.smoke);
                    keyboardDelayCounter = 300;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    myEffectsManager.AddEffect(eEffectType.spiral);
                    keyboardDelayCounter = 300;
                }
            }

            myEffectsManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color clearColor = Color.Black;
            //clearColor.A = 0;
            GraphicsDevice.Clear(clearColor);

            myEffectsManager.Draw(spriteBatch);

            
            base.Draw(gameTime);
        }
    }
}
