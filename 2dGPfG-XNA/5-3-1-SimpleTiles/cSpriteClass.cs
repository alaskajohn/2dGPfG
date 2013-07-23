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

namespace simpleTiles
{
    class cSpriteClass
    {
        private Texture2D mTexture;
        private Rectangle mLocation;
        private Vector2 mOrigin;
        public Color mColor;

        public cSpriteClass()
        {
            mColor = Color.White;

        }

        public void LoadContent(ContentManager pContent, String fileName, Rectangle pLocation, Vector2 pOrigin)
        {
            mTexture = pContent.Load<Texture2D>(fileName);
            mLocation = pLocation;
            mOrigin = pOrigin;
        }

        public void Draw(SpriteBatch pBatch, Vector2 pGameLocation)
        {
            Draw(pBatch, pGameLocation, 0.0f, 1.0f);
        }


        public void Draw(SpriteBatch pBatch, Vector2 pGameLocation, float pRotation, float pScale)
        {

            pBatch.Draw(mTexture,
                pGameLocation,          //position
                mLocation,              //source rectangle
                mColor,
                pRotation,                   //Rotation
                mOrigin,                //Origin
                pScale,                   //scale
                SpriteEffects.None,
                0.0f);
        }
    }
}
