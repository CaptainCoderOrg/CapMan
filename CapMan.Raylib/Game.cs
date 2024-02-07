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
        if(Board.IsDot(Player.Row, Player.Column))
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
        Func<CapMan, double, double> NextX = Player.CurrentDirection switch
        {
            Direction.Left => MoveLeft,
            Direction.Right => MoveRight,
            _ => (actor, _) => actor.Column,
        };

        Func<CapMan, double, double> NextY = Player.CurrentDirection switch
        {
            Direction.Up => MoveUp,
            Direction.Down => MoveDown,
            _ => (actor, _) => actor.Row,
        };

        double distance = Player.Speed * delta;
        Direction next = NextDirection(Player, distance);
        if (next == Player.CurrentDirection)
        {
            Player.X = NextX(Player, distance);
            Player.Y = NextY(Player, distance);
        }
        else
        {
            // If I change direction, this means I am passing a threshold
            // I need to determine how much to move in my previous direction
            // And how much to move in my new direction
            SwitchDirection(Player, distance);
        }
    }

    private void SwitchDirection(CapMan actor, double distance)
    {
        if (actor.CurrentDirection is Direction.Up && actor.NextDirection is Direction.Down)
        {
            actor.Y = MoveDown(actor, distance);
        }
        else if (actor.CurrentDirection is Direction.Down && actor.NextDirection is Direction.Up)
        {
            actor.Y = MoveUp(actor, distance);
        }
        else if (actor.CurrentDirection is Direction.Left && actor.NextDirection is Direction.Right)
        {
            actor.X = MoveRight(actor, distance);
        }
        else if (actor.CurrentDirection is Direction.Right && actor.NextDirection is Direction.Left)
        {
            actor.X = MoveLeft(actor, distance);
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

    private Direction NextDirectionIfGoingUp(CapMan actor, double distance)
    {
        if (actor.NextDirection is Direction.Down or Direction.Up) { return actor.NextDirection; }
        double endPosition = actor.Y - distance;
        if (((int)actor.Y) != ((int)Math.Ceiling(endPosition))) { return actor.CurrentDirection; }
        (int row, int col) = actor.NextDirection switch
        {
            Direction.Left => ((int)Math.Ceiling(endPosition), (int)(actor.X - 1)),
            Direction.Right => ((int)Math.Ceiling(endPosition), (int)(actor.X + 1)),
            _ => throw new Exception($"Invalid directino (spanish for direction): {actor.NextDirection}"),
        };
        if (Board.IsWall(row, col)) { return actor.CurrentDirection; }
        return actor.NextDirection;
    }

    private Direction NextDirectionIfGoingDown(CapMan actor, double distance)
    {
        if (actor.NextDirection is Direction.Down or Direction.Up) { return actor.NextDirection; }
        double endPosition = actor.Y + distance;
        if (((int)Math.Ceiling(actor.Y)) != ((int)endPosition)) { return actor.CurrentDirection; }
        (int row, int col) = actor.NextDirection switch
        {
            Direction.Left => ((int)endPosition, (int)(actor.X - 1)),
            Direction.Right => ((int)endPosition, (int)(actor.X + 1)),
            _ => throw new Exception($"Invalid directino (spanish for direction): {actor.NextDirection}"),
        };
        if (Board.IsWall(row, col)) { return actor.CurrentDirection; }
        return actor.NextDirection;
    }

    private Direction NextDirectionIfGoingRight(CapMan actor, double distance)
    {
        if (actor.NextDirection is Direction.Left or Direction.Right) { return actor.NextDirection; }
        double endPosition = actor.X + distance;
        if (((int)Math.Ceiling(actor.X)) != ((int)endPosition)) { return actor.CurrentDirection; }
        (int row, int col) = actor.NextDirection switch
        {
            Direction.Up => ((int)(actor.Y - 1), (int)endPosition),
            Direction.Down => ((int)(actor.Y + 1), (int)endPosition),
            _ => throw new Exception($"Invalid directino (spanish for direction): {actor.NextDirection}"),
        };
        if (Board.IsWall(row, col)) { return actor.CurrentDirection; }
        return actor.NextDirection;
    }

    private Direction NextDirectionIfGoingLeft(CapMan actor, double distance)
    {
        if (actor.NextDirection is Direction.Left or Direction.Right) { return actor.NextDirection; }
        double endPosition = actor.X - distance;
        if (((int)actor.X) != ((int)Math.Ceiling(endPosition))) { return actor.CurrentDirection; }
        (int row, int col) = actor.NextDirection switch
        {
            Direction.Up => ((int)(actor.Y - 1), (int)Math.Ceiling(endPosition)),
            Direction.Down => ((int)(actor.Y + 1), (int)Math.Ceiling(endPosition)),
            _ => throw new Exception($"Invalid directino (spanish for direction): {actor.NextDirection}"),
        };
        if (Board.IsWall(row, col)) { return actor.CurrentDirection; }
        return actor.NextDirection;
    }

    public Direction NextDirection(CapMan actor, double distance)
    {
        if (actor.NextDirection == actor.CurrentDirection) { return actor.CurrentDirection; }

        if (actor.CurrentDirection is Direction.Up)
        {
            return NextDirectionIfGoingUp(actor, distance);
        }
        else if (actor.CurrentDirection is Direction.Down)
        {
            return NextDirectionIfGoingDown(actor, distance);
        }
        else if (actor.CurrentDirection is Direction.Right)
        {
            return NextDirectionIfGoingRight(actor, distance);
        }
        else if (actor.CurrentDirection is Direction.Left)
        {
            return NextDirectionIfGoingLeft(actor, distance);
        }
        else
        {
            throw new Exception($"Invalid current direction: {actor.CurrentDirection}");
        }
    }


    public double MoveUp(CapMan actor, double distance)
    {
        if (Board.IsWall((int)(actor.Y - distance), actor.Column))
        {
            return (int)(actor.Y - distance) + 1;
        }
        return actor.Y - distance;
    }

    public double MoveDown(CapMan actor, double distance)
    {
        if (Board.IsWall((int)Math.Ceiling(actor.Y + distance), actor.Column))
        {
            return (int)Math.Ceiling(actor.Y + distance) - 1;
        }
        return actor.Y + distance;
    }

    public double MoveLeft(CapMan actor, double distance)
    {
        if (Board.IsWall(actor.Row, (int)(actor.X - distance)))
        {
            return (int)(actor.X - distance) + 1;
        }
        return actor.X - distance;
    }

    public double MoveRight(CapMan actor, double distance)
    {
        if (Board.IsWall(actor.Row, (int)Math.Ceiling(actor.X + distance)))
        {
            return (int)Math.Ceiling(actor.X + distance) - 1;
        }
        return actor.X + distance;
    }

}