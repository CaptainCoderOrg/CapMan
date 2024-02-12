namespace CapMan;

public class Game
{
    public PlayerActor Player { get; } = new();
    public EnemyActor[] Enemies { get; init; }
    public Board Board { get; } = new Board(Board.StandardBoard);
    public int Score { get; private set; }

    public Game(IEnumerable<EnemyActor> enemies)
    {
        Enemies = [.. enemies];
    }

    public void Update(double delta)
    {
        Player.Update(Board, delta);
        CheckEatDots();
        foreach (EnemyActor enemy in Enemies)
        {
            enemy.Update(Board, delta);
        }
    }

    private void CheckEatDots()
    {
        if (Board.IsDot(Player.Tile))
        {
            Board.RemoveElement(Player.Tile);
            Score += 10;
        }

        if (Board.IsPowerPill(Player.Tile))
        {
            Board.RemoveElement(Player.Tile);
            Score += 50;
        }
    }
}