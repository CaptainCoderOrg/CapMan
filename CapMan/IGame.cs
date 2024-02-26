namespace CapMan;
public interface IGame
{
    public GameState State { get; set; }
    /// <summary>
    /// The amount of time the player remains powered up after collecting a
    /// Power Up.
    /// </summary>
    public double PoweredUpTime { get; }
    public bool IsPoweredUp { get; }
    public double PlayTime { get; }
    public double RespawnTime { get; }
    public double RespawnCountDown { get; }
    public int Lives { get; set; }
    public PlayerActor Player { get; }
    public EnemyActor[] Enemies { get; }
    public Board Board { get; }
    public int Score { get; }
    public IReadOnlyList<IProjectile> Projectiles { get; }

    /// <summary>
    /// Progress the game forward the specified amount of time in seconds.
    /// </summary>
    public void Update(double deltaTime);
}