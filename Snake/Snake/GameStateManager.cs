using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Snake
{
    public class GameStateManager
    {
        public List<GameState> states = new List<GameState>();
        
        public SpriteBatch spriteBatch;
        public GraphicsDevice graphicsDevice;
        public GraphicsDeviceManager graphicsDeviceManager;
        public ContentManager content;

        const bool showDiagnostics =
#if DEBUG
            true;
#else
            false;
#endif
        int framesPerSeccond = 0;
        int millisecondsPerFrame = 0;
        SpriteFont debugfont;
        Vector2 debugDisplayPosition = Vector2.Zero;

        public bool exit = false;

        public void AddState(GameState state)
        {
            states.Insert(0, state);
            state.graphicsDevice = graphicsDevice;
            state.graphicsDeviceManager = graphicsDeviceManager;
            state.content = content;
            state.spriteBatch = spriteBatch;
            state.manager = this;
            state.InitializePreLoad();
            state.LoadContent(content);
            state.Initialize();
        }

        public void Initialize(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            this.graphicsDeviceManager = graphics;
            this.graphicsDevice = graphics.GraphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.content = contentManager;
            debugfont = contentManager.Load<SpriteFont>(@"DebugDraw\debugfont");
        }

        public void Post(GameTime gameTime)
        {
            InputManager.Update();
            for (int i = 0; i < states.Count; ++i)
            {
                switch (states[i].transitionState)
                {
                    case GameState.GameTransitionState.on:
                        states[i].currentTransition += gameTime.ElapsedGameTime;
                        if (states[i].currentTransition >= states[i].transitionOnTime)
                        {
                            states[i].currentTransition = new TimeSpan(0, 0, 0, 0, 0);
                            states[i].transitionState = GameState.GameTransitionState.none;
                            states[i].transitionProgress = 0.0f;
                        }
                        else
                        {
                            states[i].transitionProgress = (float)(states[i].currentTransition.TotalMilliseconds / states[i].transitionOnTime.TotalMilliseconds);
                            //System.Diagnostics.Trace.WriteLine(states[i].transitionProgress);
                        }
                        break;
                    case GameState.GameTransitionState.off:
                        states[i].currentTransition += gameTime.ElapsedGameTime;
                        if (states[i].currentTransition >= states[i].transitionOffTime)
                        {
                            states[i].currentTransition = new TimeSpan(0, 0, 0, 0, 0);
                            states[i].transitionState = GameState.GameTransitionState.on;
                            states[i].transitionProgress = 1.0f;
                            states.Remove(states[i]);
                            --i;
                            continue;
                        }
                        else
                        {
                            states[i].transitionProgress = (float)(states[i].currentTransition.TotalMilliseconds / states[i].transitionOffTime.TotalMilliseconds);
                            //System.Diagnostics.Trace.WriteLine(states[i].transitionProgress);
                        }
                        break;
                    default:
                    case GameState.GameTransitionState.none:
                        break;
                }
                states[i].HandleInput(gameTime);
                if (exit) return;
                if (states[i].runningState != GameState.GameRunningState.stopped)
                {
                    states[i].Update(gameTime);
                    states[i].Draw(gameTime);
                }
            }

            if (showDiagnostics)
            {
                spriteBatch.Begin();
                framesPerSeccond = (int)(1.0 / gameTime.ElapsedGameTime.TotalSeconds);
                millisecondsPerFrame = gameTime.ElapsedGameTime.Milliseconds;
                spriteBatch.DrawString(debugfont, "Frames Per Seccond: " + framesPerSeccond.ToString() + "\nMilliseconds Per Frame: " + millisecondsPerFrame.ToString(), debugDisplayPosition, Color.Purple);
                spriteBatch.End();
            }
        }
    }
}
