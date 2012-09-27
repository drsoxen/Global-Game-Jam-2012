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
    class CreditState : GameState
    {
        #region Fields
        private MainMenuState m_MainMenuState = null;
        private Texture2D m_CreditsBackground = null;
        private bool m_IsInitialized = false;
        #endregion

        public override void LoadContent(ContentManager content)
        {
            SnakeManager.Instance().Content = content;

            m_MainMenuState = new MainMenuState();
            m_MainMenuState.LoadContent(content);
            m_MainMenuState.spriteBatch = spriteBatch;
            m_MainMenuState.Initialize();

            m_CreditsBackground = content.Load<Texture2D>(@"Asset\Snake");
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (m_IsInitialized == true)
            {
                if (this.transitionState != GameState.GameTransitionState.off)
                {
                    if (InputManager.SingleKeyPressInput(Keys.Space))
                    {
                        this.transitionState = GameState.GameTransitionState.off;
                        manager.states.Add(m_MainMenuState);
                    }
                }
            }
            else
            {
                m_IsInitialized = true;
            }
        }

        public override void HandleInput(GameTime gametime)
        {

        }

        public override void Draw(GameTime gametime)
        {
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(m_CreditsBackground, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
