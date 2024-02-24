namespace CapMan;

public class Game(IEnumerable<EnemyActor> enemies, Board board) : IGame
{
    public GameState State { get; set; } = GameState.Playing;
    public double RespawnTime { get; } = 2.0;
    public double StartNextLevelTime { get; } = 3.0;
    public double PlayTime { get; private set; } = 0.0;
    // Run at least 30 frames per second
    public double MaxDeltaTime { get; } = 1.0 / 30.0;
    public double RespawnCountDown { get; private set; } = 0;
    public double StartNextLevelCountDown { get; private set; } = 0;
    public int Lives { get; set; } = 3;
    public PlayerActor Player { get; private set; } = new();
    public EnemyActor[] Enemies { get; private set; } = [.. enemies];
    public Board Board { get; private set; } = board.Copy();
    private readonly Board _originalBoard = board.Copy();
    public int Score { get; private set; }
    public int Level { get; private set; } = 1;
    public int DotsRemaining => Board.CountDots();

    private void ResetEnemies()
    {
        foreach (EnemyActor enemy in Enemies)
        {
            enemy.Reset();
        }
    }

    public void Update(double deltaTime)
    {
        while (deltaTime > 0)
        {
            double simulateTime = Math.Min(deltaTime, MaxDeltaTime);
            Update_(simulateTime);
            deltaTime -= simulateTime;
        }
    }

    private void Update_(double deltaTime)
    {
        Action<double> action = State switch
        {
            GameState.Playing => Play,
            GameState.Respawning => Respawning,
            GameState.LevelComplete => StartNextLevel,
            GameState.Paused => DoNothing,
            GameState.GameOver => DoNothing,
            _ => throw new Exception($"Encountered unknown GameState: {State}")
        };
        action.Invoke(deltaTime);
        PlayTime += deltaTime;
    }

    private void DoNothing(double _) { }

    private void Respawning(double delta)
    {
        RespawnCountDown -= delta;
        if (RespawnCountDown <= 0)
        {
            Player = new();
            ResetEnemies();
            //TODO: Reset enemies
            State = GameState.Playing;
            PlayTime = 0;
        }
    }

    private void StartNextLevel(double deltaTime)
    {
        StartNextLevelCountDown -= deltaTime;
        if (StartNextLevelCountDown <= 0)
        {
            Level++;
            Player = new();
            ResetEnemies();
            Board = _originalBoard.Copy();
            PlayTime = 0;
            State = GameState.Playing;
        }
    }

    private void Play(double delta)
    {
        Player.Update(this, delta);
        if (CheckEatDots())
        {
            CheckLevelComplete();
        }
        foreach (EnemyActor enemy in Enemies)
        {
            enemy.Update(this, delta);
            if (enemy.BoundingBox().IntersectsWith(Player.BoundingBox()))
            {
                PlayerKilled();
            }
        }
    }

    private void CheckLevelComplete()
    {
        if (DotsRemaining == 0)
        {
            State = GameState.LevelComplete;
            StartNextLevelCountDown = StartNextLevelTime;
        }
    }

    public void PlayerKilled()
    {
        RespawnCountDown = RespawnTime;
        Lives--;
        State = GameState.Respawning;
        if (Lives <= 0)
        {
            State = GameState.GameOver;
        }
    }

    private bool CheckEatDots()
    {
        if (Board.IsDot(Player.Tile))
        {
            Board.RemoveElement(Player.Tile);
            Score += 10;
            return true;
        }

        if (Board.IsPowerPill(Player.Tile))
        {
            Board.RemoveElement(Player.Tile);
            Score += 50;
            return true;
        }

        return false;
    }
}