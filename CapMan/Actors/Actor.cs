namespace CapMan;

public abstract class Actor(Position position, double speed, Direction direction)
{
    public Position Position { get; set; } = position;
    public double Speed { get; set; } = speed;
    public Direction CurrentDirection { get; set; } = direction;
    public Direction NextDirection { get; set; } = direction;
    public Tile Tile => Position.CurrentTile(CurrentDirection);

    public virtual void Update(Board board, double deltaTime)
    {
        (Position, CurrentDirection) = board.CalculateActorMove(deltaTime, this);
        Position = board.WrapPosition(this);
    }
}