﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    public class Particle
    {
        #region Fields
        private Vector2 m_Position = Vector2.Zero;
        private float m_Rotation = 0.0f;
        private float m_Speed = 0.0f;
        private float m_LifeTotal = 0.0f;
        private float m_LifeCurrent = 0.0f;
        private Color m_Tint = Color.White;

        private Emitter m_Owner = null;

        private Rectangle m_Rectangle = new Rectangle(0, 0, 8, 8);
        private Vector2 m_Origin = new Vector2(4,4);
        #endregion

        #region Properties
        private Vector2 Velocity
        {
            get
            {
                Vector2 tempVelocity = new Vector2((float)Math.Sin(m_Rotation), (float)Math.Cos(m_Rotation));
                return tempVelocity;
            }
        }
        private float Alpha
        {
            get { return (1 -((LifeCurrent + LifeTotal) / LifeTotal)) * 255; }
        }
        private float LifeCurrent
        {
            get { return m_LifeCurrent; }
            set { m_LifeCurrent = value; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }
        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }
        public float LifeTotal
        {
            get { return m_LifeTotal; }
            set { m_LifeTotal = value; }
        }
        public Color Tint
        {
            get { return m_Tint; }
            set { m_Tint = value; }
        }
        #endregion

        #region Construction
        public Particle(Emitter a_Owner)
        {
            m_Owner = a_Owner;
        }
        #endregion

        #region Methods
        public void Update(GameTime a_GameTime)
        {
            m_LifeCurrent += (float)a_GameTime.ElapsedGameTime.TotalMilliseconds * 0.05f;
            if (m_LifeCurrent > m_LifeTotal)
            {
                m_Owner.List.Remove(this);
            }

            m_Position += ((Vector2.Normalize(Velocity) * (float)a_GameTime.ElapsedGameTime.TotalMilliseconds / 3.0f)) * ((m_LifeCurrent + m_LifeTotal) / m_LifeTotal);
        }

        public void Draw(SpriteBatch a_SpriteBatch)
        { 
            m_Tint.A = (Byte)Alpha;

            a_SpriteBatch.Draw(EmitterManager.Sprite, Position, m_Rectangle, Tint, 0.0f, m_Origin, 1.0f, SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
