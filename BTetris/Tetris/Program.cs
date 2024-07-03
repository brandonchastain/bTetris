using System;
using System.Threading;

namespace Tetris
{
    public class Program
    {
        private const int Height = 15;
        private const int Width = 15;

        public static void Main(string[] args)
        {
            var game = new Tetris(Width, Height);
            //ITetrisDrawer window = new TextTetrisWindow(game);

            while (true)
            {
                Thread.Sleep(17);

                // game.Update();

                // window.Draw();
            }
        }
    }
}