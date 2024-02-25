namespace CapMan;

public class PlayerActor() : Actor(new Position(14, 23), 8, Direction.Left)
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