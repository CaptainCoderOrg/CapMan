public class Game
{
    public CapMan Player { get; } = new CapMan();
    public Board Board { get; } = new Board(Board.StandardBoard);

    public void Update(double delta)
    {
        
        Func<CapMan, double, double> NextX = Player.CurrentDirection switch
        {
            Direction.Left => MoveLeft,
            Direction.Right => MoveRight,
            _ => (actor,_) => actor.Column,
        };
        
        Func<CapMan, double, double> NextY = Player.CurrentDirection switch
        {
            Direction.Up => MoveUp,
            Direction.Down => MoveDown,
            _ => (actor,_) => actor.Row,
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
            // Player.CurrentDirection = next;
            // Player.X = NextX(Player, distance);
            // Player.Y = NextY(Player, distance);
        }

    }

    private void SwitchDirection(CapMan actor, double distance)
    {
        if (actor.CurrentDirection is Direction.Up && actor.NextDirection is Direction.Down) 
        { 
            actor.Y += distance; 
        }
        else if (actor.CurrentDirection is Direction.Up)
        {
            // 10.1 => 10
            Console.WriteLine($"Moving Up {distance}");
            Console.WriteLine($"Was: {actor.Y}");
            double start = actor.Y;
            actor.Y = (int)actor.Y;
            Console.WriteLine($"IsNow: {actor.Y}");
            double leftOverDistance = start - actor.Y;
            Console.WriteLine($"Leftover: {leftOverDistance}");
            
            actor.X += actor.NextDirection is Direction.Left ? -leftOverDistance : leftOverDistance;             
        }

        actor.CurrentDirection = actor.NextDirection;
    }

    private Direction NextDirectionIfGoingUp(CapMan actor, double distance)
    {
        if (actor.NextDirection is Direction.Down or Direction.Up) { return actor.NextDirection; }
        // start   th    end
        // 10.1 => 10 => 9.9 
        double endPosition = actor.Y - distance; 
        if(((int)actor.Y) != ((int)Math.Ceiling(endPosition))) { return actor.CurrentDirection; }
        (int row, int col) = actor.NextDirection switch
        {
            Direction.Left => ((int)endPosition + 1, (int)(actor.X - 1)),
            Direction.Right => ((int)endPosition + 1, (int)(actor.X + 1)),
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

        (int row, int col) = actor.NextDirection switch
        {
            Direction.Up => ((int)(actor.Y - 1), actor.Column),
            Direction.Down => ((int)Math.Ceiling(actor.Y + 1), actor.Column),
            Direction.Left => (actor.Row, (int)(actor.X - 1)),
            Direction.Right => (actor.Row, (int)Math.Ceiling(actor.X + 1)),
            _ => throw new Exception($"Invalid directino (spanish for direction): {actor.NextDirection}"),
        };
        if (Board.IsWall(row, col)) { return actor.CurrentDirection; }
        // TODO: Did the player cross a boundary for which they should
        //       change directions? If so, by how much?
        return actor.NextDirection;
        
    }


    public double MoveUp(CapMan actor, double distance)
    {
        if(Board.IsWall((int)(actor.Y - distance), actor.Column))
        {
            return (int)(actor.Y - distance) + 1;
        }
        return actor.Y - distance;        
    }

    public double MoveDown(CapMan actor, double distance)
    {
        if(Board.IsWall((int)Math.Ceiling(actor.Y + distance), actor.Column))
        {
            return (int)Math.Ceiling(actor.Y + distance) - 1;
        }
        return actor.Y + distance;        
    }

    public double MoveLeft(CapMan actor, double distance)
    {
        if(Board.IsWall(actor.Row, (int)(actor.X - distance)))
        {
            return (int)(actor.X - distance) + 1;
        }
        return actor.X - distance;
    }

    public double MoveRight(CapMan actor, double distance)
    {
        if(Board.IsWall(actor.Row, (int)Math.Ceiling(actor.X + distance)))
        {
            return (int)Math.Ceiling(actor.X + distance) - 1;
        }
        return actor.X + distance;
    }

}