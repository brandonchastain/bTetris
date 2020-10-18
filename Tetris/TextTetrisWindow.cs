using System;

namespace Tetris
{
    public class TextTetrisWindow : ITetrisDrawer
    {
        private Tetris game;

        public TextTetrisWindow(Tetris game)
        {
            this.game = game;
        }

        public void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            foreach (var drawable in game.GetDrawables())
            {
                WriteTiles(drawable.GetTiles(), drawable.GetRow(), drawable.GetCol());
            }
        }

        private void WriteTiles(bool[][] tiles, int row, int col)
        {
            for (int r = 0; r < tiles.Length; r++)
            {
                for (int c = 0; c < tiles[r].Length; c++)
                {
                    if (tiles[r][c])
                    {
                        WriteAt("x", r + row, c + col);
                    }
                }
            }
        }

        // wraps around edges, but throws exception at top or bottom boundaries
        private void WriteAt(string s, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(s);
        }
    }
}