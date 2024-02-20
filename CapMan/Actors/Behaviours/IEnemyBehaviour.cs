namespace CapMan;

public interface IEnemyBehaviour
{
    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor actor);
}
