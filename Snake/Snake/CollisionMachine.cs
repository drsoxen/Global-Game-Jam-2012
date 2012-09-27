using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Snake
{
    public class CollisionMachine
    {
        public enum Collision
        {
            HitTail,
            HitHead,
            HitSegment,
            HitWall,
            HitOther,
            HitOwnTail,
            HitOwnSegment,
            None
        }

        #region Fields
        private SnakePlayer m_SnakePlayer = null;
        private SnakePlayer m_CollidedWith = null;

        private List<SnakePlayer> m_SnakeList = new List<SnakePlayer>();

        protected float m_Range = 600;

        private Collision m_CollisionType = Collision.None;

        protected Vector2 m_CheckPositionOld = Vector2.Zero;
        protected Vector2 m_CheckPositionCurrent = Vector2.Zero;
        protected float m_CheckMaxDisplacement = 25;

        protected Vector2 m_UpdatePositionOld = Vector2.Zero;
        protected Vector2 m_UpdatePositionCurrent = Vector2.Zero;
        protected float m_UpdateMaxDisplacement = 300;
        #endregion

        #region Properties
        public SnakePlayer CollidedWith
        {
            get { return m_CollidedWith; }
            set { m_CollidedWith = value; }
        }

        public List<SnakePlayer> SnakeList
        {
            get { return m_SnakeList; }
            set { m_SnakeList = value; }
        }

        protected float CheckDisplacement
        {
            get { return Displacement(m_CheckPositionOld, m_CheckPositionCurrent); }
        }
        protected float UpdateDisplacement
        {
            get { return Displacement(m_UpdatePositionOld, m_UpdatePositionCurrent); }
        }

        public Collision CollisionType
        {
            get { return m_CollisionType; }
            set { m_CollisionType = value; }
        }
        #endregion

        #region Construction
        public CollisionMachine(SnakePlayer a_SnakePlayer)
        {
            m_SnakePlayer = a_SnakePlayer;
            UpdateCollision();
        }
        #endregion

        #region Methods
        public virtual bool Update(GameTime a_GameTime)
        {
            m_CheckPositionCurrent = m_SnakePlayer.Position + (Vector2.Normalize(m_SnakePlayer.Velocity)) * 5;
            m_UpdatePositionCurrent = m_SnakePlayer.Position + (Vector2.Normalize(m_SnakePlayer.Velocity)) * 5;

            if (UpdateDisplacement > m_UpdateMaxDisplacement)
            {
                m_UpdatePositionOld = m_UpdatePositionCurrent;
                UpdateCollision();
            }
            if (CheckDisplacement > m_CheckMaxDisplacement)
            {
                m_CheckPositionOld = m_CheckPositionCurrent;
                if (CheckCollision())
                {
                    return true;
                }
                else
                {
                }
            }
            else
            {
                CollisionType = Collision.None;
                Vector2 incr = (Vector2.Normalize(m_SnakePlayer.Velocity)) * 5;
                m_SnakePlayer.Position += incr;
            }

            return false;
        }

        protected float Displacement(Vector2 aPosition_1, Vector2 aPosition_2)
        {
            float tempDisplacement = 0;

            tempDisplacement = (float)Math.Sqrt((aPosition_1.X - aPosition_2.X) * (aPosition_1.X - aPosition_2.X) +
                                                (aPosition_1.Y - aPosition_2.Y) * (aPosition_1.Y - aPosition_2.Y));
            return tempDisplacement;
        }

        protected virtual void UpdateCollision()
        {
            for (int loop = 0; loop < CollisionManager.Instance().SnakeList.Count; loop++)
            {
                if (Displacement(m_UpdatePositionCurrent, CollisionManager.Instance().SnakeList[loop].Position) < m_Range)
                {
                    m_SnakeList.Add(CollisionManager.Instance().SnakeList[loop]);
                }
            }
        }

        protected virtual bool CheckCollision()
        {
            for (int loop = 0; loop < m_SnakeList.Count; loop++)
            {
                if (m_SnakeList[loop] != m_SnakePlayer)
                {
                    if (CheckForCollision(loop, m_SnakePlayer))
                    {
                        return true;
                    }
                }
                else
                {
                    if (CheckForSelfCollision())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckForCollision(int a_loop, SnakePlayer a_SnakePlayer)
        {
            for (int loop = 0; loop < m_SnakeList[a_loop].SnakeNodes.Count; loop++)
            {
                if(a_SnakePlayer.SnakeNodes.Count > 0)
                {
                    if (a_SnakePlayer.SnakeNodes[0].CollisionCircle.Intersects(m_SnakeList[a_loop].SnakeNodes[loop].CollisionCircle))
                    {
                        if (m_SnakeList[a_loop].SnakeNodes[loop].SegmentType == SegmentType.Body)
                        {
                            CollidedWith = m_SnakeList[a_loop];
                            CollisionType = Collision.HitSegment;
                        }
                        if (m_SnakeList[a_loop].SnakeNodes[loop].SegmentType == SegmentType.Head)
                        {
                            CollidedWith = m_SnakeList[a_loop];
                            CollisionType = Collision.HitHead;
                        }
                        if (m_SnakeList[a_loop].SnakeNodes[loop].SegmentType == SegmentType.Tail)
                        {
                            CollidedWith = m_SnakeList[a_loop];
                            CollisionType = Collision.HitTail;
                        }

                        return true;
                    }
                }
            }

            CollisionType = Collision.None;

            return false;
        }

        public bool CheckForSelfCollision()
        {
            for (int loop = 3; loop < m_SnakePlayer.SnakeNodes.Count; loop++)
            {
                if (m_SnakePlayer.SnakeNodes[0].CollisionCircle.Intersects(m_SnakePlayer.SnakeNodes[loop].CollisionCircle))
                {
                    if (m_SnakePlayer.SnakeNodes[loop].SegmentType == SegmentType.Body)
                    {
                        CollidedWith = m_SnakePlayer;
                        CollisionType = Collision.HitOwnSegment;
                    }
                    if (m_SnakePlayer.SnakeNodes[loop].SegmentType == SegmentType.Head)
                    {
                        throw new Exception("More than one head.");
                    }
                    if (m_SnakePlayer.SnakeNodes[loop].SegmentType == SegmentType.Tail)
                    {
                        CollidedWith = m_SnakePlayer;
                        CollisionType = Collision.HitOwnTail;
                    }

                    return true;
                }
            }

            CollisionType = Collision.None;

            return false;
        }

        #endregion
    }
}


