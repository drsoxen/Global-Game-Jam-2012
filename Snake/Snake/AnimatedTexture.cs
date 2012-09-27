using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    enum SpriteDirection
    {
        down,
        right
    }

    enum AnimationPlayState
    {
        pause,
        play,
        stop,
    }

    class AnimatedTexture
    {
        private int framecount;             // total number of frames
        private Rectangle[] destrect;       // all of the built sprites
        private int currentframe;           // what frame are we on now
        private double framespace;          // how many seconds between frames
        private TimeSpan frametime;         // the timer we update every tick
        private Texture2D sourcetexture;    // the texture we sample from
        private AnimationPlayState state;   // what is the animation doing right now?

        public bool invertHorizontal = false;
        public bool invertVertical = false;

#if DEBUG
        private bool valid = false;
#endif

        public AnimatedTexture(Texture2D sourcetexture, Rectangle firstframe, SpriteDirection direction, int framecount, int FPS)
        {
            Initialize(sourcetexture, firstframe, direction, framecount, FPS);
        }

        public virtual void Initialize(Texture2D sourcetexture, Rectangle firstframe, SpriteDirection direction, int framecount, int FPS)
        {
#if DEBUG // If we're not in debug let the initialize try anyways. worst case scenario it throws an exception.
            if (direction == SpriteDirection.right)
                valid = (sourcetexture != null && (firstframe.X + (firstframe.Width * framecount) <= sourcetexture.Width) && firstframe.Height <= sourcetexture.Height && FPS >= 1);    
            else
                valid = (sourcetexture != null && (firstframe.Y + (firstframe.Height * framecount) <= sourcetexture.Height) && firstframe.Width <= sourcetexture.Height && FPS >= 1);
            if (!valid)
                throw new Exception("Woah tiger, something isn't right here. Sort your shit out.");
#endif
            this.framecount = framecount;
            this.sourcetexture = sourcetexture;
            this.state = AnimationPlayState.play;
            this.framespace = (1.0 / (double)FPS);

            this.currentframe = 0;
            this.frametime = new TimeSpan(0, 0, 0, 0, 0);

            destrect = new Rectangle[framecount];
            for (int i = 0; i < framecount; ++i)
            {
                destrect[i] = new Rectangle();
                destrect[i].Width = firstframe.Width;
                destrect[i].Height = firstframe.Height;
                if (direction == SpriteDirection.right)
                {
                    destrect[i].X = (firstframe.X + (i * firstframe.Width));
                    destrect[i].Y = firstframe.Y;
                }
                else
                {   
                    destrect[i].X = firstframe.X;
                    destrect[i].Y = (firstframe.Y + (framecount * firstframe.Height));
                }
            }
        }

        public int frame
        {
            get { return currentframe; }
            set { currentframe = value; }
        }

        public void gotoFrame(int frame)
        {
            currentframe = frame;
            frametime = new TimeSpan(0, 0, 0, 0, 0);
        }

        public void gotoAndPause(int frame)
        {
            currentframe = frame;
        }

        public void gotoAndStop(int frame)
        {
            currentframe = frame;
            frametime = new TimeSpan(0, 0, 0, 0, 0);
        }

        public void gotoAndPlay(int frame)
        {
            currentframe = frame;
            play();
        }

        public void stop()
        {
            state = AnimationPlayState.stop;
            frametime = new TimeSpan(0, 0, 0, 0, 0);
            currentframe = 0;
        }

        public void pause()
        {
            state = AnimationPlayState.pause;
        }

        public void play()
        {
            state = AnimationPlayState.play;
        }

        public void Update(GameTime gametime)
        {
            if (state != AnimationPlayState.play)
                return;
#if DEBUG
            if (!valid)
                throw new Exception("Might want to initialize that animated texture bro.");
#endif
            frametime += gametime.ElapsedGameTime;
            if (frametime.TotalSeconds >= framespace)
            {
                ++currentframe;
                frametime -= frametime;
                if (currentframe >= framecount)
                    currentframe = 0;
            }
        }

        public void Draw(SpriteBatch spritebatch, Rectangle destination, Color tint, float rotation = 0.0f)
        {
#if DEBUG
            if (!valid)
                throw new Exception("Might want to initialize that animated texture bro.");
#endif
            Rectangle segment = new Rectangle(destrect[currentframe].X, destrect[currentframe].Y, destrect[currentframe].Width, destrect[currentframe].Height);
            if (invertHorizontal)
            {
                segment.Width *= -1;
                segment.X += segment.Height;
            }
            if (invertVertical)
            {   
                segment.Y += segment.Height;
                segment.Height *= -1;
            }
            spritebatch.Draw(sourcetexture, destination, segment, tint, rotation, new Vector2(segment.Width / 2, segment.Height / 2), SpriteEffects.None, 0);
        }
    }
}
