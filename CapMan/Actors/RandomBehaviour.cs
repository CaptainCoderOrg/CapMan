namespace CapMan;

public class RandomBehaviour : IEnemyBehaviour
{
    public Direction GetNextDirection(Game game, double delta, Actor actor)
    {
        var (board, direction) = (game.Board, actor.CurrentDirection);
        Direction[] turns = [.. board.ValidNextDirection(delta, actor).Where(d => d != direction.Opposite())];
        if (turns.Length > 0)
        {
            return turns[Random.Shared.Next(turns.Length)];
        }
        return direction;
    }
}
