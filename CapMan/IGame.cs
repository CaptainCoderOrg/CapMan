namespace CapMan;
public interface IGame
{
    public GameState State { get; set; }
    public double PlayTime { get; }
    public double RespawnTime { get; }
    public double RespawnCountDown { get; }
    public int Lives { get; set; }
    public PlayerActor Player { get; }
    public EnemyActor[] Enemies { get; }
    public Board Board { get; }
    public int Score { get; }
    public void Update(double delta);
}