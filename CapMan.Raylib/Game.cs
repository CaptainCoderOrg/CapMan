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
        // Player.Update(this, delta);
        double distance = Player.Speed * delta;
        Player.X = NextX(Player, distance);
        Player.Y = NextY(Player, distance);
        Player.CurrentDirection = NextDirection(Player);
    }

    public Direction NextDirection(CapMan actor)
    {
        if (actor.NextDirection != actor.CurrentDirection)
        {
            (int row, int col) = actor.NextDirection switch
            {
                Direction.Up => (actor.Row - 1, actor.Column),
                Direction.Down => (actor.Row + 1, actor.Column),
                Direction.Left => (actor.Row, actor.Column - 1),
                Direction.Right => (actor.Row, actor.Column + 1),
                _ => throw new Exception($"Invalid directino: {actor.NextDirection}"),
            };
            if (Board.IsWall(row, col)) { return actor.CurrentDirection; }
            return actor.NextDirection;
        }
        return actor.CurrentDirection;
    }


    public double MoveUp(CapMan actor, double distance)
    {
        if(Board.IsWall(actor.Row - 1, actor.Column) && Board.IsWall((int)(actor.Y - distance), actor.Column))
        {
            return (int)(actor.Y - distance) + 1;
        }
        return actor.Y - distance;        
    }

    public double MoveDown(CapMan actor, double distance)
    {
        if(Board.IsWall(actor.Row + 1, actor.Column) && Board.IsWall((int)Math.Ceiling(actor.Y + distance), actor.Column))
        {
            return (int)Math.Ceiling(actor.Y + distance) - 1;
        }
        return actor.Y + distance;        
    }

    public double MoveLeft(CapMan actor, double distance)
    {
        if(Board.IsWall(actor.Row, actor.Column - 1) && Board.IsWall(actor.Row, (int)(actor.X - distance)))
        {
            return (int)(actor.X - distance) + 1;
        }
        return actor.X - distance;
    }

    public double MoveRight(CapMan actor, double distance)
    {
        if(Board.IsWall(actor.Row, actor.Column + 1) && Board.IsWall(actor.Row, (int)Math.Ceiling(actor.X + distance)))
        {
            return (int)Math.Ceiling(actor.X + distance) - 1;
        }
        return actor.X + distance;
    }

}