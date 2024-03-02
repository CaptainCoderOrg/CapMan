namespace CapMan;

public static class SpeedMultipliers
{
    public static double BasicIncreasingSpeed(this Actor _, IGame game)
    {
        int multiplier = Math.Min(5, game.Level);
        return .70 + .05 * multiplier;
    }

    public static double NoMultiplier(this Actor _, IGame __) => 1;
}