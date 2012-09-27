using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    public class SnakePlayer
    {
        #region Enum
        public enum Area
        {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }
        #endregion

        #region Debug
        const bool debugControls =
#if DEBUG
 true;
#else
            false;
#endif
        #endregion

        #region Constants
        float MAX_SPEED = 4.0f;

        float UPDATE_DISPLACEMENT = 64.0f;
        #endregion

        #region Fields
        private bool m_IsLocalPlayer = false;

        private bool m_IsConsumed = false;
        private bool m_IsExploding = false;
        private bool m_IsDead = false;

        private bool m_IsInitialized = false;

        private float m_RespawnTotal = 50.0f;
        private float m_RespawnCurrent = 0.0f;
        private Area m_RespawnArea = Area.TOP;

        private string m_Name = "Default";

        private Vector2 m_Position = new Vector2(400, 400);
        private Vector2 m_OldPosition = Vector2.Zero;
        private Vector2 m_Velocity = Vector2.Zero;

        private Color m_Tint = Color.White;
        private AnimatedSnake m_Animation = new AnimatedSnake();

        private List<SnakeNode> m_SnakeNodes = new List<SnakeNode>();
        private Vector2 m_Origin = new Vector2(64, 64);
        private Rectangle m_SourceRectangle = new Rectangle(0, 0, 126, 126);
        private float m_Rotation = 0.0f;
        private float m_Scale = 1.0f;

        private int m_ID = 0;
        private CollisionMachine m_CollisionController = null;

        private PlayerScore m_PlayerScore = null;

        private Color m_CustomTint = Color.White;
        #endregion

        #region Properties
        public bool IsLocalPlayer
        {
            get { return m_IsLocalPlayer; }
            set { m_IsLocalPlayer = value; }
        }

        public bool IsConsumed
        {
            get { return m_IsConsumed; }
            set { m_IsConsumed = value; }
        }

        public bool IsDead
        {
            get { return m_IsDead; }
            set { m_IsDead = value; }
        }

        public float DeathTimed
        {
            get
            {
                return m_RespawnTotal - m_RespawnCurrent;
            }
        }

        public Area RespawnArea
        {
            get { return m_RespawnArea; }
        }

        public string Name
        {
            get
            {
                if (m_IsLocalPlayer)
                {
                    m_Name = SnakeManager.Instance().LocalName;
                }

                return m_Name;
            }
            set { m_Name = value; }
        }

        public Vector2 Position
        {
            get
            {
                if (m_Position.X < WorldArena.Instance().MinPoint.X ||
                    m_Position.X > WorldArena.Instance().MaxPoint.X ||
                    m_Position.Y < WorldArena.Instance().MinPoint.Y ||
                    m_Position.Y > WorldArena.Instance().MaxPoint.Y)
                {
                    Explode();
                }

                return m_Position;
            }
            set { m_Position = value; }
        }
        public Vector2 OldPosition
        {
            get { return m_OldPosition; }
            set { m_OldPosition = value; }
        }

        public Vector2 Velocity
        {
            get
            {
                Vector2 tempVelocity = new Vector2((float)Math.Sin(m_Rotation), (float)Math.Cos(m_Rotation));
                return tempVelocity;
            }
            set { m_Velocity = value; }
        }

        public List<SnakeNode> SnakeNodes
        {
            get { return m_SnakeNodes; }
            set { m_SnakeNodes = value; }
        }

        public Color Tint
        {
            get { return m_Tint; }
            set { m_Tint = value; }
        }

        public Color CustomTint
        {
            get
            {
                if (m_IsLocalPlayer)
                {
                    m_CustomTint = SnakeManager.Instance().LocalCustomTint;
                }

                return m_CustomTint;
            }
            set
            {
                m_CustomTint.R = (Byte)((m_CustomTint.R + 255.0f) / 2.0f);
                m_CustomTint.G = (Byte)((m_CustomTint.G + 255.0f) / 2.0f);
                m_CustomTint.B = (Byte)((m_CustomTint.B + 255.0f) / 2.0f);

                m_CustomTint = value;
            }
        }

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        private Vector2 RespawnDirection
        {
            get
            {
                Vector2 tempVector2 = (Position - WorldArena.Instance().Origin);
                return Vector2.Normalize(tempVector2);
            }
        }
        #endregion

        #region Construction
        private static int staticId = 0;
        public SnakePlayer()
        {
            m_ID = staticId++;
            m_CollisionController = new CollisionMachine(this);
            m_PlayerScore = new PlayerScore(this);
        }

        public void Initialize()
        {
            m_Animation.Initialize(SnakeManager.Instance().Texture);
            m_IsInitialized = true;
            if (m_IsInitialized == false)
            {
                m_IsInitialized = true;

                m_Animation.Initialize(SnakeManager.Instance().Texture);

                m_IsDead = true;
            }
        }
        #endregion

        #region Methods
        public void Update(GameTime a_GameTime)
        {
            if (m_Rotation > MathHelper.Pi)
            {
                m_Rotation = -MathHelper.Pi;
            }

            if (m_Rotation < -MathHelper.Pi)
            {
                m_Rotation = MathHelper.Pi;
            }

            if (SnakeNodes.Count > 0)
            {
                SnakeNodes[0].SendRotation(m_Rotation);
            }

            OnCollisionEffects(a_GameTime);

            if (m_IsDead)
            {
                float tempDeathTimed = m_RespawnTotal - m_RespawnCurrent;

                if (tempDeathTimed < 0)
                {
                    m_RespawnCurrent = 0;
                    m_IsDead = false;
                    Respawn();
                }

                m_RespawnCurrent += (float)a_GameTime.ElapsedGameTime.TotalMilliseconds * 0.02f;
            }

            HandleInput(a_GameTime);

            if (!m_IsExploding)
            {
                m_CollisionController.Update(a_GameTime);
                UpdateSnakeBody(a_GameTime);
            }
        }

        private float explodingInterval = 50;
        private float explodingTime = 0;
        private void OnCollisionEffects(GameTime a_GameTime)
        {
            if (SnakeNodes.Count < 1)
            {
                m_IsDead = true;
            }

            if (!m_IsExploding)
            {
                if (m_CollisionController.CollisionType == CollisionMachine.Collision.HitSegment)
                {
                    RecursiveExplosive();
                }
                if (m_CollisionController.CollisionType == CollisionMachine.Collision.HitHead)
                {
                    m_CollisionController.CollidedWith.Explode();
                    Explode();
                }
                if (m_CollisionController.CollisionType == CollisionMachine.Collision.HitTail)
                {
                    Consume(m_CollisionController.CollidedWith);
                }
                if (m_CollisionController.CollisionType == CollisionMachine.Collision.HitOwnTail)
                {
                    Explode();
                }
                if (m_CollisionController.CollisionType == CollisionMachine.Collision.HitOwnSegment)
                {
                    RecursiveExplosive();
                }
            }

            if (SnakeNodes.Count > 0)
            {
                if (m_IsExploding)
                {
                    explodingTime += (float)a_GameTime.ElapsedGameTime.TotalMilliseconds * 0.10f;

                    if (explodingTime > explodingInterval)
                    {
                        explodingTime = 0;

                        EmitterManager.List.Add(new Emitter(Color.Red, SnakeNodes[0].Position));
                        SnakeNodes.Remove(SnakeNodes[0]);
                    }
                }
            }
        }

        private void Die()
        {
            Die(this);
        }

        private void Explode()
        {
            for (int loop = 0; loop < SnakeNodes.Count; loop++)
            {
                EmitterManager.List.Add(new Emitter(Color.Red, SnakeNodes[loop].Position));
            }

            Die(this);
        }

        private void RecursiveExplosive()
        {
            m_IsExploding = true;
        }

        private void Die(SnakePlayer a_SnakePlayer)
        {
            a_SnakePlayer.SnakeNodes.Clear();
        }


        private void Consume(SnakePlayer a_SnakePlayer)
        {
            if (a_SnakePlayer.m_IsExploding)
            {
                return;
            }

            if (m_SnakeNodes.Count > 0 && a_SnakePlayer.SnakeNodes.Count > 0 && !m_IsDead && !m_IsExploding && !m_IsConsumed)
            {
                if (a_SnakePlayer.IsConsumed == false)
                {
                    EmitterManager.List.Add(new Emitter(Color.Green, Position));

                    List<SnakeNode> tempList = new List<SnakeNode>();

                    for (int loop = 0; loop < a_SnakePlayer.SnakeNodes.Count; loop++)
                    {
                        tempList.Add(a_SnakePlayer.SnakeNodes[loop]);
                    }

                    if (tempList.Count > 0)
                    {
                        for (int loop = 0; loop < SnakeNodes.Count; loop++)
                        {
                            tempList.Add(SnakeNodes[loop]);
                            SnakeNodes[loop].Follow = tempList[tempList.Count - 2];
                        }

                        for (int loop = 1; loop < tempList.Count - 1; loop++)
                        {
                            tempList[loop].SegmentType = SegmentType.Body;
                            tempList[loop].GraphicType = GraphicType.Body;
                        }

                        tempList[0].SegmentType = SegmentType.Head;
                        tempList[tempList.Count - 1].SegmentType = SegmentType.Tail;
                        tempList[tempList.Count - 1].GraphicType = GraphicType.Tail;

                        SnakeNodes = tempList;

                        a_SnakePlayer.IsConsumed = true;

                        Position = SnakeNodes[0].Position;
                    }

                    a_SnakePlayer.Die(a_SnakePlayer);
                }
            }
        }

        private void Respawn()
        {
            if (m_SnakeNodes.Count < 1)
            {
                //ChosenRespawnArea();
                RandomRespawnArea();

                m_IsConsumed = false;
                m_IsDead = false;
                m_IsExploding = false;

                int hello = 0;
                m_SnakeNodes.Add(new SnakeNode(null, SegmentType.Head, GraphicType.Head, m_Position + RespawnDirection * 126 * 1));

                for (int i = 0; i < 2; ++i)
                    m_SnakeNodes.Add(new SnakeNode(m_SnakeNodes[++hello - 1], SegmentType.Body, GraphicType.Body, m_Position + RespawnDirection * 126 * hello));

                m_SnakeNodes.Add(new SnakeNode(m_SnakeNodes[++hello - 1], SegmentType.Tail, GraphicType.Tail, m_Position + RespawnDirection * 126 * hello));
            }
        }

        private void ChosenRespawnArea()
        {
            switch (m_RespawnArea)
            {
                case Area.TOP:
                    m_Position = new Vector2((float)(new Random().NextDouble() * WorldArena.Instance().MaxPoint.X), WorldArena.Instance().MinPoint.Y);
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
                case Area.BOTTOM:
                    m_Position = new Vector2((float)(new Random().NextDouble() * WorldArena.Instance().MaxPoint.X), WorldArena.Instance().MaxPoint.Y);
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
                case Area.RIGHT:
                    m_Position = new Vector2(WorldArena.Instance().MaxPoint.X, (float)(new Random().NextDouble() * WorldArena.Instance().MaxPoint.Y));
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
                case Area.LEFT:
                    m_Position = new Vector2(WorldArena.Instance().MinPoint.X, (float)(new Random().NextDouble() * WorldArena.Instance().MaxPoint.Y));
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
            }
        }

        static int randomSeed = 0;
        static int respawnArea = 0;
        private void RandomRespawnArea()
        {
            respawnArea++;
            if (respawnArea > 3)
            {
                respawnArea = 0;
            }

            switch (respawnArea)
            {
                case 0:
                    m_Position = new Vector2((float)(new Random(randomSeed++).NextDouble() * WorldArena.Instance().MaxPoint.X), WorldArena.Instance().MinPoint.Y + 1);
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
                case 1:
                    m_Position = new Vector2((float)(new Random(randomSeed++).NextDouble() * WorldArena.Instance().MaxPoint.X), WorldArena.Instance().MaxPoint.Y - 1);
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
                case 2:
                    m_Position = new Vector2(WorldArena.Instance().MaxPoint.X - 1, (float)(new Random(randomSeed++).NextDouble() * WorldArena.Instance().MaxPoint.Y));
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
                case 3:
                    m_Position = new Vector2(WorldArena.Instance().MinPoint.X + 1, (float)(new Random(randomSeed++).NextDouble() * WorldArena.Instance().MaxPoint.Y));
                    m_Rotation = -(float)Math.Atan2(RespawnDirection.X, -RespawnDirection.Y);
                    break;
            }
        }

        private void UpdateSnakeBody(GameTime a_GameTime)
        {
            if (m_SnakeNodes.Count > 0)
            {
                for (int loop = 1; loop < m_SnakeNodes.Count; loop++)
                {
                    m_SnakeNodes[loop].Update(a_GameTime);
                }

                m_SnakeNodes[0].Position = Position;
            }
        }

        private float scaleDown = 300.0f;
        private void HandleInput(GameTime a_GameTime)
        {
            if (IsLocalPlayer == false)
            {
                return;
            }

            if (InputManager.KeyHeldDown(Keys.A) || InputManager.KeyHeldDown(Keys.Left))
            {
                m_Rotation += (float)a_GameTime.ElapsedGameTime.TotalMilliseconds / scaleDown;
            }
            if (InputManager.KeyHeldDown(Keys.D) || InputManager.KeyHeldDown(Keys.Right))
            {
                m_Rotation -= (float)(a_GameTime.ElapsedGameTime.TotalMilliseconds / scaleDown);
            }

            if (InputManager.SingleKeyPressInput(Keys.A) || InputManager.SingleKeyPressInput(Keys.Left))
            {
                m_RespawnArea = Area.LEFT;
            }
            if (InputManager.SingleKeyPressInput(Keys.D) || InputManager.SingleKeyPressInput(Keys.Right))
            {
                m_RespawnArea = Area.RIGHT;
            }
            if (InputManager.SingleKeyPressInput(Keys.W) || InputManager.SingleKeyPressInput(Keys.Up))
            {
                m_RespawnArea = Area.TOP;
            }
            if (InputManager.SingleKeyPressInput(Keys.S) || InputManager.SingleKeyPressInput(Keys.Down))
            {
                m_RespawnArea = Area.BOTTOM;
            }
        }

        public void Draw(SpriteBatch a_SpriteBatch)
        {
            if (m_IsInitialized == false)
            {
                Initialize();
            }
            if (m_SnakeNodes.Count > 0)
            {
                if (m_SnakeNodes.Count == 1 || m_SnakeNodes[0].Position.Y > m_SnakeNodes[1].Position.Y)
                {
                    for (int loop = 1; loop < m_SnakeNodes.Count; ++loop)
                    {
                        m_Animation.Draw(a_SpriteBatch, m_SnakeNodes[loop].Rotation, Color.White, m_SnakeNodes[loop].Position, m_SnakeNodes[loop].GraphicType);
                    }
                    m_Animation.Draw(a_SpriteBatch, (int)MathHelper.ToDegrees(m_Rotation), CustomTint, m_SnakeNodes[0].Position, m_SnakeNodes[0].GraphicType);
                }
                else
                {
                    m_Animation.Draw(a_SpriteBatch, (int)MathHelper.ToDegrees(m_Rotation), CustomTint, m_SnakeNodes[0].Position, m_SnakeNodes[0].GraphicType);
                    for (int loop = 1; loop < m_SnakeNodes.Count; ++loop)
                    {
                        m_Animation.Draw(a_SpriteBatch, m_SnakeNodes[loop].Rotation, Color.White, m_SnakeNodes[loop].Position, m_SnakeNodes[loop].GraphicType);
                    }
                }
            }
        }

        public bool CheckForCollision(SnakePlayer a_SnakePlayer)
        {
            if (m_SnakeNodes.Count > 0)
            {
                for (int loop = 0; loop < SnakeNodes.Count; loop++)
                {
                    if (a_SnakePlayer.SnakeNodes[0].CollisionCircle.Intersects(SnakeNodes[loop].CollisionCircle))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
