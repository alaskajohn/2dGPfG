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

namespace cameraSample1
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public bool parallax = true;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        cRunner runner, runner2;
        cSnowman[] snowmenArray;

        Texture2D background;

        const int HORIZON = 240;
        const int SNOWMEN_COUNT = 50;
        Random myRandom;
        Vector2 cameraLocation;
        Vector2 cameraOffset = new Vector2(1280 / 2, 0);

        public Game1()
        {
            myRandom = new Random();

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

            runner = new cRunner();
            runner2 = new cRunner();

            snowmenArray = new cSnowman[SNOWMEN_COUNT];

            for (int i = 0; i < SNOWMEN_COUNT; i++)
                snowmenArray[i] = new cSnowman();
        }


        protected override void Initialize()
        {
            runner.Initialize();
            runner.m_vPos = new Vector2(400, 400);

            runner2.Initialize();
            runner2.m_vPos = new Vector2(600, 400);



            //for (int i = 0; i < SNOWMEN_COUNT; i++)
            //{
            //    snowmenArray[i].Initialize();
            //    snowmenArray[i].m_vPos = new Vector2(myRandom.Next(1280 - 200) + 100, myRandom.Next(720 - 240) + 240);
            //}

            int x = 100;
            int y = 250;
            for (int i = 0; i < SNOWMEN_COUNT; i++)
            {
                snowmenArray[i].Initialize();
                snowmenArray[i].m_vPos = new Vector2(x, y);
                x += 400;
                if (x > 1280)
                {
                    x = 100;
                    y += 50;
                }
            }

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");

            runner.LoadContent(Content, "run_cycle");
            runner2.LoadContent(Content, "run_cycle");
            for (int i = 0; i < SNOWMEN_COUNT; i++)
            {
                snowmenArray[i].LoadContent(Content, "snow_assets");
            }
        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                runner.m_vVel.Y -= 10f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                runner.m_vVel.Y += 10f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                runner.m_eDestSprEff = SpriteEffects.FlipHorizontally;
                runner.m_vVel.X -= 10f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                runner.m_eDestSprEff = SpriteEffects.None;
                runner.m_vVel.X += 10f;
            }

            runner.Update(gameTime);
            runner2.Update(gameTime);

            for (int i = 0; i < SNOWMEN_COUNT; i++)
            {
                snowmenArray[i].Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                parallax = !parallax;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //cameraLocation.X -= (cameraOffset.X);
            GraphicsDevice.Clear(Color.White);

            cameraLocation = new Vector2(runner.m_vPos.X, 0.0f);


            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
                spriteBatch.Draw(background, Vector2.Zero, Color.White);
                runner.Draw(spriteBatch, cameraLocation, cameraOffset, parallax);
                runner2.Draw(spriteBatch, cameraLocation, cameraOffset, parallax);
                for (int i = 0; i < SNOWMEN_COUNT; i++)
                {
                    snowmenArray[i].Draw(spriteBatch, cameraLocation, cameraOffset, parallax);
                }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
