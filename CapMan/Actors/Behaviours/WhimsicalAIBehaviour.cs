namespace CapMan;

public class WhimsicalAIBehaviour(Tile patrol1, Tile patrol2, Tile houseExit, EnemyActor target) : IEnemyBehaviour
{
    private readonly IEnemyBehaviour _patrolBehaviour = new PatrolBehaviour(patrol1, patrol2);
    private readonly IEnemyBehaviour _leaveHouseBehaviour = new TargetTileBehaviour(houseExit);
    private readonly IEnemyBehaviour _afterExit = new WhimsicalTargeting(target);
    private bool _hasExited = false;
    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor enemy)
    {
        if (game.PlayTime < 21)
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