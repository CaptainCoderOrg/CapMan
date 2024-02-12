namespace CapMan;

public abstract class Actor
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; }
    public Direction CurrentDirection { get; set; }
    public Direction NextDirection { get; set; }
    public int Column => CurrentDirection switch
    {
        Direction.Left => (int)Math.Ceiling(X),
        _ => (int)X,
    };
    public int Row => CurrentDirection switch
    {
        Direction.Up => (int)Math.Ceiling(Y),
        _ => (int)Y,
    };

    public virtual void Update(Board board, double deltaTime)
    {
       (X, Y, CurrentDirection) = board.CalculateActorMove(deltaTime, this);
       (X, Y) = board.BoundsCheck(this);
    }
}