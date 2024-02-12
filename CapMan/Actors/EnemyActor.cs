namespace CapMan;

public class EnemyActor : Actor
{
    public EnemyState State { get; set; } = EnemyState.Searching;

    public override void Update(Board board, double delta)
    {
        if (board.ValidTurns(delta, this) is Direction[] turns && turns.Length > 0)
        {
            NextDirection = turns[Random.Shared.Next(turns.Length)];
        }
        base.Update(board, delta);
    }
}