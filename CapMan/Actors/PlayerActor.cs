namespace CapMan;

public class PlayerActor(Position startPosition, double speed, Direction direction) : Actor(startPosition, speed, direction)
{
    public bool HasProjectile => CreateProjectile is not null;
    public Func<PlayerActor, Projectile>? CreateProjectile { get; set; }

    public override void Update(IGame game, double deltaTime)
    {
        PickUpProjectiles(game.Projectiles);
        SetSpeed(game);
        base.Update(game, deltaTime);
    }

    private void PickUpProjectiles(IEnumerable<IProjectile> projectiles)
    {
        projectiles = projectiles.NotWhere(p => p.IsLethal)
                                 .Where(p => p.BoundingBox().IntersectsWith(this.BoundingBox()));
        foreach (IProjectile projectile in projectiles)
        {
            CreateProjectile = PlayerProjectileExtensions.BowlerHatProjectile;
            projectile.IsPickedUp = true;
        }
    }

    private void SetSpeed(IGame game)
    {
        this.Speed = BaseSpeed * SpeedMultiplier(this, game);
        this.Speed *= 1.5;
    }
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