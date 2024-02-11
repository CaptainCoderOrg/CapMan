public class Game
{
    public Actor Player { get; } = new Actor();
    public Board Board { get; } = new Board(Board.StandardBoard);
    public int Score { get; private set; }

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
        BoundsCheck(Player);
    }

    public void BoundsCheck(Actor actor)
    {
        double transitionDistance = 1;
        double width = Board.Columns;
        if (actor.X < -transitionDistance)
        {
            actor.X += width + 2 * transitionDistance;
        }
        else if (actor.X > width + transitionDistance)
        {
            actor.X -= width + 2 * transitionDistance;
        }
    }
}