namespace CapMan.Tests;

using Shouldly;

public class Player_should_
{
    [Fact]
    public void not_start_with_projectile()
    {
        PlayerActor player = new();
        player.HasProjectile.ShouldBeFalse();
    }

    [Fact]
    public void have_projectile_when_create_projectile_is_set()
    {
        PlayerActor player = new();
        player.CreateProjectile = PlayerProjectileExtensions.BowlerHatProjectile;
        player.HasProjectile.ShouldBeTrue();
        
    }
}