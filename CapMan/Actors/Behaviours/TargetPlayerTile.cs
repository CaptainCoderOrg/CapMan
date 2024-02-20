namespace CapMan;

public class TargetPlayerTile : IEnemyBehaviour
{
    public Direction GetNextDirection(IGame game, double deltaTime, EnemyActor enemy)
    {
        var (board, direction, target) = (game.Board, enemy.CurrentDirection, game.Player.Tile);
        enemy.LastTarget = target;
        return TargetTileBehaviour.DirectionWithShortestPath(board, enemy.Position.NextTile(direction), direction, target);
    }
}