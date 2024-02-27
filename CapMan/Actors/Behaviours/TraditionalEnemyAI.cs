namespace CapMan;

public class TraditionalEnemyAI(Tile patrolA, Tile patrolB, Tile exitTile, IEnemyBehaviour baseBehaviour, double exitTime) : IEnemyBehaviour
{
    private readonly double _exitTime = exitTime;
    private readonly IEnemyBehaviour _patrolBehaviour = new PatrolBehaviour(patrolA, patrolB);
    private readonly TargetTileBehaviour _leaveHouseBehaviour = new(exitTile);
    private readonly IEnemyBehaviour _afterExit = baseBehaviour;
    private readonly TargetTileBehaviour _respawnBehaviour = new(patrolA);
    private bool _hasExited = false;
    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor enemy)
    {
        if (!enemy.IsAlive)
        {
            if (enemy.Tile == _respawnBehaviour.Target)
            {
                enemy.IsAlive = true;
                _hasExited = false;
            }
            enemy.IgnoreDoors = true;
            return _respawnBehaviour.GetNextDirection(game, deltaTime, enemy);
        }

        if (game.PlayTime < _exitTime)
        {
            _hasExited = false;
            return _patrolBehaviour.GetNextDirection(game, deltaTime, enemy);
        }

        if (game.IsPoweredUp)
        {
            return RandomBehaviour.Shared.GetNextDirection(game, deltaTime, enemy);
        }

        if (_hasExited is false)
        {
            if (enemy.Tile == _leaveHouseBehaviour.Target) { _hasExited = true; }
            enemy.IgnoreDoors = true;
            return _leaveHouseBehaviour.GetNextDirection(game, deltaTime, enemy);
        }

        enemy.IgnoreDoors = false;
        return _afterExit.GetNextDirection(game, deltaTime, enemy);
    }
}