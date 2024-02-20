namespace CapMan;

public class RandomBehaviour : IEnemyBehaviour
{
    public Direction GetNextDirection(IGame game, double delta, EnemyActor actor)
    {
        var (board, direction) = (game.Board, actor.CurrentDirection);
        Direction[] turns = [.. board.ValidNextDirection(delta, actor).Where(d => d != direction.Opposite())];
        if (turns.Length > 0)
        {
            Direction turn = turns[Random.Shared.Next(turns.Length)];
            actor.LastTarget = actor.Position.NextTile(turn);
            return turn;
        }
        actor.LastTarget = actor.Position.NextTile(actor.CurrentDirection);
        return direction;
    }
}
