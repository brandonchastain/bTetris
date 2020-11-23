using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tetris
{
    public class PlayerInput
    {
        private TetrisBoard board;

        public PlayerInput(TetrisBoard board)
        {
            this.board = board ?? throw new ArgumentNullException(nameof(board));
        }

        public void HandlePlayerInput(Piece currentPlayerPiece)
        {
            var inputDir = this.DetectInputDirection();
            switch (inputDir)
            {
                case InputDirection.Up:
                    currentPlayerPiece.Rotate();
                    break;
                default:
                    TryMovePiece(currentPlayerPiece, inputDir);
                    break;
            }
        }

        private InputDirection DetectInputDirection()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(false);

                // Clear the input buffer
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(false);
                }

                switch (key.Key)
                {
                    case (ConsoleKey.UpArrow):
                        return InputDirection.Up;

                    case (ConsoleKey.RightArrow):
                        return InputDirection.Right;

                    case (ConsoleKey.DownArrow):
                        return InputDirection.Down;

                    case (ConsoleKey.LeftArrow):
                        return InputDirection.Left;

                    default:
                        return InputDirection.None;
                }
            }

            return InputDirection.None;
        }

        private void TryMovePiece(Piece piece, InputDirection dir)
        {
            var row = piece.GetRow();
            var col = piece.GetCol();

            switch (dir)
            {
                case InputDirection.Right:
                    col++;
                    break;
                case InputDirection.Left:
                    col--;
                    break;
                case InputDirection.Down:
                    row++;
                    break;
                case InputDirection.Up:
                default:
                    break;
            }

            if (dir != InputDirection.None && board.CanPieceMoveTo(piece, row, col))
            {
                piece.Move(dir);
            }
        }
    }
}
