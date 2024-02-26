namespace CapMan;

public class Projectile(Position position, double speed, Direction direction) : Actor(position, speed, direction), IProjectile
{
    public bool IsLethal { get; private set; } = true;

    public override void Update(IGame game, double deltaTime)
    {
        Position previousPosition = Position;
        base.Update(game, deltaTime);

        // If the projectile has no observed velocity
        // it is non-lethal
        double expectedDistance = Speed * deltaTime;
        double actualDistance = previousPosition.ManhattanDistance(Position);
        IsLethal = (expectedDistance - actualDistance) == 0;
    }
}