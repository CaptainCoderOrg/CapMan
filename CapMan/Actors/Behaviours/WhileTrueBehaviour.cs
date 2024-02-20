namespace CapMan;

public class WhileTrueBehaviour(IEnemyBehaviour whileTrue, Predicate<IGame> pred, IEnemyBehaviour onComplete) : IEnemyBehaviour
{
    public IEnemyBehaviour WhileTrue { get; } = whileTrue;
    public Predicate<IGame> Predicate = pred;
    public IEnemyBehaviour OnComplete { get; } = onComplete;

    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor actor)
    {
        if (Predicate.Invoke(game)) 
        { 
            return WhileTrue.GetNextDirection(game, deltaTime, actor); 
        }
        actor.Behaviour = OnComplete;
        return OnComplete.GetNextDirection(game, deltaTime, actor);
    }
}