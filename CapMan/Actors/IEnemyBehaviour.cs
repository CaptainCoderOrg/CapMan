namespace CapMan;

public interface IEnemyBehaviour
{
    public Direction GetNextDirection(Game game, Actor actor, double delta);
}
