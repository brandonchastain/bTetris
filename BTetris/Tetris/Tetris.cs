using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Tetris
    {
        private TimeSpan tickMs;
        private DateTimeOffset lastTick = default;
        private TetrisState state;
        private TetrisBoard board;
        private PlayerInput playerInput;
        private Piece piece;
        private Piece nextPiece;
        private int score;

        public Tetris(int width, int height)
        {
            Init(width, height);
        }

        public bool IsOver => state == TetrisState.GameOver;

        public void Update(DateTimeOffset ts)
        {
            this.HandlePlayerInput();

            if (state == TetrisState.Started)
            {
                if (ts - lastTick > tickMs)
                {
                    GameUpdate();
                    lastTick = ts;
                }
            }
        }

        public void Reset()
        {
            Init(this.board.Width, this.board.Height);
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

        public int GetScore()
        {
            return score;
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
            score = 0;
            tickMs = TimeSpan.FromMilliseconds(300);
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
                
                var completedRowCount = board.ClearCompleteRows();
                this.score += completedRowCount;
                this.tickMs -= TimeSpan.FromMilliseconds(10 * completedRowCount);

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