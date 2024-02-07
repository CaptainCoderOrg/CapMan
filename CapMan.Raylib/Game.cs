using System.Security.Cryptography;

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
        Direction next = NextDirection(Player.CurrentDirection, Player.NextDirection, Board, Player.X, Player.Y, distance);
        if (next == Player.CurrentDirection)
        {
            (Player.X, Player.Y) = CalculateMove(Board, Player.CurrentDirection, Player.X, Player.Y, distance);
        }
        else
        {
            // If I change direction, this means I am passing a threshold
            // I need to determine how much to move in my previous direction
            // And how much to move in my new direction
            SwitchDirection(Player, distance);
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

    private void SwitchDirection(CapMan actor, double distance)
    {
        if (actor.CurrentDirection.IsOpposite(actor.NextDirection))
        {
            (actor.X, actor.Y) = CalculateMove(Board, actor.CurrentDirection, actor.X, actor.Y, distance);
        }
        else if (actor.CurrentDirection is Direction.Up)
        {
            // 10.1 => 10
            double start = actor.Y;
            actor.Y = (int)actor.Y;
            double leftOverDistance = start - actor.Y;
            actor.X += actor.NextDirection is Direction.Left ? -leftOverDistance : leftOverDistance;
        }
        else if (actor.CurrentDirection is Direction.Down)
        {
            // 10.1 => 10
            double start = actor.Y;
            actor.Y = (int)Math.Ceiling(actor.Y);
            double leftOverDistance = actor.Y - start;
            actor.X += actor.NextDirection is Direction.Left ? -leftOverDistance : leftOverDistance;
        }
        else if (actor.CurrentDirection is Direction.Right)
        {
            // 10.1 => 10
            double start = actor.X;
            actor.X = (int)Math.Ceiling(actor.X);
            double leftOverDistance = actor.X - start;
            actor.Y += actor.NextDirection is Direction.Up ? -leftOverDistance : leftOverDistance;
        }
        else if (actor.CurrentDirection is Direction.Left)
        {
            // 10.1 => 10
            double start = actor.X;
            actor.X = (int)actor.X;
            double leftOverDistance = start - actor.X;
            actor.Y += actor.NextDirection is Direction.Up ? -leftOverDistance : leftOverDistance;
        }
        else
        {
            throw new Exception("Invalid state.");
        }

        actor.CurrentDirection = actor.NextDirection;
    }

    public static Direction NextDirection(Direction current, Direction next, Board board, double x, double y, double distance)
    {
        // You can always turn around / continue in the same direction
        if (current == next || current.IsOpposite(next)) { return next; }
        
        (double endX, double endY) = CalculateMove(board, current, x, y, distance);

        bool isCrossingCenter = current switch
        {
            Direction.Up => (int)y == (int)Math.Ceiling(endY),
            Direction.Down => (int)Math.Ceiling(y) == (int)endY,
            Direction.Right => (int)Math.Ceiling(x) == (int)endX,
            Direction.Left => (int)x == (int)Math.Ceiling(endX),
            _ => throw new Exception($"Unknown direction {current}"),
        };

        // If this move would not cross the center of a tile
        // it is not possible to turn.
        if (!isCrossingCenter) { return current; }

        // Check possible 90 degree turns
        (int row, int col) = (current, next) switch
        {
            (Direction.Up, Direction.Left) => ((int)Math.Ceiling(endY), (int)x - 1),
            (Direction.Up, Direction.Right) => ((int)Math.Ceiling(endY), (int)x + 1),
            (Direction.Down, Direction.Left) => ((int)endY, (int)x - 1),
            (Direction.Down, Direction.Right) => ((int)endY, (int)x + 1),
            (Direction.Right, Direction.Up) => ((int)y - 1, (int)endX),
            (Direction.Right, Direction.Down) => ((int)y + 1, (int)endX),
            (Direction.Left, Direction.Up) => ((int)y - 1, (int)Math.Ceiling(endX)),
            (Direction.Left, Direction.Down) => ((int)y + 1, (int)Math.Ceiling(endX)),
            _ => throw new Exception($"Unknown 90 degree turn: {current} to {next}"),
        };
        
        // If there is a wall or if we are trying to move out of the board, keep going 
        // in the current direction
        if (!board.Contains(row, col) || board.IsWall(row, col)) { return current; }

        // Otherwise, change directions
        return next;
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