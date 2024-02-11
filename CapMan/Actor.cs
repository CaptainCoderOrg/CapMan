namespace CapMan;

public class Actor
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; } = 8;
    public Direction CurrentDirection { get; set; } = Direction.Left;
    public Direction NextDirection { get; set; } = Direction.Left;

    public int Column => CurrentDirection switch
    {
        Direction.Left => (int)Math.Ceiling(X),
        _ => (int)X,
    };

    public int Row => CurrentDirection switch
    {
        Direction.Up => (int)Math.Ceiling(Y),
        _ => (int)Y,
    };
}