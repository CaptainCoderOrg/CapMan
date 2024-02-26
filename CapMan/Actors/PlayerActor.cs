namespace CapMan;

public class PlayerActor(Position startPosition, double speed, Direction direction) : Actor(startPosition, speed, direction)
{
    public bool HasProjectile => CreateProjectile is not null;
    public Func<PlayerActor, Projectile>? CreateProjectile { get; set; }
}

public static class PlayerProjectileExtensions
{
    public static Projectile BowlerHatProjectile(this PlayerActor player)
    {
        return new(player.Position, player.Speed * 2, player.CurrentDirection)
        {
            NextDirection = player.NextDirection,
        };
    }
}