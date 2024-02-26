namespace CapMan.Tests;

using Shouldly;

public class Projectile_should_
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void be_lethal_before_hitting_wall(double timePassed)
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|...|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        Projectile underTest = new(new Position(1, 1), 1, Direction.Down);
        underTest.Update(game, timePassed);
        underTest.IsLethal.ShouldBeTrue();
    }

    [Fact]
    public void be_non_lethal_after_hitting_wall()
    {
        string[] gameConfig =
        [
            "CapMan, (2, 1), 1, Down, manual",
            "",
            "+---+",
            "|...|",
            "|...|",
            "|...|",
            "+---+",
        ];
        Game game = new(gameConfig);
        Projectile underTest = new(new Position(1, 1), 1, Direction.Down);
        underTest.Update(game, 3);
        underTest.IsLethal.ShouldBeFalse();
    }
}