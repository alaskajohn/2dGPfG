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
    public class cEffectManager
    {
        public List<cEffect> m_lAllEffects;

        public cEffectManager()
        {
            m_lAllEffects = new List<cEffect>();
        }

        public void LoadContent(ContentManager Content)
        {
            cEffect.LoadContent(Content);
        }

        public void AddEffect(eEffectType type)
        {
            cEffect tempEffect = new cEffect();
            tempEffect.Initialize(type);
            m_lAllEffects.Add(tempEffect);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = m_lAllEffects.Count() - 1; i >= 0; i--)
            {
                m_lAllEffects[i].Update(gameTime);

                if (!m_lAllEffects[i].isAlive())
                    m_lAllEffects.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (cEffect e in m_lAllEffects)
            {
                e.Draw(batch);
            }
        }

    }
}
