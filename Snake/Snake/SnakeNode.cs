using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Snake
{
    public enum SegmentType // in terms of how it acts in logic
    {
        Head = 0,
        Body = 1,
        Tail = 2,
        None = -1,
    }

    public enum GraphicType // in terms of what is shown as a graphic
    {
        Head = 0,
        Body = 1,
        Tail = 2,
        None = -1,
    }

    public class Circle
    {
        #region Fields
        float m_X;
        float m_Y;
        float m_R;
        #endregion

        #region Properties
        public float X
        {
            get { return m_X; }
            set { m_X = value; }
        }
        public float Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }
        public float R
        {
            get { return m_R; }
            set { m_R = value; }
        }
        #endregion

        #region Construction
        public Circle(float a_X, float a_Y, float a_R)
        {
            m_X = a_X;
            m_Y = a_Y;
            m_R = a_R;
        }
        #endregion

        #region Methods
        public bool Intersects(Circle a_Circle)
        {
            Vector2 thisVector = new Vector2(m_X, m_Y);
            Vector2 thatVector = new Vector2(a_Circle.m_X, a_Circle.m_Y);
            Vector2 displaceBetween = thisVector - thatVector;

            float totalRange = (this.m_R + a_Circle.m_R) / 2;

            if (Math.Abs(displaceBetween.X) < totalRange && Math.Abs(displaceBetween.Y) < totalRange)
            {
                return true;
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
        #endregion
    }

    public class SnakeNode
    {
        #region Constants

        #endregion

        #region Fields
        private SegmentType m_SegmentType = SegmentType.None;
        private GraphicType m_GraphicType = GraphicType.None;
        private Vector2 m_Position = Vector2.Zero;
        private Vector2 m_Velocity = Vector2.Zero;
        private Vector2 m_ConstantVelocity = Vector2.Zero;

        private Texture2D m_Texture2D = null;

        private SnakeNode m_Follow = null;
        #endregion

        #region Properties
        public GraphicType GraphicType
        {
            get { return m_GraphicType; }
            set
            {
                m_GraphicType = value;
            }
        }

        public SegmentType SegmentType
        {
            get { return m_SegmentType; }
            set { m_SegmentType = value; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public Vector2 ConstantVelocity
        {
            get { return m_ConstantVelocity; }
            set { m_ConstantVelocity = value; }
        }

        public Circle CollisionCircle
        {
            get { return new Circle(Position.X, Position.Y, 64); } // TODO: Adjust radius to texture half.
        }

        public SnakeNode Follow
        {
            get { return m_Follow; }
            set { m_Follow = value; }
        }

        private float m_Rotation = 0;
        public int Rotation
        {
            get {
                if (m_SegmentType != Snake.SegmentType.Head)
                {
                    return (int)MathHelper.ToDegrees((float)Math.Atan2(ConstantVelocity.X, ConstantVelocity.Y));
                }
                else
                {
                    int i = 5;
                }

                if (m_Rotation > MathHelper.PiOver4)
                {
                    m_Rotation = -MathHelper.Pi;
                }

                if (m_Rotation < -MathHelper.PiOver4)
                {
                    m_Rotation = MathHelper.Pi;
                }

                return (int)MathHelper.ToDegrees(m_Rotation);
            }
            set { m_Rotation = value; }
        }
        #endregion

        #region Construction
        public SnakeNode(SnakeNode a_Follow, SegmentType a_SegmentType, GraphicType a_GraphicType, Vector2 a_Translation)
        {
            m_Follow = a_Follow;
            SegmentType = a_SegmentType;
            m_GraphicType = a_GraphicType;
            Position = Vector2.Zero + a_Translation;
        }

        public void LoadContent(ContentManager content)
        {
            m_Texture2D = content.Load<Texture2D>(@"Asset\Snake");
        }

        #endregion

        #region Methods
        private float scaleDown = 100.0f;
        public void SendRotation(float a_Rotation)
        {
            m_Rotation = a_Rotation;
        }

        public void Update(GameTime a_GameTime)
        {
            if (Follow != null)
            {
                if (CollisionCircle.Intersects(Follow.CollisionCircle) == false)
                {
                    Velocity = Follow.Position - Position;
                    Position += (Vector2.Normalize(Velocity)) * 5;

                    if (Velocity != Vector2.Zero)
                    {
                        ConstantVelocity = Velocity;
                    }
                }
            }
        }
        #endregion
    }
}
