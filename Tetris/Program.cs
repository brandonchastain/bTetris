using System;
using System.Threading;

namespace Tetris
{
    public class Program
    {
        private const int Height = 25;
        private const int Width = 20;
        private const int GameUpdateTicks = 10;

        public static void Main(string[] args)
        {
            var game = new Tetris(Width, Height);
            ITetrisDrawer window = new TextTetrisWindow(game);
            int ticks = 0;

            while (true)
            {

                Thread.Sleep(17);

                game.HandlePlayerInput();

                if (ticks > GameUpdateTicks)
                {
                    game.Update();
                    ticks = 0;
                }

                window.Draw();

                ticks++;
            }
        }
    }
}