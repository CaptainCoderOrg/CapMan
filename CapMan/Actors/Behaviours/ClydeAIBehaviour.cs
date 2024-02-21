namespace CapMan;

public class ClydeAIBehaviour : IEnemyBehaviour
{
    private readonly IEnemyBehaviour _patrolBehaviour = new PatrolBehaviour(new Tile(11, 13), new Tile(11, 15));
    private readonly IEnemyBehaviour _leaveHouseBehaviour = new TargetTileBehaviour(new Tile(13, 11));
    private readonly IEnemyBehaviour _afterExit = new ClydeTargeting();
    private bool _hasExited = false;
    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor enemy)
    {
        if (game.PlayTime < 7)
        {
            _hasExited = false;
            return _patrolBehaviour.GetNextDirection(game, deltaTime, enemy);
        }

        if (_hasExited is false)
        {
            if (enemy.Tile == new Tile(13, 11)) { _hasExited = true; }
            enemy.IgnoreDoors = true;
            return _leaveHouseBehaviour.GetNextDirection(game, deltaTime, enemy);
        }

        enemy.IgnoreDoors = false;
        return _afterExit.GetNextDirection(game, deltaTime, enemy);
    }
}