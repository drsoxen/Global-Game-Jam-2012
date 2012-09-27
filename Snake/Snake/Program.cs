using System;

namespace Snake
{
#if WINDOWS || XBOX

    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

