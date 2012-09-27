using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Snake
{
    public class WorldArena
    {
        #region Fields
        private Vector2 m_MinPoint = Vector2.Zero;
        private Vector2 m_MaxPoint = Vector2.Zero;
        private Vector2 m_Origin = Vector2.Zero;


        private Texture2D m_GrassTexture = null;
        #endregion

        #region Properties
        public Vector2 MinPoint
        {
            get { return m_MinPoint; }
            set
            {
                m_MinPoint = value;
                m_Origin = new Vector2((m_MinPoint.X + m_MaxPoint.X) / 2.0f, (m_MinPoint.Y + m_MaxPoint.Y) / 2.0f);
            }
        }
        public Vector2 MaxPoint
        {
            get { return m_MaxPoint; }
            set
            {
                m_MaxPoint = value;
                m_Origin = new Vector2((m_MinPoint.X + m_MaxPoint.X) / 2.0f, (m_MinPoint.Y + m_MaxPoint.Y) / 2.0f);
            }
        }

        public Vector2 Origin
        {
            get { return m_Origin; }
        }

        public Texture2D GrassTexture
        {
            get { return m_GrassTexture; }
            set { m_GrassTexture = value; }
        }
        #endregion

        #region Construction
        private static WorldArena m_Instance = null;
        private WorldArena() { }

        public static WorldArena Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new WorldArena();
            }

            return m_Instance;
        }
        #endregion

        #region Methods
        public void Update(GameTime a_GameTime)
        {
            for (int loop = 0; loop < SnakeManager.Instance().List.Count; loop++)
            {
                SnakeManager.Instance().List[loop].Update(a_GameTime);
            }
        }

        public void Draw(SpriteBatch a_SpriteBatch)
        {
            for (int loop = 0; loop < SnakeManager.Instance().List.Count; loop++)
            {
                SnakeManager.Instance().List[loop].Draw(a_SpriteBatch);
            }
        }
        #endregion
    }
}
