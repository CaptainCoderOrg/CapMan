namespace CapMan.Raylib;
using Raylib_cs;
public class InfoRenderer
{
    private readonly int padding = 5;
    private static Font pixelplay = Raylib.LoadFont("fonts/pixelplay.png");
    public static Font PixelPlay => pixelplay;

    public static int BlockedHeight => pixelplay.BaseSize * 2;

    public void Render(Game game, int top, int left)
    {
        int size = pixelplay.BaseSize;
        Raylib.DrawText("HIGH SCORE", left + padding, top, size, Color.White);
        Raylib.DrawText($"{game.Score}", left + padding, top + size, size, Color.White);
    }
}