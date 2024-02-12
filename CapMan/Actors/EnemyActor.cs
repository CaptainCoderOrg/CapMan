namespace CapMan;

public class EnemyActor : Actor
{
    public EnemyState State { get; set; } = EnemyState.Searching;
}