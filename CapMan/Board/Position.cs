namespace CapMan;

public record struct Position(double X, double Y)
{
    /// <summary>
    /// Given the direction of movement, returns the current BoardPosition that
    /// is being occupied.
    /// </summary>
    public Tile CurrentTile(Direction direction) => direction switch
    {
        Direction.Up => new((int)X, (int)Math.Ceiling(Y)),
        Direction.Left => new((int)Math.Ceiling(X), (int)Y),
        _ => new((int)X, (int)Y),
    };

    /// <summary>
    /// Given the direction of movement, returns the BoardPosition that is
    /// directly ahead of this Position.
    /// </summary>
    public Tile NextTile(Direction direction) => direction switch
    {
        Direction.Right => new((int)Math.Ceiling(X), (int)Math.Floor(Y)),
        Direction.Down => new((int)Math.Floor(X), (int)Math.Ceiling(Y)),
        _ => new((int)Math.Floor(X), (int)Math.Floor(Y)),
    };

    /// <summary>
    /// Returns a Position that is the specified distance away in the specified
    /// Direction.
    /// </summary>
    public Position Move(Direction direction, double distance)
    {
        return direction switch
        {
            Direction.Right => this with { X = X + distance },
            Direction.Left => this with { X = X - distance },
            Direction.Up => this with { Y = Y - distance },
            Direction.Down => this with { Y = Y + distance },
            _ => throw new Exception($"Unknown direction {direction}."),
        };
    }
}