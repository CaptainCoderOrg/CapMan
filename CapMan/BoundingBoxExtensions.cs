namespace CapMan;

public static class BoundingBoxExtensions
{
    public static BoundingBox BoundingBox(this Actor actor) => new(actor.Position.X, actor.Position.Y, 1, 1);
}