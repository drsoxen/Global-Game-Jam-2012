using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Snake
{
    public static class ScoreManager
    {
        #region Constants
        private static Vector2 SCOREBOARD_LOCATION = new Vector2(900, 300);
        private static Vector2 YOUR_SCORE_LOCATION = new Vector2(300, 100);
        #endregion

        #region Fields
        private static List<PlayerScore> m_PlayerScores = new List<PlayerScore>();
        private static PlayerScore m_YourScore = null;

        private static SpriteFont m_SmallFont = null;
        private static SpriteFont m_LargeFont = null;
        #endregion

        #region Properties
        public static List<PlayerScore> PlayerScores
        {
            get { return m_PlayerScores; }
            set { m_PlayerScores = value; }
        }

        public static PlayerScore YourScore
        {
            get
            {
                if (m_YourScore == null)
                {
                    for (int loop = 0; loop < m_PlayerScores.Count; loop++)
                    {
                        if (m_PlayerScores[loop].SnakePlayer.ID == -1)
                        {
                            m_YourScore = m_PlayerScores[loop];
                        }
                    }
                }
                return m_YourScore;
            }
            set { m_YourScore = value; }
        }

        public static SpriteFont SmallFont
        {
            get { return m_SmallFont; }
            set { m_SmallFont = value; }
        }
        public static SpriteFont LargeFont
        {
            get { return m_LargeFont; }
            set { m_LargeFont = value; }
        }
        #endregion

        #region Methods
        private const float spacing = 22.0f;
        public static void Draw(SpriteBatch a_SpriteBatch)
        {
            if (YourScore != null)
            {
                a_SpriteBatch.DrawString(LargeFont, YourScore.DisplayScore(), YOUR_SCORE_LOCATION, Color.Purple);
                if (YourScore.SnakePlayer.IsDead)
                {
                    a_SpriteBatch.DrawString(LargeFont, YourScore.DisplayDead(), YOUR_SCORE_LOCATION + new Vector2(0, spacing * 2.0f), Color.Purple);
                }
            }

            for (int loop = 0; loop < PlayerScores.Count; loop++)
            {
                a_SpriteBatch.DrawString(SmallFont, PlayerScores[loop].DisplayScore(), SCOREBOARD_LOCATION + new Vector2(0, loop * spacing), Color.Purple);
            }
        }
        #endregion
    }
}
