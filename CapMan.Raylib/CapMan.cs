using Raylib_cs;

public class CapMan
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; } = 6;
    public Direction CurrentDirection { get; set; } = Direction.Left;
    public Direction NextDirection { get; set; } = Direction.Left;

    public void Update(Game game, double delta)
    {
        UpdateDirection(delta);
        int nextRow = NextRow(delta);
        int nextCol = NextColumn(delta);

        // If we would hit a wall, 
        if (game.Board.IsWall(nextRow, nextCol)) 
        { 
            X = nextCol - Velocity.X;
            Y = nextRow - Velocity.Y;
            return; 
        }

        X = NextX(delta);
        Y = NextY(delta);

        // Collision Check
        // If hitting wall, do not move
        // int row = CurrentDirection is Direction.Up ? (int)nextY : (int)Math.Ceiling(nextY);
        // int col = CurrentDirection is Direction.Left ? (int)nextX : (int)Math.Ceiling(nextX);
        // int row = (int)nextY;
        
    }

    private void UpdateDirection()
    {
        
    }

    public double NextX(double delta) => X + (Speed * delta * Velocity.X);
    public double NextY(double delta) => Y + (Speed * delta * Velocity.Y);

    public int NextRow(double delta)
    {
        return CurrentDirection switch
        {
            Direction.Right or Direction.Left => Row,
            Direction.Down => (int)Math.Ceiling(NextY(delta)),
            Direction.Up => (int)NextY(delta),
            _ => throw new Exception($"Unknown direction: {CurrentDirection}"),
        };
    }

    public int NextColumn(double delta)
    {
        return CurrentDirection switch
        {
            Direction.Up or Direction.Down => Column,
            Direction.Right => (int)Math.Ceiling(NextX(delta)),
            Direction.Left => (int)NextX(delta),
            _ => throw new Exception($"Unknown direction: {CurrentDirection}"),
        };
    }

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

    public (int X, int Y) Velocity => GetVelocity(CurrentDirection);
    private (int X, int Y) GetVelocity(Direction direction) => direction switch
    {
        Direction.Left => (-1, 0),
        Direction.Right => (1, 0),
        Direction.Up => (0, -1),
        Direction.Down => (0, 1),
        _ => throw new Exception($"Unknown direction: {CurrentDirection}"),
    };
}