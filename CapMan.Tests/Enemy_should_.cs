namespace CapMan.Tests;

using Shouldly;

public class Enemy_should_
{

    [Fact]
    public void start_alive()
    {
        EnemyActor enemy = new(new Position(0, 0), 8, Direction.Up);
        enemy.IsAlive.ShouldBeTrue();
    }

    [Fact]
    public void die_when_colliding_with_projectile()
    {
        Board board = new(
        [
            "------",
            "......",
            "------",
        ]
        );
        
        EnemyActor enemy = new(new Position(3, 1), 1, Direction.Right);
        PlayerActor player = new(new Position(0, 0), 0, Direction.Left);
        
        Game game = new([enemy, player], board);
        Projectile projectile = new(new Position(4, 1), 1, Direction.Left);
        game.AddProjectile(projectile);

        enemy.Update(game, 1);

        enemy.IsAlive.ShouldBeFalse();
    }
}