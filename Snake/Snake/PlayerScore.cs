using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    public class PlayerScore
    {
        #region Fields
        private SnakePlayer m_SnakePlayer = null;
        #endregion

        #region Properties
        public int Score
        {
            get { return m_SnakePlayer.SnakeNodes.Count; }
        }

        public SnakePlayer SnakePlayer
        {
            get { return m_SnakePlayer; }
        }

        private string Name
        {
            get { return m_SnakePlayer.Name; }
        }

        private bool IsDead
        {
            get
            {
                if (m_SnakePlayer.ID == -1)
                {
                    return m_SnakePlayer.IsDead;
                }
                return false;
            }
        }
        #endregion

        #region Construction
        public PlayerScore(SnakePlayer a_SnakePlayer)
        {
            m_SnakePlayer = a_SnakePlayer;

            ScoreManager.PlayerScores.Add(this);
        }
        #endregion

        #region Methods
        public string DisplayScore()
        {
            return Name + ": " + Score.ToString();
        }

        public string DisplayDead()
        {
            switch (m_SnakePlayer.RespawnArea)
            {
                case Snake.SnakePlayer.Area.TOP:
                    return "Respawning at TOP in " + m_SnakePlayer.DeathTimed.ToString();
                    break;
                case Snake.SnakePlayer.Area.BOTTOM:
                    return "Respawning at BOTTOM in " + m_SnakePlayer.DeathTimed.ToString();
                    break;
                case Snake.SnakePlayer.Area.LEFT:
                    return "Respawning at LEFT in " + m_SnakePlayer.DeathTimed.ToString();
                    break;
                case Snake.SnakePlayer.Area.RIGHT:
                    return "Respawning at RIGHT in " + m_SnakePlayer.DeathTimed.ToString();
                    break;
            }

            return m_SnakePlayer.DeathTimed.ToString();
        }
        #endregion
    }
}
