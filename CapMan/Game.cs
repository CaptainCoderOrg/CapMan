public class Game
{
    public Actor Player { get; } = new Actor();
    public Board Board { get; } = new Board(Board.StandardBoard);
    public int Score { get; private set; }

    public void Update(double delta)
    {
        UpdateCapMan(delta);
        CheckEatDots();
    }

    private void CheckEatDots()
    {
        if (Board.IsDot(Player.Row, Player.Column))
        {
            Board.RemoveElement(Player.Row, Player.Column);
            Score += 10;
        }

        if (Board.IsPowerPill(Player.Row, Player.Column))
        {
            Board.RemoveElement(Player.Row, Player.Column);
            Score += 50;
        }
    }

    private void UpdateCapMan(double delta)
    {
        double distance = Player.Speed * delta;
        Direction nextDirection = NextDirection(Player.CurrentDirection, Player.NextDirection, Board, Player.X, Player.Y, distance);
        if (nextDirection == Player.CurrentDirection)
        {
            (Player.X, Player.Y) = Board.CalculateMove(Player.CurrentDirection, Player.X, Player.Y, distance);
        }
        else
        {
            (Player.X, Player.Y) = CalculateTurn(Player.CurrentDirection, Player.NextDirection, Board, Player.X, Player.Y, distance);
            Player.CurrentDirection = nextDirection;
        }

        BoundsCheck(Player);
    }

    public void BoundsCheck(Actor actor)
    {
        double transitionDistance = 1;
        double width = Board.Columns;
        if (actor.X < -transitionDistance)
        {
            actor.X += width + 2 * transitionDistance;
        }
        else if (actor.X > width + transitionDistance)
        {
            actor.X -= width + 2 * transitionDistance;
        }
    }

    public static (double, double) CalculateTurn(Direction current, Direction next, Board board, double x, double y, double distance)
    {
        // This method assumes the move is valid and the distance <= 1
        if (current == next) { return board.CalculateMove(current, x, y, distance); }
        if (current.IsOpposite(next)) { return board.CalculateMove(next, x, y, distance); }

        (int col, int row) = current switch
        {
            Direction.Down => ((int)x, (int)Math.Ceiling(y)),
            Direction.Right => ((int)Math.Ceiling(x), (int)y),
            Direction.Up or Direction.Left => ((int)x, (int)y),
            _ => throw new Exception($"Unknown direction: {current}"),
        };

        return next switch
        {
            Direction.Left => (col + Math.Abs(x - col), row),
            Direction.Right => (col - Math.Abs(x - col), row),
            Direction.Up => (col, row + Math.Abs(y - row)),
            Direction.Down => (col, row - Math.Abs(y - row)),
            _ => throw new Exception($"Unknown direction: {next}"),
        };
    }

    public static Direction NextDirection
        (Direction currentDir, Direction nextDir, 
        Board board, double x, double y, double distance)
    {
        // You can always turn around / continue in the same direction
        if (currentDir == nextDir || currentDir.IsOpposite(nextDir)) { return nextDir; }
        // Calculate the position, if you move straight
        (double endX, double endY) = board.CalculateMove(currentDir, x, y, distance);
        bool isCrossingCenter = currentDir switch
        {
            Direction.Up => (int)y == (int)Math.Ceiling(endY),
            Direction.Down => (int)Math.Ceiling(y) == (int)endY,
            Direction.Right => (int)Math.Ceiling(x) == (int)endX,
            Direction.Left => (int)x == (int)Math.Ceiling(endX),
            _ => throw new Exception($"Unknown direction {currentDir}"),
        };

        // If this move would not cross the center of a tile
        // it is not possible to turn.
        if (!isCrossingCenter) { return currentDir; }

        // Check possible 90 degree turns
        (int row, int col) = (currentDir, nextDir) switch
        {
            (Direction.Up, Direction.Left) => ((int)Math.Ceiling(endY), (int)x - 1),
            (Direction.Up, Direction.Right) => ((int)Math.Ceiling(endY), (int)x + 1),
            (Direction.Down, Direction.Left) => ((int)endY, (int)x - 1),
            (Direction.Down, Direction.Right) => ((int)endY, (int)x + 1),
            (Direction.Right, Direction.Up) => ((int)y - 1, (int)endX),
            (Direction.Right, Direction.Down) => ((int)y + 1, (int)endX),
            (Direction.Left, Direction.Up) => ((int)y - 1, (int)Math.Ceiling(endX)),
            (Direction.Left, Direction.Down) => ((int)y + 1, (int)Math.Ceiling(endX)),
            _ => throw new Exception($"Unknown 90 degree turn: {currentDir} to {nextDir}"),
        };

        // If there is a wall or if we are trying to move out of the board, keep going 
        // in the current direction
        if (!board.Contains(row, col) || board.IsWall(row, col)) { return currentDir; }

        // Otherwise, change directions
        return nextDir;
    }

    
}