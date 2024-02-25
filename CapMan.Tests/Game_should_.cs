namespace CapMan.Tests;

using Shouldly;

public class Game_should_
{
    [Fact]
    public void be_able_to_have_projectiles()
    {
        Board board = new(
            [
                "+------+",
                "|......|",
                "|......|",
                "|......|",
                "|......|",
                "|......|",
                "|......|",
                "+------+",
            ]
        );
        Game game = new([], board);
        Projectile toAdd = new(new Position(2, 2), 16, Direction.Up)
        {
            NextDirection = Direction.Up,
        };
        game.AddProjectile(toAdd);
        game.Projectiles.Count.ShouldBe(1);
    }

    [Fact]
    public void set_player_projectile_when_moving_over_power_pill()
    {
        Board board = new(
            [
                "+---+",
                "|   |",
                "| O |",
                "|   |",
                "+---+",
            ]
        );

        Game game = new([], board);
        game.Player.Position = new Position(2, 1);
        game.Player.Speed = 1; // Ensures player moves 1 tile per second
        game.Player.CurrentDirection = Direction.Down;
        game.Player.NextDirection = Direction.Down;
        game.Update(1f);

        // The player should now have a Projectile
        game.Player.HasProjectile.ShouldBeTrue();
    }
}