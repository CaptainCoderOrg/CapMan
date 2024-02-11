namespace CapMan;

public record struct Position(int Row, int Col)
{
    public Position Neighbor(Neighbors neighbor)
    {
        return neighbor switch
        {
            Neighbors.Up => new Position(Row - 1, Col),
            Neighbors.Down => new Position(Row + 1, Col),
            Neighbors.Left => new Position(Row, Col - 1),
            Neighbors.Right => new Position(Row, Col + 1),
            _ => throw new ArgumentException($"Invalid neighbor: {neighbor}"),
        };
    }
}