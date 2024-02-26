namespace CapMan.Tests;
public class Game_should
{
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
            "CapMan         , 1, 3, 4, Left , manual",
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
        game.Player.CurrentDirection.ShouldBe(Direction.Left);

        game.Enemies.Count().ShouldBe(4);

        game.Enemies[0].Behaviour.ShouldBeOfType<KevinAIBehaviour>();
        game.Enemies[1].Behaviour.ShouldBeOfType<ClydeAIBehaviour>();
        game.Enemies[2].Behaviour.ShouldBeOfType<BobAIBehaviour>();
        game.Enemies[3].Behaviour.ShouldBeOfType<WhimsicalAIBehaviour>();
    }
}
