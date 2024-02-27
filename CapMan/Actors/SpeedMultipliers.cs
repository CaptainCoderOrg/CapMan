namespace CapMan;

public static class SpeedMultipliers
{
    public static double BasicIncreasingSpeed(this EnemyActor _, IGame game)
    {
        int multiplier = Math.Min(5, game.Level);
        return .70 + .05 * multiplier;
    }

    public static double NoMultiplier(this EnemyActor _, IGame __) => 1;
}