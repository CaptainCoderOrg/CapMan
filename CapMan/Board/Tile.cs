namespace CapMan;

public record struct Tile(int X, int Y)
{
    public Tile Step(Direction direction) => Step(direction, 1);
    public Tile Step(Direction direction, int distance) => direction switch
    {
        Direction.Up => this with { Y = Y - distance },
        Direction.Down => this with { Y = Y + distance },
        Direction.Left => this with { X = X - distance },
        Direction.Right => this with { X = X + distance },
        _ => throw new Exception($"Unknown direction {direction}"),
    };

    public Position ToPosition() => new(X, Y);

    public int ManhattanDistance(Tile other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
}