using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    // The base type we use for states in our game.
    // If you're looking for the buisness logic of the game, try PlayState.cs
    public abstract class GameState
    {
        public enum GameRunningState
        {
            running,    // run normally
            paused,     // also run normally, you have to implement logic for what you do when you're paused
            stopped,    // don't update or draw, just handle input
            deathrow,   // about to be killed.
        }

        public enum GameTransitionState
        {
            on,
            off,
            none,
        }

        public TimeSpan transitionOnTime = new TimeSpan(0, 0, 0, 0, 0);
        public TimeSpan transitionOffTime = new TimeSpan(0, 0, 0, 0, 0);
        public GameTransitionState transitionState = GameTransitionState.on; // GameStateManager handles this
        public TimeSpan currentTransition = new TimeSpan(0, 0, 0, 0, 0); // Don't modify this. GameStateManager handles this.
        public float transitionProgress = 0.0f; // 0.0-1.0 GameStateManager sets this value. Read only.

        public GameRunningState runningState = GameRunningState.running;
        public GameStateManager manager;

        public ContentManager content;
        public SpriteBatch spriteBatch;
        public GraphicsDevice graphicsDevice;
        public GraphicsDeviceManager graphicsDeviceManager;

        public virtual void InitializePreLoad() {}
        public virtual void LoadContent(ContentManager content) {}
        public virtual void Initialize() {}
        public virtual void Update(GameTime gametime) {}
        public virtual void HandleInput(GameTime gametime) {}
        public virtual void Draw(GameTime gametime) {}
    }
}
