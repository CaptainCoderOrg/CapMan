namespace CapMan.Raylib;
using Raylib_cs;
public class InfoRenderer
{
    private static readonly int s_padding = 5;
    private static Font s_pixelplay = Raylib.LoadFont("assets/fonts/pixelplay.png");
    public static Font PixelPlay => s_pixelplay;

    public static int BlockedHeight => (s_pixelplay.BaseSize + s_padding) * 2;

    public void Render(Game game, int top, int left)
    {
        int center = game.Board.Width * BoardRenderer.CellSize / 2;
        int size = s_pixelplay.BaseSize;

        string message = "HIGH SCORE";
        string score = $"{game.Score}";

        int messageOffset = Raylib.MeasureText(message, size) / 2;
        int scoreOffset = Raylib.MeasureText(score, size) / 2;

        Raylib.DrawText(message, left + center - messageOffset, top + s_padding, size, Color.White);
        Raylib.DrawText(score, left + center - scoreOffset, top + size + s_padding, size, Color.White);
    }
}