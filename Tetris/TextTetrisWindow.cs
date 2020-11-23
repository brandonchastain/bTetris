using System;

namespace Tetris
{
    public class TextTetrisWindow : ITetrisDrawer
    {
        private Tetris game;
        private bool printedGameOver;

        public TextTetrisWindow(Tetris game)
        {
            this.game = game;
        }

        public void Draw()
        {
            if (!this.game.IsOver)
            {
                DrawGame();
            }
            else if (!printedGameOver)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("GAME OVER");
                Console.WriteLine("Press enter to play again.");
                printedGameOver = true;
            }
            else
            {
                Console.ReadLine();
                game.Reset();
                printedGameOver = false;
            }
        }

        private void DrawGame()
        {
            Console.Clear();

            IDrawable board = game.GetDrawableBoard();
            WriteBorders(board);
            Draw(board);
            Draw(game.GetDrawablePiece());
            DrawNextPiece(game.GetDrawableNextPiece(), board);

            Console.SetCursorPosition(board.GetTiles().Length, board.GetTiles()[0].Length);
        }

        private void DrawNextPiece(IDrawable p, IDrawable board)
        {
            var w = board.GetTiles()[0].Length;
            WriteTiles(p.GetTiles(), 0, w + 2);
        }

        private void Draw(IDrawable p)
        {
            WriteTiles(p.GetTiles(), p.GetRow(), p.GetCol());
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

        private void WriteBorders(IDrawable board)
        {
            var tiles = board.GetTiles();
            var height = tiles.Length;
            var width = tiles[0].Length;

            for (int r = 0; r < height; r++)
            {
                var rightEdge = width;
                WriteAt("|", r, rightEdge);
            }

            for (int c = 0; c < width; c++)
            {
                var bottomEdge = height;
                WriteAt("-", bottomEdge, c);
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