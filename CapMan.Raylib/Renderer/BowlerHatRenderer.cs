namespace CapMan.Raylib;

using Raylib_cs;

public class BowlerHatRenderer
{
    public void Render(Projectile? projectile, int left, int top)
    {
        if (projectile is null) { return; }
        int x = (int)(projectile.Position.X * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        int y = (int)(projectile.Position.Y * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        Raylib.DrawCircle(left + x, top + y, BoardRenderer.CellSize / 2, Color.Red);
    }
}