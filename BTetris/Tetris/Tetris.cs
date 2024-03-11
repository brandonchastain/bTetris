using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetris
    {
        private int gameUpdateTicks = 7;

        private TetrisState state;
        private TetrisBoard board;
        private PlayerInput playerInput;
        private Piece piece;
        private Piece nextPiece;
        private int ticks = 0;

        public Tetris(int width, int height)
        {
            Init(width, height);
        }

        public bool IsOver => state == TetrisState.GameOver;

        public void Update()
        {
            this.HandlePlayerInput();

            if (state == TetrisState.Started)
            {
                if (ticks > gameUpdateTicks)
                {
                    GameUpdate();
                    ticks = 0;
                }
            }

            ticks++;
        }

        public void Reset()
        {
            Init(this.board.Width, this.board.Height);
        }

        public void Resize(int w, int h)
        {
            Init(w / BlazorDrawer.TileSize, h / BlazorDrawer.TileSize);
        }

        public void SendKeyDown(string keyCode)
        {
            this.playerInput.QueueInput(keyCode);
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

        public void Start()
        {
            state = TetrisState.Started;
        }

        private void Init(int width, int height)
        {
            board = new TetrisBoard(height, width);
            playerInput = new PlayerInput(board);
            piece = Piece.GetNextPiece();
            nextPiece = Piece.GetNextPiece();
        }

        private void HandlePlayerInput()
        {
            this.playerInput.HandlePlayerInput(this.piece);
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
                var didCompleteRow = board.ClearCompleteRows();
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