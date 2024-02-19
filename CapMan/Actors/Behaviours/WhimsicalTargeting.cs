namespace CapMan;

/// <summary>
/// This is an implementation of Inky's intended AI from the original Pac Man game.
/// - Look 2 tiles in front of the Player (x,y)
/// - Draw a line from an Observer ("Blinky") to that position vector(lx, ly)
/// - The final position is (x + lx*2, y + ly*2) 
/// </summary>
public class WhimsicalTargeting(Actor observer) : IEnemyBehaviour
{
    public Actor Observer { get; } = observer;
    public Direction GetNextDirection(Game game, double deltaTime, EnemyActor enemy)
    {
        var (board, direction, player) = (game.Board, enemy.CurrentDirection, game.Player);
        var (x, y) = board.WrapTile(player.Tile.Step(player.CurrentDirection, 2));
        var (ox, oy) = Observer.Tile;
        var (lx, ly) = (x - ox, y - oy);
        Tile target = board.WrapTile(new(x + lx * 2, y + ly * 2));
        enemy.LastTarget = target;
        return TargetTileBehaviour.DirectionWithShortestPath(board, enemy.Position.NextTile(direction), direction, target);
    }
}