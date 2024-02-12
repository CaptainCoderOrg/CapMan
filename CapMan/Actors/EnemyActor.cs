namespace CapMan;

public class EnemyActor : Actor
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