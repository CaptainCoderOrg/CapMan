namespace CapMan.Tests;

using NSubstitute;

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
    public void die_when_colliding_with_lethal_projectile()
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

        IProjectile projectile = Substitute.For<IProjectile>();
        projectile.Position.Returns(new Position(4, 1));
        projectile.IsLethal.Returns(true);
        game.AddProjectile(projectile);

        enemy.Update(game, 1);

        enemy.IsAlive.ShouldBeFalse();
    }

    [Fact]
    public void not_die_when_colliding_with_non_lethal_projectile()
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

        IProjectile projectile = Substitute.For<IProjectile>();
        projectile.Position.Returns(new Position(4, 1));
        projectile.IsLethal.Returns(false);
        game.AddProjectile(projectile);

        enemy.Update(game, 1);

        enemy.IsAlive.ShouldBeTrue();
    }

    [Fact]
    public void reverse_direction_when_game_powers_up()
    {
        string[] gameConfig = [
            "CapMan         , (2, 1), 1, Left  , manual",
            "kevinEnemy     , (4, 1), 1, Left  , Kevin    , (12, 13), (12, 15), (13, 11)",
            "",
            "+-----+",
            "|O....|",
            "|.+-+.|",
            "|.| |.|",
            "|.+-+.|",
            "|.....|",
            "+-----+",
        ];
        Game game = new(gameConfig);
        EnemyActor underTest = game.Enemies.Where(e => e.Tile == new Tile(4, 1)).First();
        underTest.IsFleeing = false;
        game.PoweredUpTimeRemaining = 5;
        underTest.Update(game, 1);

        underTest.CurrentDirection.ShouldBe(Direction.Right);
    }

    [Fact]
    public void flee_when_game_is_powered_up()
    {
        string[] gameConfig = [
            "CapMan         , (2, 1), 1, Left  , manual",
            "kevinEnemy     , (4, 1), 1, Left  , Kevin    , (12, 13), (12, 15), (13, 11)",
            "",
            "+-----+",
            "|O....|",
            "|.+-+.|",
            "|.| |.|",
            "|.+-+.|",
            "|.....|",
            "+-----+",
        ];
        Game game = new(gameConfig);
        EnemyActor underTest = game.Enemies.Where(e => e.Tile == new Tile(4, 1)).First();
        underTest.IsFleeing = false;
        game.PoweredUpTimeRemaining = 5;
        underTest.Update(game, 1);

        underTest.IsFleeing.ShouldBeTrue();
    }

    [Fact]
    public void not_flee_when_game_is_not_powered_up()
    {
        string[] gameConfig = [
            "CapMan         , (2, 1), 1, Left  , manual",
            "kevinEnemy     , (4, 1), 1, Left  , Kevin    , (12, 13), (12, 15), (13, 11)",
            "",
            "+-----+",
            "|O....|",
            "|.+-+.|",
            "|.| |.|",
            "|.+-+.|",
            "|.....|",
            "+-----+",
        ];
        Game game = new(gameConfig);
        EnemyActor underTest = game.Enemies.Where(e => e.Tile == new Tile(4, 1)).First();
        underTest.IsFleeing = true;
        game.PoweredUpTimeRemaining = 0;
        underTest.Update(game, 1);

        underTest.IsFleeing.ShouldBeFalse();
    }
}