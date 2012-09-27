using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Snake
{
    public class CollisionManager
    {
        //TODO: Collide with other stuff? ie rocks
        #region Properties
        public List<SnakePlayer> SnakeList
        {
            get { return SnakeManager.Instance().List; }
            set { SnakeManager.Instance().List = value; }
        }
        #endregion

        #region Construction
        private CollisionManager(){}
        private static CollisionManager m_Instance = null;
        public static CollisionManager Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new CollisionManager();
            }

            return m_Instance;
        }
        #endregion

        #region Methods
        public void Reset()
        {
            SnakeList.Clear();
        }
        #endregion
    }
}