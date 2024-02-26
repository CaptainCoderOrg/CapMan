namespace CapMan.Tests;

using NSubstitute;


using Shouldly;

public class Player_should_
{
    [Fact]
    public void not_start_with_projectile()
    {
        PlayerActor player = new(new Position(0, 0), 8, Direction.Up);
        player.HasProjectile.ShouldBeFalse();
    }

    [Fact]
    public void have_projectile_when_create_projectile_is_set()
    {
        PlayerActor player = new(new Position(0, 0), 8, Direction.Up);
        player.CreateProjectile = PlayerProjectileExtensions.BowlerHatProjectile;
        player.HasProjectile.ShouldBeTrue();

    }

    [Fact]
    public void pick_up_non_lethal_projectile()
    {
        string[] gameConfig = [
            "CapMan, (1, 1), 1, Right, manual",
            "",
            "+--+",
            "|..|",
            "+--+",
        ];
        Game game = new(gameConfig);

        IProjectile projectile = Substitute.For<IProjectile>();
        projectile.IsLethal.Returns(false);
        projectile.Position.Returns(new Position(2, 1));
        game.AddProjectile(projectile);

        game.Player.Update(game, 1);
        game.Player.HasProjectile.ShouldBe(true);
        projectile.IsPickedUp.ShouldBeTrue();
    }
}