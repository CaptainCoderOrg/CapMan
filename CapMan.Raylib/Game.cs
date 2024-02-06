public class Game
{
    public CapMan Player { get; } = new CapMan();
    public Board Board { get; } = new Board(Board.StandardBoard);

    public void Update(double delta)
    {
        Player.Update(this, delta);
    }
}