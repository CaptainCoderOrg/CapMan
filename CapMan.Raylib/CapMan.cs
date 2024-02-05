public class CapMan
{
    public double X { get; set; }
    public double Y { get; set; }    
    public double Speed { get; set; } = BoardRenderer.CellSize * 6;
    public Direction CurrentDirection { get; set; } = Direction.Left;

    public void Update(double delta)
    {
        (int dX, int dY) = CurrentDirection switch
        {
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            _ => throw new Exception($"Unknown direction: {CurrentDirection}"),
        };

        X = X + (Speed * delta * dX);
        Y = Y + (Speed * delta * dY);
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}