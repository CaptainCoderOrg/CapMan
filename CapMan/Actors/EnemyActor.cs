namespace CapMan;

public class EnemyActor(Position position, double speed, Direction direction) : Actor(position, speed, direction)
{
    public EnemyState State { get; set; } = EnemyState.Searching;

    public override void Update(Board board, double delta)
    {
        Direction[] turns = [.. board.ValidTurns(delta, this).Where(d => d != CurrentDirection.Opposite())];
        if (turns.Length > 0)
        {
            NextDirection = turns[Random.Shared.Next(turns.Length)];
        }
        base.Update(board, delta);
    }
}