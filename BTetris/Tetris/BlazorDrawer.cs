using Blazor.Extensions.Canvas.Canvas2D;

namespace Tetris
{
    public class BlazorDrawer : ITetrisDrawer
    {
        private Canvas2DContext context;
        private Tetris game;
        private bool[][] erasePiece;
        public int TileSize = 30;

        public BlazorDrawer(Canvas2DContext context, Tetris game, bool isVertical)
        {
            this.context = context;
            this.game = game;
            this.erasePiece = new bool[5][];
            for (int i = 0; i < 5; i++)
            {
                this.erasePiece[i] = new bool[5];
            }

            this.TileSize = 30;
            if (isVertical)
            {
                this.TileSize = 35;
            }
        }

        public async ValueTask Draw()
        {
            await DrawTitle();

            IDrawable board = game.GetDrawableBoard();
            await DrawBorders(board, "white");
            await Draw(board, "green", force: true);
            await Draw(game.GetDrawablePiece(), "red");
            await DrawNextPiece(game.GetDrawableNextPiece(), board);
        }

        private async ValueTask DrawTitle()
        {
            await context.SetFillStyleAsync("black");
            await context.FillRectAsync(0, 0, 100, 200);
            await context.SetFillStyleAsync("white");
            await context.SetFontAsync("bold 48px Helvetica");
            await context.FillTextAsync("B TETRIS", 100, 200);
        }

        private async ValueTask DrawNextPiece(IDrawable p, IDrawable board)
        {
            var w = board.GetTiles()[0].Length;
            await DrawTiles(erasePiece, 0, w, "black", force: true);
            await DrawTiles(p.GetTiles(), 0, w, "blue");
        }

        private async ValueTask Draw(IDrawable p, string color, bool force = false)
        {
            await DrawTiles(p.GetTiles(), p.GetRow(), p.GetCol(), color, force);
        }

        private async ValueTask DrawTiles(bool[][] tiles, int row, int col, string color, bool force = false)
        {
            for (int r = 0; r < tiles.Length; r++)
            {
                for (int c = 0; c < tiles[r].Length; c++)
                {
                    if (tiles[r][c])
                    {
                        await context.SetFillStyleAsync(color);
                        await context.FillRectAsync((col + c) * TileSize, (row + r) * TileSize, TileSize, TileSize);
                    }
                    else if (force)
                    {
                        await context.SetFillStyleAsync("gray");
                        await context.StrokeRectAsync((col + c) * TileSize, (row + r) * TileSize, TileSize, TileSize);
                        await context.SetFillStyleAsync("black");
                        await context.FillRectAsync((col + c) * TileSize, (row + r) * TileSize, TileSize, TileSize);
                    }
                }
            }
        }

        private async ValueTask DrawBorders(IDrawable board, string color)
        {
            var tiles = board.GetTiles();
            var height = tiles.Length * TileSize;
            var width = tiles[0].Length * TileSize;
            await context.SetStrokeStyleAsync(color);
            await context.StrokeRectAsync(1, 1, width - 1, height - 1);
        }
    }
}
