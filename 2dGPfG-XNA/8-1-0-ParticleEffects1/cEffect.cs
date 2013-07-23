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

namespace ParticleEffects
{
    public enum eEffectType
    {
        smoke,
        fire,
        explosion,
        snow,
        spiral
    }

    public class cEffect
    {
        
        public eEffectType m_eType;

        public Texture2D particleTexture;
        static public Texture2D snowflakeTexture;
        static public Texture2D circleTexture;
        static public Texture2D starTexture;
        
        public Vector2 m_vOrigin;
        public int m_iRadius;

        public BlendState   m_eBlendType;
        
        public int m_iEffectDuration;
        public int m_iNewParticleAmmount;
        public int m_iBurstFrequencyMS;
        public int m_iBurstCountdownMS;

        public Random myRandom;

        public List<cParticle> m_allParticles;


        public cEffect()
        {
            
            m_allParticles = new List<cParticle>();
            myRandom = new Random();
        }

        static public void LoadContent(ContentManager content)
        {
            snowflakeTexture = content.Load<Texture2D>("snowFlake");
            circleTexture = content.Load<Texture2D>("whiteCircle");
            starTexture = content.Load<Texture2D>("whiteStar");
        }

        public bool isAlive()
        {
            if (m_iEffectDuration > 0)
                return true;
            if (m_allParticles.Count() > 0)
                return true;
            return false;
        }

        public void Initialize(eEffectType pType)
        {
            m_eType = pType;

            switch (m_eType)
            {
                case eEffectType.fire:
                    FireInitialize();
                    break;
                case eEffectType.smoke:
                    SmokeInitialize();
                    break;
                case eEffectType.explosion:
                    ExplosionInitialize();
                    break;
                case eEffectType.snow:
                    SnowInitialize();
                    break;
                case eEffectType.spiral:
                    SpiralInitialize();
                    break;
            }
        }

