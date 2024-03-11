using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tetris
{
    public class PlayerInput
    {
        private TetrisBoard board;
        private InputDirection lastInput = InputDirection.None;

        public PlayerInput(TetrisBoard board)
        {
            this.board = board ?? throw new ArgumentNullException(nameof(board));
        }

        public void QueueInput(string keyCode)
        {
            InputDirection direction;
            switch (keyCode)
            {
                case "ArrowUp":
                    direction = InputDirection.Up;
                    break;
                case "ArrowRight":
                    direction = InputDirection.Right;
                    break;
                case "ArrowDown":
                    direction = InputDirection.Down;
                    break;
                case "ArrowLeft":
                    direction = InputDirection.Left;
                    break;
                default:
                    direction = InputDirection.None;
                    break;
            }

            this.lastInput = direction;
        }

        public void HandlePlayerInput(Piece currentPlayerPiece)
        {
            var inputDir = this.lastInput;
            this.lastInput = InputDirection.None;

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
