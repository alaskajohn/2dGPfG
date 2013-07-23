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
    class cSnowman
    {
        //Source Data
        private Texture2D m_tSrcSpriteSheet;
        private Rectangle m_rSrcLocation;
        private Vector2 m_vSrcOrigin;

        //DestinationData
        public Vector2 m_vPos;
        public Color m_cDestColor;
        public float m_fDestRotation;
        public float m_fDestScale;
        public float m_fDestDepth;
        public SpriteEffects m_eDestSprEff;

        public Vector2 m_vDrawPos;

        public int m_cHorizon = 240;

        public cSnowman()
        {
            m_cDestColor = Color.White;
            m_eDestSprEff = SpriteEffects.None;

       }

        public void Initialize()
        {
            m_fDestRotation = 0.0f;
            m_fDestScale = 1.0f;

            m_rSrcLocation = new Rectangle(0, 128, 256, 256);
            m_vSrcOrigin = new Vector2(122, 194);
        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            m_tSrcSpriteSheet = pContent.Load<Texture2D>(fileName);
        }


        public void UpdateDepth(GameTime gameTime)
        {
            m_fDestDepth = (m_vPos.Y - m_cHorizon) / (720-m_cHorizon);
        }

        public void UpdateScale(GameTime gameTime)
        {
            //m_fDestScale = 0.25f + (m_fDestDepth * 1.0f);
            //m_fDestScale = (m_fDestDepth * 3.56f);
            float fEyeLevel = 70.0f;		//runner
            //float fEyeLevel = 135.0f;		//snowman
            m_fDestScale = m_fDestDepth * ((720.0f - m_cHorizon) / fEyeLevel);
        }

        public void UpdateColor(GameTime gameTime)
        {
            float greyValue = 0.75f + (m_fDestDepth * 0.25f);

            m_cDestColor = new Color(greyValue, greyValue, greyValue);
        }

        public void Update(GameTime gameTime)
        {
            UpdateDepth(gameTime);
            UpdateScale(gameTime);
            UpdateColor(gameTime);          
        }

        public void Draw(SpriteBatch pBatch, Vector2 pCameraLocation, Vector2 pCameraOffset, bool pParallax)
        {
            m_vDrawPos = m_vPos;
            m_vDrawPos.X -= pCameraLocation.X;
            if (pParallax) m_vDrawPos.X *= m_fDestScale;
            m_vDrawPos += pCameraOffset;

            pBatch.Draw(m_tSrcSpriteSheet,
                m_vDrawPos ,//m_vPos,         //position
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
