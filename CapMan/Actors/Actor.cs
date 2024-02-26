namespace CapMan;

public abstract class Actor(Position position, double speed, Direction direction) : IActor
{
    public Position StartPosition { get; } = position;
    public Position Position { get; set; } = position;
    public double Speed { get; set; } = speed;
    public Direction StartDirection { get; } = direction;
    public Direction CurrentDirection { get; set; } = direction;
    public Direction NextDirection { get; set; } = direction;
    public Tile Tile => Position.CurrentTile(CurrentDirection);

    public virtual void Update(IGame game, double deltaTime)
    {
        (Position, CurrentDirection) = game.Board.CalculateActorMove(deltaTime, this);
        Position = game.Board.WrapPosition(this);
    }
}