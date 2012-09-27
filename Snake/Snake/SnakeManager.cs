using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Snake
{
    public class SnakeManager
    {
        #region Fields

        private string m_LocalName = "Default";
        private Color m_LocalCustomTint = Color.White;
        
        private SpriteFont m_dfont;

        private List<SnakePlayer> m_List = new List<SnakePlayer>();

        private ContentManager m_ContentManager;
        private Texture2D m_Texture;
        #endregion

        #region Properties
        public string LocalName
        {
            get { return m_LocalName; }
            set { m_LocalName = value; }
        }
        public Color LocalCustomTint
        {
            get { return m_LocalCustomTint; }
            set { m_LocalCustomTint = value; }
        }
        
        public SpriteFont DFont
        {
            get { return m_dfont; }
        }

        public ContentManager Content
        {
            get { return m_ContentManager; }
            set { m_ContentManager = value; }
        }

        public List<SnakePlayer> List
        {
            get { return m_List; }
            set { m_List = value; }
        }

        public Texture2D Texture
        {
            get { return m_Texture; }
        }
        #endregion

        #region Construction
        private static SnakeManager m_Instance = null;
        private SnakeManager() { }

        public static SnakeManager Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new SnakeManager();
            }

            return m_Instance;
        }
        #endregion

        #region Methods
        public void LoadContent(ContentManager content)
        {
            m_Texture = content.Load<Texture2D>(@"Asset\Snake");
            m_dfont = content.Load<SpriteFont>(@"DebugDraw\debugfont");

            if (m_List.Count < 1)
            {
                for (int loop = 0; loop < 10; loop++)
                {
                    m_List.Add(new SnakePlayer());
                }

                m_List[0].IsLocalPlayer = true;
            }
        }
        #endregion
    }
}
