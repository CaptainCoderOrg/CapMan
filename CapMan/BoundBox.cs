namespace CapMan;
public record struct BoundingBox(double X, double Y, double Width, double Height)
{
    public double Left => X;
    public double Top => Y;
    public double Right => X + Width;
    public double Bottom => Y + Height;
    public bool IntersectsWith(BoundingBox other) =>
        other.Left <= Right && other.Top <= Bottom &&
        Left <= other.Right && Top <= other.Bottom;
}