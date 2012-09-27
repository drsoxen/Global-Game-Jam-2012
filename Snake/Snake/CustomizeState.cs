using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    class CustomizeState : GameState
    {
        #region Fields
        private Customize m_Customize;
        #endregion

        public override void LoadContent(ContentManager content)
        {
            SnakeManager.Instance().Content = content;

            PlayState tempPlayState = new PlayState();
            tempPlayState.LoadContent(content);
            tempPlayState.spriteBatch = spriteBatch;

            m_Customize = new Customize();
            m_Customize.ShowDialog();

            manager.states.Remove(this);
            manager.states.Add(tempPlayState);
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void HandleInput(GameTime gametime)
        {

        }

        public override void Draw(GameTime gametime)
        {
            graphicsDevice.Clear(Color.Black);
        }
    }
}
