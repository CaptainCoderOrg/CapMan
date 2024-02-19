namespace CapMan;

public class TargetAheadOfPlayer(int steps) : IEnemyBehaviour
{
    public int Steps { get; } = steps;
    public Direction GetNextDirection(Game game, double deltaTime, EnemyActor enemy)
    {
        var (board, direction, player) = (game.Board, enemy.CurrentDirection, game.Player);
        Tile target = board.WrapTile(player.Tile.Step(player.CurrentDirection, Steps));
        enemy.LastTarget = target;
        return TargetTileBehaviour.DirectionWithShortestPath(board, enemy.Position.NextTile(direction), direction, target);
    }
}