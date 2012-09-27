using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    class AnimatedSnake
    {
        AnimatedTexture[] parts;

        struct FramePair
        {
            public FramePair(int deg, int frame, bool flip)
            {
                this.deg = deg;
                this.frame = frame;
                this.flip = flip;
            }
            public int deg;
            public int frame;
            public bool flip;
            public override string ToString()
            {
                return "deg:" + deg.ToString() + " fr:" + frame.ToString() + " fl:" + flip.ToString();
            }
        }

        FramePair[] pairs;

        public void Initialize(Texture2D sourcetexture)
        {
            parts = new AnimatedTexture[]
            {
                new AnimatedTexture(sourcetexture, new Rectangle(0, 0, 128, 128), SpriteDirection.right, 5, 1),
                new AnimatedTexture(sourcetexture, new Rectangle(0, 128, 128, 128), SpriteDirection.right, 5, 1),
                new AnimatedTexture(sourcetexture, new Rectangle(0, 256, 128, 128), SpriteDirection.right, 5, 1),
            };

            int s = 36;
            pairs = new FramePair[]
            {
                new FramePair(-180 + (s*0), 4, false),
                new FramePair(-180 + (s*1), 3, false),
                new FramePair(-180 + (s*2), 2, false),
                new FramePair(-180 + (s*3), 2, false),
                new FramePair(-180 + (s*4), 1, false),

                new FramePair((s*0), 0, true),
                new FramePair((s*1), 1, true),
                new FramePair((s*2), 2, true),
                new FramePair((s*3), 3, true),
                new FramePair((s*4), 4, true),
            };
        }

        public void Draw(SpriteBatch spritebatch, int angle, Color tint, Vector2 position, GraphicType gtype)
        {
            int gidx = (int)gtype;
            int closesta=int.MaxValue;
            FramePair current;
            int i, j = 0;

            if (pairs == null || pairs.Length < 1)
                return;
            current = pairs[0];
            
            for (i = 0; i < pairs.Length; ++i)
            {
                if (Math.Max(pairs[i].deg, angle) - Math.Min(pairs[i].deg, angle) < closesta)
                {
                    closesta = Math.Max(pairs[i].deg, angle) - Math.Min(pairs[i].deg, angle);
                    current = pairs[i];
                    j = i;
                    if (closesta == 0)
                        break;

                }
            }
            System.Diagnostics.Trace.WriteLine(j);

            parts[gidx].invertHorizontal = current.flip;
            parts[gidx].gotoAndStop(current.frame);
            // TODO(xoorath): don't hard code 128*128
            parts[gidx].Draw(spritebatch, new Rectangle((int)position.X, (int)position.Y, 128, 128), tint, 0.0f); 

            if (SnakeManager.Instance().DFont != null)
                spritebatch.DrawString(SnakeManager.Instance().DFont, j.ToString() + " : " + angle.ToString(), position, Color.White);
        }
    }
}
