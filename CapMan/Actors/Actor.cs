namespace CapMan;

public abstract class Actor(Position position, double speed, Direction direction) : IActor
{
    public Position StartPosition { get; } = position;
    public Position Position { get; set; } = position;
    public double BaseSpeed { get; set; } = speed;
    public double Speed { get; set; } = speed;
    public Direction StartDirection { get; } = direction;
    public Direction CurrentDirection { get; set; } = direction;
    public Direction NextDirection { get; set; } = direction;
    public Tile Tile => Position.CurrentTile(CurrentDirection);
    public Func<Actor, IGame, double> SpeedMultiplier { get; init; } = SpeedMultipliers.BasicIncreasingSpeed;

    public virtual void Update(IGame game, double deltaTime)
    {
        (Position, CurrentDirection) = game.Board.CalculateActorMove(deltaTime, this);
        Position = game.Board.WrapPosition(this);
        SetSpeed(game);
    }

    private void SetSpeed(IGame game)
    {
        this.Speed = BaseSpeed * SpeedMultiplier(this, game);
        if (this is EnemyActor enemyActor)
        {
            if (!enemyActor.IsAlive)
            {
                enemyActor.Speed *= 2;
            }
            else if (game.Board.IsSlowTile(enemyActor.Tile))
            {
                enemyActor.Speed *= 0.75;
            }
        }
    }
}