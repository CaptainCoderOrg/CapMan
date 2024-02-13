namespace CapMan;

public class TargetTileBehaviour(Tile target) : IEnemyBehaviour
{
    public Tile Target { get; } = target;
    public Direction GetNextDirection(Game game, Actor actor, double delta)
    {
        var (board, direction) = (game.Board, actor.CurrentDirection);
        Direction[] turns = [.. board.ValidTurns(delta, actor).Where(d => d != direction.Opposite())];
        if (turns.Length > 0)
        {
            return turns[Random.Shared.Next(turns.Length)];
        }
        return direction;
    }
}