        public void SpiralInitialize()
        {
            //Explosion
            particleTexture = starTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 1;
            m_iBurstFrequencyMS = 64;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 100);
            m_iRadius = 50;
            m_eBlendType = BlendState.NonPremultiplied;

        }
        public void SnowInitialize()
        {
            //Explosion
            particleTexture = snowflakeTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 1;
            m_iBurstFrequencyMS = 64;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, -50);
            m_iRadius = 50;
            m_eBlendType = BlendState.NonPremultiplied;

        }

        public void FireInitialize()
        {
            //Fire
            particleTexture = circleTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 15;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 400);
            m_iRadius = 15;
            m_eBlendType = BlendState.Additive;

        }
        public void SmokeInitialize()
        {
            //Smoke
            particleTexture = circleTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 4;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 640);
            m_iRadius = 50;
            m_eBlendType = BlendState.Additive;


        }
        public void ExplosionInitialize()
        {
            //Explosion
            particleTexture = starTexture;
            m_iEffectDuration = 16;
            m_iNewParticleAmmount = 800;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(200, 720);
            m_iRadius = 20;
            m_eBlendType = BlendState.NonPremultiplied;

        }



        public void createParticle()
        {
            switch (m_eType)
            {
                case eEffectType.fire:
                    createFireParticle();
                    break;
                case eEffectType.smoke:
                    createSmokeParticle();
                    break;
                case eEffectType.explosion:
                    createExplosionParticle();
                    break;
                case eEffectType.snow:
                    createSnowParticle();
                    break;
                case eEffectType.spiral:
                    createSpiralParticle();
                    break;
            }
        }


        public void createSpiralParticle()
        {
            int initAge = 3000; //3 seconds

            Vector2 initPos = m_vOrigin;


            Vector2 initVel = new Vector2(((float)(100.0f * Math.Cos(m_iEffectDuration))),
                                          ((float)(100.0f * Math.Sin(m_iEffectDuration))));

            Vector2 initAcc = new Vector2(0, 75);
            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = 2.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.2f;
            float initScaleVel = 0.2f;
            float initScaleAcc = -0.1f;
            float maxScale = 1.0f;

            Color initColor = Color.DarkRed;
            Color finalColor = Color.DarkRed;
            finalColor *= 0;
            //finalColor.A = 0;
            int fadeAge = initAge;

            cParticle tempParticle = new cParticle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createFireParticle()
        {
            int initAge = 500 + (int)myRandom.Next(500); //3 seconds
            int fadeAge = initAge - (int)myRandom.Next(100);

            Vector2 initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 offset2 = Vector2.Zero;
            offset2.X += (float)(200 * Math.Cos(m_iEffectDuration / 500.0f));
            initPos += offset2;

            Vector2 initVel = Vector2.Zero;
            initVel.X = -(offset.X);
            initVel.Y = -500;

            Vector2 initAcc = new Vector2(0, -myRandom.Next(300));

            float initDamp = 0.96f;

            float initRot = 0.0f;
            float initRotVel = 2.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.5f;
            float initScaleVel = -0.1f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            Color initColor = Color.DarkBlue;
            Color finalColor = Color.DarkOrange;
            finalColor.A = 0;


            cParticle tempParticle = new cParticle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }
        public void createSmokeParticle()
        {
            int initAge = 5000 + (int)myRandom.Next(5000);
            int fadeAge = initAge - (int)myRandom.Next(5000);

            Vector2 initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 offset2 = Vector2.Zero;
            offset2.X += (float)(400 * Math.Cos(m_iEffectDuration / 500.0f));
            initPos += offset2;

            Vector2 initVel = Vector2.Zero;
            initVel.X = 0;//
            initVel.Y = -30 - myRandom.Next(30);

            Vector2 initAcc = new Vector2(10 + myRandom.Next(10), 0);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = 0.0f;
            float initRotDamp = 1.0f;

            float initScale = 0.6f;
            float initScaleVel = ((float)myRandom.Next(10)) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 3.0f;

            Color initColor = Color.Black;
            initColor.A = 128;
            Color finalColor = new Color(32, 32, 32);
            finalColor.A = 0;


            cParticle tempParticle = new cParticle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createExplosionParticle()
        {
            int initAge = 3000 + (int)myRandom.Next(5000);
            int fadeAge = initAge / 2;

            Vector2 initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 initVel = Vector2.Zero;
            initVel.X = myRandom.Next(500) + (offset.X * 30);
            initVel.Y = -60 * Math.Abs(offset.Y);

            Vector2 initAcc = new Vector2(0, 400);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = initVel.X / 50.0f;
            float initRotDamp = 0.97f;

            float initScale = 0.1f + ((float)myRandom.Next(10)) / 50.0f;
            float initScaleVel = ((float)myRandom.Next(10) - 5) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            byte randomGray = (byte)(myRandom.Next(128) + 128);
            Color initColor = new Color(randomGray, 0, 0);

            Color finalColor = new Color(32, 32, 32);
            finalColor = Color.Black;

            cParticle tempParticle = new cParticle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createSnowParticle()
        {
            float initScale = 0.1f + ((float)myRandom.Next(10)) / 20.0f;
            float initScaleVel = 0.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            int initAge = (int)(10000/initScale);
            int fadeAge = initAge;

            Vector2 initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 offset2 = Vector2.Zero;
            offset2.X += (float)(600 * Math.Cos(m_iEffectDuration/500.0));
            initPos += offset2;


            Vector2 initVel = Vector2.Zero;
            initVel.X = myRandom.Next(10) - 5;
            initVel.Y = 100 * initScale;

            Vector2 initAcc = new Vector2(0, 0);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = initVel.X / 5.0f; ;
            float initRotDamp = 1.0f;

            Color initColor = Color.White;
            Color finalColor = Color.White;
            finalColor.A = 0;

            cParticle tempParticle = new cParticle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void Update(GameTime gameTime)
        {
            

            m_iEffectDuration -= gameTime.ElapsedGameTime.Milliseconds;
            m_iBurstCountdownMS -= gameTime.ElapsedGameTime.Milliseconds;

            if ((m_iBurstCountdownMS <= 0) && (m_iEffectDuration >= 0))
            {
                for (int i = 0; i < m_iNewParticleAmmount; i++)
                    createParticle();
                m_iBurstCountdownMS = m_iBurstFrequencyMS;
            }

            for (int i = m_allParticles.Count()-1; i>=0; i--)
            {
                m_allParticles[i].Update(gameTime);

                if (m_allParticles[i].m_iAge <= 0)
                    m_allParticles.RemoveAt(i);     
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.BackToFront, m_eBlendType);
            foreach (cParticle p in m_allParticles)
            {
                p.Draw(batch);
            }
            batch.End();

        }
    }
}
