using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using NetZ;

namespace Snake
{
    class PlayState : GameState
    {
        Texture2D grass;
        Texture2D Grass, DGrass;
        int m_UserID = -1;
        List<string> m_UserIDs = new List<string>();
        int[][] m_Ground = CaveGenerator.Generate(100, 100, 2);

        Stopwatch s = new Stopwatch();

        public override void LoadContent(ContentManager content)
        {
            
            grass = content.Load<Texture2D>(@"Asset\Grass");
            SnakeManager.Instance().LoadContent(content);
            Grass = content.Load<Texture2D>(@"Asset\NGrass");
            DGrass = content.Load<Texture2D>(@"Asset\DGrass");
        }

        public override void Initialize()
        {

        }

        public void WatchUsers()
        {
            Thread.Sleep(2000);
            if (m_UserID == -1 && m_UserIDs.Count > 0)
            {
                m_UserID = m_UserIDs.Count;
            }
        }

        public override void Update(GameTime gameTime)
        {
            string output = "";
            s.Restart();
            WorldArena.Instance().Update(gameTime);
            EmitterManager.Update(gameTime);
            
        }

        public void UpdatePlayerStatus()
        {

        }

        public override void HandleInput(GameTime gametime)
        {
            
        }

        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(grass, grass.Bounds, Color.White);
            tile();
            WorldArena.Instance().Draw(spriteBatch);
            ScoreManager.Draw(spriteBatch);
            EmitterManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void tile()
        {
            int numberAcross = 1;
            int numberDown = 1;
            int sizeAcross = 0;
            int sizeDown = 0;

            numberAcross = m_Ground.Length;
            numberDown = m_Ground[0].Length;

            int s = 64;

            for (int i = 1; i < numberDown; i++)
            {
                for (int c = 1; c < numberAcross; c++)
                {
                    Rectangle Dest = new Rectangle(0, 0, s, s);
                    Dest.X = s * (i-1); 
                    Dest.Y = s * (c-1);
                    switch (m_Ground[i][c])
                    {
                        case 0:
                            spriteBatch.Draw(Grass, Dest, Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(DGrass, Dest, Color.White);
                            break;
                    }
                    sizeAcross += s;
                }
                sizeDown += s;
                sizeAcross = 0;
            }
        }

        
    }
}
