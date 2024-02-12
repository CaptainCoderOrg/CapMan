namespace CapMan;

public class Game
{
    public Actor Player { get; } = new Actor();
    public Actor[] Enemies { get; init; }
    public Board Board { get; } = new Board(Board.StandardBoard);
    public int Score { get; private set; }

    public Game(IEnumerable<Actor> enemies)
    {
        Enemies = [.. enemies];
    }

    public void Update(double delta)
    {
        UpdateCapMan(delta);
        CheckEatDots();
    }

    private void CheckEatDots()
    {
        if (Board.IsDot(Player.Row, Player.Column))
        {
            Board.RemoveElement(Player.Row, Player.Column);
            Score += 10;
        }

        if (Board.IsPowerPill(Player.Row, Player.Column))
        {
            Board.RemoveElement(Player.Row, Player.Column);
            Score += 50;
        }
    }

    private void UpdateCapMan(double deltaTime)
    {
        (Player.X, Player.Y, Player.CurrentDirection) = Board.CalculateActorMove(deltaTime, Player);
        (Player.X, Player.Y) = Board.BoundsCheck(Player);
    }
}