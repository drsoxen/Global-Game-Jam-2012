using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Snake
{
    public class User32
    {
        [DllImport("user32.dll")]
        public static extern void SetWindowPos(uint Hwnd, uint Level, int X, int Y, int W, int H, uint Flags);
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GameStateManager manager;

        bool test = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            manager = new GameStateManager();

            IsMouseVisible = true;

            /* Set up the resolution. 
             If single monitor is false, we will put the game on the second monitor,
             assuming both monitors are the same size and the left one is primary. */
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            if (test)
            {
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 800;
            }

            WorldArena.Instance().MinPoint = Vector2.Zero;
            WorldArena.Instance().MaxPoint = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            bool singleMonitor = false;
            
            User32.SetWindowPos(
                (uint)this.Window.Handle, 0,
                singleMonitor ? 0 : graphics.PreferredBackBufferWidth, 0, 
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 
                0);
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SnakeManager.Instance().Content = Content;
            ScoreManager.SmallFont = Content.Load<SpriteFont>(@"Small");
            ScoreManager.LargeFont = Content.Load<SpriteFont>(@"Large");

            EmitterManager.Sprite = Content.Load<Texture2D>(@"Particle\Pixel");

            manager.Initialize(graphics, Content);
            manager.AddState(new MainMenuState());
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.KeyHeldDown(Keys.Escape))
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            manager.Post(gameTime);
            if (manager.exit)
                Exit();
            base.Draw(gameTime);
        }
    }
}
