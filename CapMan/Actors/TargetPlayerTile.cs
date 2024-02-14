namespace CapMan;

public class TargetPlayerTile : IEnemyBehaviour
{
    public Direction GetNextDirection(Game game, double deltaTime, Actor actor)
    {
        var (board, direction, target) = (game.Board, actor.CurrentDirection, game.Player.Tile);
        return TargetTileBehaviour.DirectionWithShortestPath(board, actor.Position.NextTile(direction), direction, target);
    }
}
