namespace CapMan;

/// <summary>
/// This is an implementation of Clyde's intended AI from the original Pac Man game.
/// - If distance from Pac Man is greater than 8 tiles, Target PacMan's tile
/// - Otherwise, target the bottom left corner of the map
/// </summary>
public class ClydeTargeting : IEnemyBehaviour
{
    public const int TargetPlayerDistance = 8;
    private readonly TargetPlayerTile _targetPlayerTile = new();
    public Direction GetNextDirection(Game game, double deltaTime, EnemyActor enemy)
    {
        var (board, direction, player) = (game.Board, enemy.CurrentDirection, game.Player);
        if (player.Tile.ManhattanDistance(enemy.Tile) > TargetPlayerDistance)
        {
            return _targetPlayerTile.GetNextDirection(game, deltaTime, enemy);
        }
        Tile target = new(1, board.Height - 2);
        enemy.LastTarget = target;
        return TargetTileBehaviour.DirectionWithShortestPath(board, enemy.Position.NextTile(direction), direction, target);
    }
}