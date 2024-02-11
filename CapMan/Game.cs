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

    private void UpdateCapMan(double delta)
    {
        double distance = Player.Speed * delta;
        Direction nextDirection = Board.NextDirection(Player.CurrentDirection, Player.NextDirection, Player.X, Player.Y, distance);
        if (nextDirection == Player.CurrentDirection)
        {
            (Player.X, Player.Y) = Board.CalculateMove(Player.CurrentDirection, Player.X, Player.Y, distance);
        }
        else
        {
            (Player.X, Player.Y) = Board.CalculateMoveWithTurn(Player.CurrentDirection, Player.NextDirection, Player.X, Player.Y, distance);
            Player.CurrentDirection = nextDirection;
        }

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