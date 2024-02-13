namespace CapMan;

public class EnemyActor(Position position, double speed, Direction direction) : Actor(position, speed, direction)
{
    public EnemyState State { get; set; } = EnemyState.Searching;
    public IEnemyBehaviour Behaviour { get; private set; } = new RandomBehaviour();
    public override void Update(Game game, double delta)
    {
        NextDirection = Behaviour.GetNextDirection(game, this, delta);
        base.Update(game, delta);
    }
}