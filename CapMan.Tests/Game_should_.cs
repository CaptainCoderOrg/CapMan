namespace CapMan.Tests;

using NSubstitute;


using Shouldly;

public class Game_should_
{
    [Fact]
    public void begin_with_is_powered_up_false()
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down , manual",
            "",
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        game.IsPoweredUp.ShouldBe(false);
    }

    [Fact]
    public void set_is_powered_up_when_powerup_is_collected()
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        game.Update(1f);
        game.IsPoweredUp.ShouldBeTrue();
    }

    [Fact]
    public void power_up_should_expire_after_power_up_time()
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        game.Update(1f);
        game.IsPoweredUp.ShouldBeTrue();

        game.Update(game.PoweredUpTime);
        game.IsPoweredUp.ShouldBeFalse();
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(2f)]
    [InlineData(3f)]
    [InlineData(4f)]
    [InlineData(5f)]
    [InlineData(6f)]
    [InlineData(7f)]
    [InlineData(8f)]
    [InlineData(9f)]
    public void power_up_should_not_expire_before_power_up_time(double timePassed)
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        game.Update(1f);
        game.IsPoweredUp.ShouldBeTrue();

        game.Update(timePassed);
        game.IsPoweredUp.ShouldBeTrue();
    }

    [Fact]
    public void clear_projectiles_when_power_up_time_expires()
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        game.AddProjectile(new Projectile(new Position(1, 1), 8, Direction.Up));
        game.Update(1f);

        game.Update(game.PoweredUpTime);
        game.Projectiles.ShouldBeEmpty();
    }

    [Fact]
    public void remove_player_projectile_when_time_expires()
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        game.AddProjectile(new Projectile(new Position(1, 1), 8, Direction.Up));
        game.Update(1f);
        game.Player.HasProjectile.ShouldBeTrue();

        game.Update(game.PoweredUpTime);
        game.Player.HasProjectile.ShouldBeFalse();
    }

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
        PlayerActor player = new(new Position(0, 0), 0, Direction.Left);
        Game game = new([player], board);
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
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down , manual",
            "",
            "+---+",
            "|   |",
            "| O |",
            "|   |",
            "+---+",
        ];

        Game game = new(gameConfig);
        game.Update(1f);

        // The player should now have a Projectile
        game.Player.HasProjectile.ShouldBeTrue();
    }

    [Fact]
    public void create_from_string_layout()
    {
        string layout = $"""
            CapMan         , (14, 23), 8, Left , manual
            kevinEnemy     , (14, 11), 4, Down , Kevin    , (12, 13), (12, 15), (13, 11)
            clydeEnemy     , (11, 14), 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)
            targetAhead    , (13, 15), 4, Right, Bob      , (13, 15), (13, 13), (13, 11)
            whimsicalEnemy , (16, 14), 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy
            
            {Board.StandardBoard}
            """;

        Game game = new(layout);
        game.Board.Height.ShouldBe(31);
        game.Board.Width.ShouldBe(28);

        game.Player.Position.X.ShouldBe(14);
        game.Player.Position.Y.ShouldBe(23);
        game.Player.Speed.ShouldBe(8);
        game.Player.CurrentDirection.ShouldBe(Direction.Left);
        game.Player.NextDirection.ShouldBe(Direction.Left);

        game.Enemies.Count().ShouldBe(4);

        game.Enemies[0].Behaviour.ShouldBeOfType<KevinAIBehaviour>();
        game.Enemies[1].Behaviour.ShouldBeOfType<ClydeAIBehaviour>();
        game.Enemies[2].Behaviour.ShouldBeOfType<BobAIBehaviour>();
        game.Enemies[3].Behaviour.ShouldBeOfType<WhimsicalAIBehaviour>();
    }

    [Fact]
    public void create_from_string_array_layout()
    {
        string[] layout = [
            "CapMan         , 1, 3, 4, Down , manual",
            "kevinEnemy     , 2, 1, 5, Down , Kevin    , (12, 13), (12, 15), (13, 11)",
            "clydeEnemy     , 3, 4, 6, Left , Clyde    , (11, 13), (11, 15), (13, 11)",
            "targetAhead    , 4, 5, 7, Right, Bob      , (13, 15), (13, 13), (13, 11)",
            "whimsicalEnemy , 5, 4, 8, Up   , Whimsical, (16, 15), (16, 13), (13, 11), clydeEnemy",
            "",
            "+----------+",
            "|          |",
            "|          |",
            "|          |",
            "|          |",
            "|          |",
            "|          |",
            "|          |",
            "+----------+",
            ];

        Game game = new(layout);
        game.Board.Height.ShouldBe(9);
        game.Board.Width.ShouldBe(12);

        game.Player.Position.X.ShouldBe(1);
        game.Player.Position.Y.ShouldBe(3);
        game.Player.Speed.ShouldBe(4);
        game.Player.CurrentDirection.ShouldBe(Direction.Down);
        game.Player.NextDirection.ShouldBe(Direction.Down);

        game.Enemies.Count().ShouldBe(4);

        game.Enemies[0].Behaviour.ShouldBeOfType<KevinAIBehaviour>();
        game.Enemies[1].Behaviour.ShouldBeOfType<ClydeAIBehaviour>();
        game.Enemies[2].Behaviour.ShouldBeOfType<BobAIBehaviour>();
        game.Enemies[3].Behaviour.ShouldBeOfType<WhimsicalAIBehaviour>();
    }

    [Fact]
    public void should_remove_picked_up_projectiles()
    {
        string[] gameConfig = [
            "CapMan, (1, 1), 1, Right, manual",
            "",
            "+-----+",
            "|.....|",
            "+-----+",
        ];
        Game game = new(gameConfig);

        IProjectile projectile = Substitute.For<IProjectile>();
        projectile.IsPickedUp.Returns(true);
        projectile.Position.Returns(new Position(5, 1));
        game.AddProjectile(projectile);
        game.Projectiles.Count.ShouldBe(1);
        game.PoweredUpTimeRemaining = 10;

        game.Update(1);
        game.Projectiles.ShouldBeEmpty();
    }

    [Fact]
    public void award_points_when_defeating_enemies()
    {
        string layout = $"""
            CapMan         , (14, 23), 8, Left , manual
            kevinEnemy     , (14, 11), 4, Down , Kevin    , (12, 13), (12, 15), (13, 11)
            clydeEnemy     , (11, 14), 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)
            targetAhead    , (13, 15), 4, Right, Bob      , (13, 15), (13, 13), (13, 11)
            whimsicalEnemy , (16, 14), 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy
            
            {Board.StandardBoard}
            """;

        Game game = new(layout);
        EnemyActor kevinEnemy = game.Enemies.Where(e => e.Tile == new Tile(14, 11)).First();
        EnemyActor clydeEnemy = game.Enemies.Where(e => e.Tile == new Tile(11, 14)).First();
        EnemyActor bobEnemy = game.Enemies.Where(e => e.Tile == new Tile(13, 15)).First();
        EnemyActor whimsicalEnemy = game.Enemies.Where(e => e.Tile == new Tile(16, 14)).First();
        
        game.Score.ShouldBe(0);
        kevinEnemy.IsAlive = false;
        // + 200 points
        game.Score.ShouldBe(200);
        clydeEnemy.IsAlive = false;
        // + 400 points
        game.Score.ShouldBe(600);
        bobEnemy.IsAlive = false;
        // + 800 points
        game.Score.ShouldBe(1400);
        whimsicalEnemy.IsAlive = false;
        // + 1600 points
        game.Score.ShouldBe(3000);
    }

    [Fact]
    public void reset_point_multiplier_when_powered_down()
    {
        string layout = $"""
            CapMan         , (14, 23), 8, Left , manual
            kevinEnemy     , (14, 11), 4, Down , Kevin    , (12, 13), (12, 15), (13, 11)
            clydeEnemy     , (11, 14), 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)
            targetAhead    , (13, 15), 4, Right, Bob      , (13, 15), (13, 13), (13, 11)
            whimsicalEnemy , (16, 14), 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy
            
            {Board.StandardBoard}
            """;

        Game game = new(layout);
        EnemyActor kevinEnemy = game.Enemies.Where(e => e.Tile == new Tile(14, 11)).First();
        EnemyActor clydeEnemy = game.Enemies.Where(e => e.Tile == new Tile(11, 14)).First();
        EnemyActor bobEnemy = game.Enemies.Where(e => e.Tile == new Tile(13, 15)).First();
        EnemyActor whimsicalEnemy = game.Enemies.Where(e => e.Tile == new Tile(16, 14)).First();
        
        kevinEnemy.IsAlive = false;
        game.ScoreMultiplier.ShouldBe(2);
        clydeEnemy.IsAlive = false;
        game.ScoreMultiplier.ShouldBe(4);
        bobEnemy.IsAlive = false;
        game.ScoreMultiplier.ShouldBe(8);
        whimsicalEnemy.IsAlive = false;
        game.ScoreMultiplier.ShouldBe(16);
        game.PoweredUpTimeRemaining = 2;
        game.Update(1);
        game.ScoreMultiplier.ShouldBe(16);
        game.Update(1);
        game.ScoreMultiplier.ShouldBe(1);
    }

    [Fact]
    public void reset_power_up_on_start_level()
    {
        // Arrange
        Game underTest = MockObject.StateUnimportant;
        underTest.PoweredUpTimeRemaining = 5;

        // Act
        underTest.StartLevel();

        // Assert
        underTest.PoweredUpTimeRemaining.ShouldBe(0);
        underTest.IsPoweredUp.ShouldBeFalse();
    }
}