namespace CapMan.Raylib;
using Raylib_cs;
public class InfoRenderer
{
    private readonly int padding = 5;
    public Font PixelFont = Raylib.LoadFont("");

    public const int BlockedHeight = 30;

    public void Render(Game game, int top, int left)
    {
        var half = BlockedHeight / 2;
        Raylib.DrawText("HIGH SCORE", left + padding, top, half, Color.White);
        Raylib.DrawText($"{game.Score}", left + padding, top + half, half, Color.White);
    }
}