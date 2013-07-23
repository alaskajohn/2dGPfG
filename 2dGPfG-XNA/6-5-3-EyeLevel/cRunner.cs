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
    class cRunner
    {
        //Source Data
        private Texture2D m_tSrcSpriteSheet;
        private Rectangle m_rSrcLocation;
        private Vector2 m_vSrcOrigin;

        //Game Data
        public Vector2 m_vVel;
        public Vector2 m_vPos;

        //DestinationData
        public Color m_cDestColor;
        public float m_fDestRotation;
        public float m_fDestScale;        
        public float m_fDestDepth;
        public SpriteEffects m_eDestSprEff;

        //Animation Data
        private int m_iCurrentCel;
        private int m_iNumberOfCels;
        private int m_iMsUntilNextCel;
        private int m_iMsPerCel;

        public bool m_bIsRunning;

        public int m_cHorizon = 240;

        public cRunner()
        {
            m_cDestColor = Color.White;
            m_eDestSprEff = SpriteEffects.None;

            m_iNumberOfCels = 12;
            m_iCurrentCel = 0;
            m_iMsPerCel = 50;
            m_iMsUntilNextCel = m_iMsPerCel;
        }

        public void Initialize()
        {
            m_fDestRotation = 0.0f;
            m_fDestScale = 1.0f;

            m_rSrcLocation = new Rectangle(0, 0, 128, 128);
            m_vSrcOrigin = new Vector2(57, 105);
        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            m_tSrcSpriteSheet = pContent.Load<Texture2D>(fileName);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            m_iMsUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;

            m_bIsRunning = false;
            if (m_vVel.X > 5) m_bIsRunning = true;
            if (m_vVel.X < -5) m_bIsRunning = true;
            if (m_vVel.Y > 5) m_bIsRunning = true;
            if (m_vVel.Y < -5) m_bIsRunning = true;

            if ((m_iMsUntilNextCel <= 0) && (m_bIsRunning))
            {
                m_iCurrentCel++;
                m_iMsUntilNextCel = m_iMsPerCel;
            }

            if (m_iCurrentCel >= m_iNumberOfCels)
                m_iCurrentCel = 0;

            m_rSrcLocation.X = m_rSrcLocation.Width * m_iCurrentCel;
        }

        public void UpdateDepth(GameTime gameTime)
        {
            m_fDestDepth = (m_vPos.Y - m_cHorizon) / (720 - m_cHorizon);
        }

        public void UpdateScale(GameTime gameTime)
        {
            //m_fDestScale = 0.25f + (m_fDestDepth * 1.0f);
            float fEyeLevel = 70.0f;
            m_fDestScale = m_fDestDepth * ((720.0f - m_cHorizon) / fEyeLevel);
        }

        public void UpdateColor(GameTime gameTime)
        {
            float greyValue = 0.5f + (m_fDestDepth * 0.5f);
            greyValue = 1.0f;
            m_cDestColor = new Color(greyValue, greyValue, greyValue);
        }

        public void UpdatePosition(GameTime gameTime)
        {
            float MAX_VEL = 1280 / 6.0f;

            m_vVel *= 0.95f;
            m_vVel.X = MathHelper.Clamp(m_vVel.X, -MAX_VEL, +MAX_VEL);
            m_vVel.Y = MathHelper.Clamp(m_vVel.Y, -MAX_VEL, +MAX_VEL);

            m_vPos.X += (float)(m_vVel.X * gameTime.ElapsedGameTime.TotalSeconds);
            m_vPos.Y += (float)(m_fDestScale * m_vVel.Y * gameTime.ElapsedGameTime.TotalSeconds);
            m_vPos.Y = MathHelper.Clamp(m_vPos.Y, m_cHorizon, 720);
        }

        public void Update(GameTime gameTime)
        {
            UpdatePosition(gameTime);
            UpdateAnimation(gameTime);
            UpdateDepth(gameTime);
            UpdateScale(gameTime);
            UpdateColor(gameTime);
        }

        public void Draw(SpriteBatch pBatch, Vector2 pCameraLocation, Vector2 pCameraOffset, bool pParallax)
        {
            Vector2 m_vDrawPos = m_vPos;
            m_vDrawPos.X -= (pCameraLocation.X);
            if (pParallax) m_vDrawPos.X *= m_fDestScale;
            m_vDrawPos += pCameraOffset;


            pBatch.Draw(m_tSrcSpriteSheet,
                m_vDrawPos, //m_vPos,         //position
                m_rSrcLocation,          //source rectangle
                m_cDestColor,
                m_fDestRotation,         //Rotation
                m_vSrcOrigin,            //Origin
                m_fDestScale,            //scale
                m_eDestSprEff,
                m_fDestDepth);
        }
    }
}
