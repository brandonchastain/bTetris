using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetris
    {
        private const int gameUpdateTicks = 10;

        private int ticks;
        private TetrisState state;
        private TetrisBoard board;
        private Piece piece;
        private Piece nextPiece;

        public Tetris(int width, int height)
        {
            board = new TetrisBoard(height, width);
            piece = Piece.GetNextPiece();
            nextPiece = Piece.GetNextPiece();
            state = TetrisState.Started;
        }

        public IEnumerable<IDrawable> GetDrawables()
        {
            yield return board;
            yield return piece;
            // yield return NextPiece;
        }

        public void Update()
        {
            if (state == TetrisState.Started)
            {
                GameUpdate();
            }
        }

        private void GameUpdate()
        {
            var rowBelow = piece.GetRow() + 1;
            if (board.CanPieceMoveTo(piece, rowBelow, piece.GetCol()))
            {
                piece.Move(Direction.Down);
            }
            else
            {
                board.PlacePiece(piece);
                GetNextPiece();
            }
        }

        private void GetNextPiece()
        {
            piece = nextPiece;
            nextPiece = Piece.GetNextPiece();
            CheckForGameOver();
        }

        private void CheckForGameOver()
        {
            if (!board.CanPieceMoveTo(piece, piece.GetRow(), piece.GetCol()))
            {
                state = TetrisState.GameOver;
            }
        }

        private enum TetrisState
        {
            Init,
            Started,
            GameOver
        }
    }
}