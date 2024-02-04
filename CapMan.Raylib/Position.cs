public record struct Position(int Row, int Col)
{
    public Position Neighbor(Neighbors neighbor)
    {
        return neighbor switch
        {
            global::Neighbors.Up => new Position(Row - 1, Col),
            global::Neighbors.Down => new Position(Row + 1, Col),
            global::Neighbors.Left => new Position(Row, Col - 1),
            global::Neighbors.Right => new Position(Row, Col + 1),
            _ => throw new ArgumentException($"Invalid neighbor: {neighbor}"),
        };
    }
}