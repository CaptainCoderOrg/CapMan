namespace CapMan;

public record struct Tile(int X, int Y)
{
    public Tile Step(Direction direction) => direction switch
    {
        Direction.Up => this with { Y = Y - 1 },
        Direction.Down => this with { Y = Y + 1 },
        Direction.Left => this with { X = X - 1 },
        Direction.Right => this with { X = X + 1 },
        _ => throw new Exception($"Unknown direction {direction}"),
    };

    public Position ToPosition() => new (X, Y);
}