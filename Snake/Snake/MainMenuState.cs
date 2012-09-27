using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Snake
{
    class MainMenuState : GameState
    {
        struct entry
        {
            public entry(string text, Vector2 position, GameState leading)
            {
                this.text = text;
                this.position = position;
                this.leading = leading;
            }
            public string text;
            public Vector2 position;
            public GameState leading;
        }

        SpriteFont entryfont;
        Texture2D background;
        entry[] entries;
        int selected = 0;

        
        public override void LoadContent(ContentManager content)
        {
            entryfont = content.Load<SpriteFont>(@"menu\menu_entry_font");
            background = content.Load<Texture2D>(@"menu\bg");
        }

        public override void Initialize()
        {
            transitionOffTime = new TimeSpan(0, 0, 0, 1, 0);
            int height = (int)entryfont.MeasureString("Hg").Y;

            entry join = new entry("Join Game", new Vector2(1920 / 2, 120), new PlayState());
            entry cust = new entry("Customize", new Vector2(1920 / 2, 120), new CustomizeState());
            entry cred = new entry("Credits", new Vector2(1920 / 2, 120), new CreditState());
            entry quit = new entry("Quit", new Vector2(1920 / 2, 120), null);

            entries = new entry[]
            {
                join, cust, cred, quit
            };
            
            int currenty = (int)entries[0].position.Y;
            int spacing = 100;

            for (int i = 0; i < entries.Length; ++i)
            {
                Vector2 strsize = entryfont.MeasureString(entries[i].text);
                entries[i].position.X -= (int)strsize.X / 2;
                entries[i].position.Y = currenty;
                currenty += spacing;
            }
        }

        public override void Update(GameTime gametime)
        {
        }

        public override void HandleInput(GameTime gametime)
        {
            if (this.transitionState == GameTransitionState.off)
                return;

            if (InputManager.SingleKeyPressInput(Microsoft.Xna.Framework.Input.Keys.Enter) ||
                InputManager.SingleKeyPressInput(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                if (entries[selected].leading == null)
                {
                    manager.exit = true;
                    return;
                }
                else
                {
                    manager.AddState(entries[selected].leading);
                    this.transitionState = GameState.GameTransitionState.off;
                    return;
                }
            }

            if (InputManager.SingleKeyPressInput(Microsoft.Xna.Framework.Input.Keys.Down) ||
               InputManager.SingleKeyPressInput(Microsoft.Xna.Framework.Input.Keys.S))
                ++selected;
            if(InputManager.SingleKeyPressInput(Microsoft.Xna.Framework.Input.Keys.Up) ||
                InputManager.SingleKeyPressInput(Microsoft.Xna.Framework.Input.Keys.W))
                --selected;
            if (selected < 0)
                selected = entries.Length - 1;
            if (selected >= entries.Length)
                selected = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Color white = new Color(1.0f, 1.0f, 1.0f, transitionState == GameTransitionState.off ? 1.0f - transitionProgress : 1.0f);
            
            spriteBatch.Draw(background, background.Bounds, white);
            for (int i = 0; i < entries.Length; ++i)
            {
                Color fontc;
                if (i == selected)
                    fontc = new Color(0.5f, 0.0f, 0.0f, transitionState == GameTransitionState.off ? 1.0f - transitionProgress : 1.0f);
                else
                    fontc = new Color(0.8f, 0.8f, 0.8f, transitionState == GameTransitionState.off ? 1.0f - transitionProgress : 1.0f);
                spriteBatch.DrawString(entryfont, entries[i].text, entries[i].position, fontc);
            }
            spriteBatch.End();
        }
    }
}
