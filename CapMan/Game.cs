public class Game
{
    public CapMan Player { get; } = new CapMan();
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
            (Player.X, Player.Y) = CalculateMove(Board, Player.CurrentDirection, Player.X, Player.Y, distance);
        }
        else
        {
            (Player.X, Player.Y) = CalculateTurn(Player.CurrentDirection, Player.NextDirection, Board, Player.X, Player.Y, distance);
            Player.CurrentDirection = nextDirection;
        }

        BoundsCheck(Player);
    }

    public void BoundsCheck(CapMan actor)
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
        if (current == next) { return CalculateMove(board, current, x, y, distance); }
        if (current.IsOpposite(next)) { return CalculateMove(board, next, x, y, distance); }

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
        (double endX, double endY) = CalculateMove(board, currentDir, x, y, distance);
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

    public static (double, double) CalculateMove(Board board, Direction moving, double x, double y, double distance)
    {
        (double nextX, double nextY) = moving switch
        {
            Direction.Right => (x + distance, y),
            Direction.Left => (x - distance, y),
            Direction.Up => (x, y - distance),
            Direction.Down => (x, y + distance),
            _ => throw new Exception($"Unknown direction {moving}."),
        };

        (int nextCol, int nextRow) = moving switch
        {
            Direction.Right => ((int)Math.Ceiling(nextX), (int)nextY),
            Direction.Left => ((int)nextX, (int)nextY),
            Direction.Up => ((int)nextX, (int)nextY),
            Direction.Down => ((int)nextX, (int)Math.Ceiling(nextY)),
            _ => throw new Exception($"Unknown direction {moving}."),
        };

        // If there is no wall, return move
        if (!board.IsWall(nextRow, nextCol)) { return (nextX, nextY); }

        // Otherwise, snap to the specific wall
        return moving switch
        {
            Direction.Right => (nextCol - 1, nextY),
            Direction.Left => (nextCol + 1, nextY),
            Direction.Up => (nextX, nextRow + 1),
            Direction.Down => (nextX, nextRow - 1),
            _ => throw new Exception($"Unknown direction {moving}."),
        };
    }
}