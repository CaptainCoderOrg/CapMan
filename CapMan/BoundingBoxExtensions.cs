namespace CapMan;

public static class BoundingBoxExtensions
{
    public static BoundingBox BoundingBox(this IActor actor) => new(actor.Position.X, actor.Position.Y, 1, 1);
    public static BoundingBox BoundingBox(this Tile tile) => new(tile.X, tile.Y, 1, 1);
}