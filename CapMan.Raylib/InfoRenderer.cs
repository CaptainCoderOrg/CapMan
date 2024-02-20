namespace CapMan.Raylib;
using Raylib_cs;
public class InfoRenderer
{
    private static readonly int padding = 5;
    private static Font pixelplay = Raylib.LoadFont("assets/fonts/pixelplay.png");
    public static Font PixelPlay => pixelplay;

    public static int BlockedHeight => (pixelplay.BaseSize + padding) * 2;

    public void Render(Game game, int top, int left)
    {
        var center = game.Board.Width * BoardRenderer.CellSize / 2;
        int size = pixelplay.BaseSize;

        string message = "HIGH SCORE";
        string score = $"{game.Score}";

        var messageOffset = Raylib.MeasureText(message, size) / 2;
        var scoreOffset = Raylib.MeasureText(score, size) / 2;

        Raylib.DrawText(message, left + center - messageOffset, top + padding, size, Color.White);
        Raylib.DrawText(score, left + center - scoreOffset, top + size + padding, size, Color.White);
    }
}