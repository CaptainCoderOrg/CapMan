namespace CapMan.Raylib;

using Raylib_cs;

public static class BoundingBoxExtensions
{
    public static void Render(this CapMan.BoundingBox box, Color color)
    {
        Raylib.DrawRectangleLinesEx(new Rectangle((float)box.X, (float)box.Y, (float)box.Width, (float)box.Height), 1, color);
    }

    public static CapMan.BoundingBox ToBoard(this CapMan.BoundingBox box) =>
        new(box.X * BoardRenderer.CellSize, box.Y * BoardRenderer.CellSize, BoardRenderer.CellSize, BoardRenderer.CellSize);
}