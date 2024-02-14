namespace CapMan;

public class EnemyActor(Position position, double speed, Direction direction) : Actor(position, speed, direction)
{
    public EnemyState State { get; set; } = EnemyState.Searching;
    public IEnemyBehaviour Behaviour { get; set; } = new TargetTileBehaviour(new Tile(1, 1));
    public override void Update(Game game, double deltaTime)
    {
        NextDirection = Behaviour.GetNextDirection(game, deltaTime, this);
        base.Update(game, deltaTime);
    }
}