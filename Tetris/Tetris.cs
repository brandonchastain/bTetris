using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetris
    {
        private TetrisState state;
        private TetrisBoard board;
        private PlayerInput playerInput;
        private Piece piece;
        private Piece nextPiece;

        public Tetris(int width, int height)
        {
            Init(width, height);
        }

        public bool IsOver => state == TetrisState.GameOver;

        public void HandlePlayerInput()
        {
            this.playerInput.HandlePlayerInput(this.piece);
        }

        public void Update()
        {
            if (state == TetrisState.Started)
            {
                GameUpdate();
            }
        }

        public void Reset()
        {
            Init(this.board.Width, this.board.Height);
        }

        public IDrawable GetDrawablePiece()
        {
            return piece;
        }

        public IDrawable GetDrawableNextPiece()
        {
            return nextPiece;
        }

        public IDrawable GetDrawableBoard()
        {
            return board;
        }

        private void Init(int width, int height)
        {
            board = new TetrisBoard(height, width);
            playerInput = new PlayerInput(board);
            piece = Piece.GetNextPiece();
            nextPiece = Piece.GetNextPiece();
            state = TetrisState.Started;
        }

        private void GameUpdate()
        {
            var rowBelow = piece.GetRow() + 1;
            if (board.CanPieceMoveTo(piece, rowBelow, piece.GetCol()))
            {
                piece.Move(InputDirection.Down);
            }
            else
            {
                board.PlacePiece(piece);

                board.ClearCompleteRows();

                GenerateNextPiece();
            }
        }

        private void GenerateNextPiece()
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