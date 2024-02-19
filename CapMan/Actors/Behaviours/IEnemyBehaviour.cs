namespace CapMan;

public interface IEnemyBehaviour
{
    public Direction GetNextDirection(Game game, double deltaTime, EnemyActor actor);
}
