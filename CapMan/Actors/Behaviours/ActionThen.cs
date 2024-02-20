namespace CapMan;
public class ActionThen(Action action, IEnemyBehaviour behaviour) : IEnemyBehaviour
{
    public Action Action { get; } = action;
    public IEnemyBehaviour Behaviour { get; } = behaviour;

    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor actor)
    {
        Action.Invoke();
        return Behaviour.GetNextDirection(game, deltaTime, actor);
    }
}