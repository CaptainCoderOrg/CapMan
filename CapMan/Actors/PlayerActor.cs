namespace CapMan;

public class PlayerActor : Actor
{

    public PlayerActor(int speed)
    {
        Speed = speed;
        CurrentDirection = Direction.Left;
        NextDirection = Direction.Left;
    }
}