using System;
using System.Linq;

namespace Tetris
{
    public class Piece : IDrawable
    {
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
                "x  \n" +
                "x  \n" +
                "x  \n" +
                "x  \n"
            )
        };

        private static Random random;
        private bool[][] tiles;
        private int row;
        private int col;

        private Piece(string shape)
        {
            random = new Random();
            ParseCharMap(shape);
        }

        private Piece(Piece p)
        {
            this.tiles = p.GetTiles();
        }

        public static Piece GetNextPiece()
        {
            var idx = random.Next(Pieces.Length);
            return new Piece(Pieces[idx]);
        }

        public bool[][] GetTiles()
        {
            return tiles;
        }

        public int GetCol() => col;
        public int GetRow() => row;

        public void Move(Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    this.row -= 1;
                    break;
                case Direction.Right:
                    this.col += 1;
                    break;
                case Direction.Down:
                    this.row += 1;
                    break;
                case Direction.Left:
                    this.col -= 1;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value for Direction type d.");
            }
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
    }
}