using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    public static class EmitterManager
    {
        #region Fields
        private static List<Emitter> m_List = new List<Emitter>();

        private static Texture2D m_Sprite = null;        
        #endregion

        #region Properties
        public static List<Emitter> List
        {
            get { return m_List; }
            set { m_List = value; }
        }

        public static Texture2D Sprite
        {
            get { return m_Sprite; }
            set { m_Sprite = value; }
        }
        #endregion

        #region Methods
        public static void Update(GameTime a_GameTime)
        {
            for (int loop = 0; loop < m_List.Count; loop++)
            {
                m_List[loop].Update(a_GameTime);
            }
        }

        public static void Draw(SpriteBatch a_SpriteBatch)
        {
            for (int loop = 0; loop < m_List.Count; loop++)
            {
                m_List[loop].Draw(a_SpriteBatch);
            }
        }
        #endregion
    }
}
