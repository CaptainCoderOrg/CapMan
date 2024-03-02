namespace CapMan.Tests;

using Shouldly;

public class GameBoard_should_
{
    [Fact]
    public void add_enemy_using_string_parsing()
    {
        string[] boardLayout =
        [
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];

        Game game = new GameBuilder()
            .AddPlayer("CapMan, (2, 1), 1, Down")
            .AddEnemy("kevinEnemy     , (14, 11), 4, Down , Kevin    , (12, 13), (12, 15), (13, 11)")
            .AddEnemy("clydeEnemy     , (11, 14), 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)")
            .AddEnemy("targetAhead    , (13, 15), 4, Right, Bob      , (13, 15), (13, 13), (13, 11)")
            .AddEnemy("whimsicalEnemy , (16, 14), 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy")
            .AddBoard(boardLayout)
            .Build();

        game.Enemies.Count().ShouldBe(4);

        game.Enemies[0].Behaviour.ShouldBeOfType<KevinAIBehaviour>();
        game.Enemies[1].Behaviour.ShouldBeOfType<ClydeAIBehaviour>();
        game.Enemies[2].Behaviour.ShouldBeOfType<BobAIBehaviour>();
        game.Enemies[3].Behaviour.ShouldBeOfType<WhimsicalAIBehaviour>();
    }

    [Fact]
    public void add_enemies_using_string_parsing()
    {
        string enemies = $"""
            kevinEnemy     , (14, 11), 4, Down , Kevin    , (12, 13), (12, 15), (13, 11)
            clydeEnemy     , (11, 14), 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)
            targetAhead    , (13, 15), 4, Right, Bob      , (13, 15), (13, 13), (13, 11)
            whimsicalEnemy , (16, 14), 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy
            """;

        Game game = new GameBuilder()
            .AddPlayer("CapMan, (2, 1), 1, Down")
            .AddEnemies(enemies)
            .AddBoard(Board.StandardBoard)
            .Build();

        game.Enemies.Count().ShouldBe(4);

        game.Enemies[0].Behaviour.ShouldBeOfType<KevinAIBehaviour>();
        game.Enemies[1].Behaviour.ShouldBeOfType<ClydeAIBehaviour>();
        game.Enemies[2].Behaviour.ShouldBeOfType<BobAIBehaviour>();
        game.Enemies[3].Behaviour.ShouldBeOfType<WhimsicalAIBehaviour>();
    }

    [Fact]
    public void add_player_using_string_parsing()
    {
        string[] boardLayout =
        [
            "+---+",
            "|...|",
            "|.O.|",
            "|...|",
            "+---+",
        ];

        Game game = new GameBuilder()
            .AddPlayer("CapMan, (2, 1), 1, Down")
            .AddBoard(boardLayout)
            .Build();

        game.Player.StartPosition.X.ShouldBe(2);
        game.Player.StartPosition.Y.ShouldBe(1);
        game.Player.Speed.ShouldBe(1);
        game.Player.StartDirection.ShouldBe(Direction.Down);
    }

    [Fact]
    public void add_player_using_parameters()
    {
        Board board = new(
            [
                "+------+",
                "|......|",
                "+------+",
            ]
        );

        Game game = new GameBuilder()
            .AddPlayer(new Position(1, 2), 3, Direction.Left)
            .AddBoard(board)
            .Build();

        game.Player.StartPosition.X.ShouldBe(1);
        game.Player.StartPosition.Y.ShouldBe(2);
        game.Player.Speed.ShouldBe(3);
        game.Player.StartDirection.ShouldBe(Direction.Left);
    }


    [Fact]
    public void add_player_using_playeractor()
    {
        Board board = new(
            [
                "+------+",
                "|......|",
                "+------+",
            ]
        );
        PlayerActor player = new(new Position(0, 0), 0, Direction.Left);
        Game game = new GameBuilder()
            .AddPlayer(player)
            .AddBoard(board)
            .Build();

        game.Player.StartPosition.X.ShouldBe(0);
        game.Player.StartPosition.Y.ShouldBe(0);
        game.Player.Speed.ShouldBe(0);
        game.Player.StartDirection.ShouldBe(Direction.Left);
    }

    [Fact]
    public void throw_if_player_is_not_added()
    {
        Board board = new(
            [
                "+------+",
                "|......|",
                "+------+",
            ]
        );

        Should.Throw<ArgumentNullException>(
            () => new GameBuilder()
                .AddBoard(board)
                .Build())
            .ParamName.ShouldBe("player");

    }

    [Fact]
    public void throw_if_board_is_not_added()
    {
        Should.Throw<ArgumentNullException>(
            () => new GameBuilder()
                .AddPlayer(new PlayerActor(new Position(0, 0), 1, Direction.Left))
                .Build())
            .ParamName.ShouldBe("board");
    }

    [Fact]
    public void create_from_string_layout()
    {
        string gameConfig = $"""
            CapMan         , (14, 23), 8, Left , manual
            kevinEnemy     , (14, 11), 4, Down , Kevin    , (12, 13), (12, 15), (13, 11)
            clydeEnemy     , (11, 14), 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)
            targetAhead    , (13, 15), 4, Right, Bob      , (13, 15), (13, 13), (13, 11)
            whimsicalEnemy , (16, 14), 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy

            {Board.StandardBoard}
            """;

        Game game = new GameBuilder()
            .Configure(gameConfig)
            .Build();

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
        string[] gameConfig = [
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

        Game game = new GameBuilder()
           .Configure(gameConfig)
           .Build();

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
}