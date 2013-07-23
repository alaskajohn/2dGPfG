using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PixelModBlur
{
    public class FrameRateCounter
    {
        int m_iFPS = 0;
        int m_iFrameCount = 0;
        double m_iElapsedMilliseconds = 0;

        SpriteFont myFont;

        Vector2 renderLoc = new Vector2(50,50);

        public void LoadContent(ContentManager content)
        {
            myFont = content.Load<SpriteFont>("myFont");
        }

        public void Update(GameTime gameTime)
        {
            m_iElapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (m_iElapsedMilliseconds > 1000)
            {
                m_iElapsedMilliseconds -= 1000;
                m_iFPS = m_iFrameCount;
                m_iFrameCount = 0;
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            m_iFrameCount++;

            string fps = string.Format("{0} FPS", m_iFPS);

            spriteBatch.Begin();
            spriteBatch.DrawString(myFont, fps, renderLoc, Color.Red);
            spriteBatch.End();
        }
    }
}
