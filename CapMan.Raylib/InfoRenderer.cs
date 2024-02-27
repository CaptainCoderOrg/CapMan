namespace CapMan.Raylib;

using System.Numerics;

using Raylib_cs;
public class InfoRenderer
{
    public static InfoRenderer Shared { get; } = new();
    public const int Padding = 5;
    public const float Spacing = 1;
    private static Font? s_pixelplay;
    public static Font PixelPlay => s_pixelplay ??= Raylib.LoadFont(Path.Combine("assets", "fonts", "pixelplay.png"));
    public static int BlockedHeight => (PixelPlay.BaseSize + Padding) * 2;
    public static int LivesSpriteHeight => 24;

    public void Render(Game game, int top, int left)
    {
        RenderScore(game, top, left);
        RenderRemainingLives(game, top, left);
    }

    private void RenderRemainingLives(Game game, int top, int left)
    {
        top = top + game.Board.Height * BoardRenderer.CellSize + BlockedHeight + Padding;
        for (int count = 0; count < game.Lives - 1; count++)
        {
            left = left + BoardRenderer.CellSize + Padding;
            CapManRenderer.SpriteSheet.DrawSprite(0, 0, left, top, 0, true, Color.White);
        }
    }

    private void RenderScore(Game game, int top, int left)
    {
        int center = game.Board.Width * BoardRenderer.CellSize / 2;

        const string message = "HIGH SCORE";
        Vector2 hsSize = Raylib.MeasureTextEx(PixelPlay, message, PixelPlay.BaseSize, Spacing);
        Vector2 hsPos = new(left + center - hsSize.X / 2, top + Padding);

        string score = $"{game.Score}";
        Vector2 scoreSize = Raylib.MeasureTextEx(PixelPlay, score, PixelPlay.BaseSize, Spacing);
        Vector2 scorePos = new(left + center - scoreSize.X / 2, top + Padding + hsSize.Y + Padding);

        Raylib.DrawTextEx(PixelPlay, message, hsPos, PixelPlay.BaseSize, Spacing, Color.White);
        Raylib.DrawTextEx(PixelPlay, score, scorePos, PixelPlay.BaseSize, Spacing, Color.White);
    }
}