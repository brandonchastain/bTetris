using System;
using System.Linq;

namespace Tetris
{
    public class Piece : IDrawable
    {
        private static Random random;
        private bool[][] tiles;
        private int row;
        private int col;

        private Piece()
        {
            this.row = 1;

            // TODO: base column off of actual board width
            this.col = GetRandom().Next(0, 10);
        }

        private Piece(string shape)
        : this()
        {
            ParseCharMap(shape);
        }

        private Piece(Piece p)
        : this()
        {
            this.tiles = p.GetTiles();
        }

        public static Piece GetNextPiece()
        {
            var idx = GetRandom().Next(Pieces.Length);
            return new Piece(Pieces[idx]);
        }

        public bool[][] GetTiles()
        {
            return tiles;
        }

        public int GetCol() => col;
        public int GetRow() => row;

        public void Move(InputDirection d)
        {
            switch (d)
            {
                case InputDirection.Up:
                    this.row -= 1;
                    break;
                case InputDirection.Right:
                    this.col += 1;
                    break;
                case InputDirection.Down:
                    this.row += 1;
                    break;
                case InputDirection.Left:
                    this.col -= 1;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value for Direction type d.");
            }
        }

        public void Rotate()
        {
            // bug: pieces can be rotated out of bounds.
            // TODO: Add check for outofbounds (currently that logic is in TetrisBoard.CanPieceMoveTo).

            var width = tiles[0].Length;
            var rotated = new bool[tiles.Length][];
            for (int r = 0; r < rotated.Length; r++)
            {
                rotated[r] = new bool[width];
            }
            for (int r = 0; r < tiles.Length; r++)
            {
                for (int c = 0; c < tiles[r].Length; c++)
                {
                    var rotatedRow = (width - 1) - c;
                    var rotatedCol = r;
                    rotated[rotatedRow][rotatedCol] = tiles[r][c];
                }
            }

            this.tiles = rotated;
        }

        private static Random GetRandom()
        {
            random ??= new Random();
            return random;
        }

        private void ParseCharMap(string charMap)
        {
            var charRows = charMap.Split("\n").Where(x => !String.IsNullOrEmpty(x)).ToArray();
            tiles = new bool[charRows.Length][];

            for (int r = 0; r < charRows.Length; r++)
            {
                ParseCharMapRow(charRows, r);
            }
        }

        private void ParseCharMapRow(string[] charMapRows, int row)
        {
            if (String.IsNullOrEmpty(charMapRows[row]))
            {
                return;
            }

            // do not count newline as part of the row
            var rowLength = charMapRows[row].Length;
            if (charMapRows[row][rowLength - 1] == '\n')
            {
                rowLength -= 1;
            }

            tiles[row] = new bool[rowLength];

            for (int col = 0; col < rowLength; col++)
            {
                if (charMapRows[row][col] == 'x')
                {
                    tiles[row][col] = true;
                }
            }
        }

        private static Piece[] Pieces = new Piece[]
        {
            new Piece(
                "x  \n" +
                "x  \n" +
                "xx \n"
            ),
            new Piece
            (
                "xx \n" +
                "xx \n" +
                "   \n"
            ),
            new Piece
            (
                "xx \n" +
                "x  \n" +
                "x  \n"
            ),
            new Piece
            (
                "x  \n" +
                "xx \n" +
                "x  \n"
            ),
            new Piece
            (
                "x  \n" +
                "xx \n" +
                " x \n"
            ),
            new Piece
            (
                " x \n" +
                "xx \n" +
                "x  \n"
            ),
            new Piece
            (
                "x   \n" +
                "x   \n" +
                "x   \n" +
                "x   \n"
            )
        };
    }
}