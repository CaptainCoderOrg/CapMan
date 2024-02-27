namespace CapMan;

public class Game : IGame
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
    public PlayerActor Player { get; private set; }
    public EnemyActor[] Enemies { get; private set; }
    public Board Board { get; private set; }
    private readonly Board _originalBoard;
    public int Score { get; private set; }
    public int Level { get; private set; } = 1;
    public int DotsRemaining => Board.CountDots();
    public event Action<GameEvent>? OnEvent;
    private readonly List<IProjectile> _projectiles = new();
    public IReadOnlyList<IProjectile> Projectiles => _projectiles.AsReadOnly();
    public double PoweredUpTime { get; set; } = 10;
    public double PoweredUpTimeRemaining { get; set; } = 0;
    public bool IsPoweredUp => PoweredUpTimeRemaining > 0;
    public int ScoreMultiplier { get; private set; } = 1;

    public Game(string gameInput) : this(gameInput.ReplaceLineEndings().Split(Environment.NewLine)) { }
    public Game(IEnumerable<string> gameInput) : this(ParseActors(gameInput), new Board(gameInput.SkipWhile(IsNotABlankLine).Skip(1))) { }
    public Game(IEnumerable<Actor> actors, Board board)
    {
        Player = actors.OfType<PlayerActor>().Single();
        Enemies = [.. actors.OfType<EnemyActor>()];
        Board = board.Copy();
        _originalBoard = board.Copy();
        foreach (EnemyActor actor in Enemies)
        {
            actor.OnDeath += HandleEnemyDeath;
        }
    }
    private static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s);

    private static IEnumerable<Actor> ParseActors(IEnumerable<string> gameInput)
    {
        Dictionary<string, Actor> actors = [];

        foreach (string line in gameInput.TakeWhile(IsNotABlankLine))
        {
            string[] tokens = line.Split(",()".ToCharArray(), StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (tokens is [string name, string startX, string startY, string speed, string direction, string behaviour, .. string[] behaviourParams])
            {
                Position startPosition = new(double.Parse(startX), double.Parse(startY));
                double startSpeed = double.Parse(speed);
                Direction startDirection = Enum.Parse<Direction>(direction);
                Actor actor;
                if (name.Equals("CapMan", StringComparison.InvariantCultureIgnoreCase))
                {
                    actor = new PlayerActor(startPosition, startSpeed, startDirection);
                }
                else
                {
                    IEnemyBehaviour enemyBehaviour = behaviour.ToLowerInvariant() switch
                    {
                        "kevin" => new KevinAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5]))
                        ),
                        "bob" => new BobAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5]))
                            ),
                        "clyde" => new ClydeAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5]))
                            ),
                        "whimsical" => new WhimsicalAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5])),
                            (EnemyActor)actors[behaviourParams[6]]
                            ),
                        _ => throw new NotImplementedException(),
                    };
                    actor = new EnemyActor(startPosition, startSpeed, startDirection) { Behaviour = enemyBehaviour };
                }
                actors[name] = actor;
            }
        }
        return actors.Values;
    }

    private void HandleEnemyDeath()
    {
        Score += 200 * ScoreMultiplier;
        ScoreMultiplier <<= 1;
    }

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
            Player = new(Player.StartPosition, Player.Speed, Player.StartDirection);
            ResetEnemies();
            //TODO: Reset enemies
            State = GameState.Playing;
            PlayTime = 0;
        }
    }

    public void StartLevel()
    {
        Player = new(Player.StartPosition, Player.Speed, Player.StartDirection);
        ResetEnemies();
        Board = _originalBoard.Copy();
        PoweredUpTimeRemaining = 0;
        PlayTime = 0;
        State = GameState.Playing;
        _projectiles.Clear();
    }

    private void StartNextLevel(double deltaTime)
    {
        StartNextLevelCountDown -= deltaTime;
        if (StartNextLevelCountDown <= 0)
        {
            Level++;
            StartLevel();
        }
    }

    private void Play(double delta)
    {
        Player.Update(this, delta);
        if (CheckEatDots())
        {
            OnEvent?.Invoke(GameEvent.DotEaten);
            CheckLevelComplete();
        }

        _projectiles.RemoveAll(p => p.IsPickedUp);

        foreach (IProjectile projectile in _projectiles)
        {
            projectile.Update(this, delta);
        }

        foreach (EnemyActor enemy in Enemies)
        {
            enemy.Update(this, delta);
            if (!enemy.IsAlive) { continue; }
            if (enemy.BoundingBox().IntersectsWith(Player.BoundingBox()))
            {
                PlayerKilled();
            }
        }
        PoweredUpTimeRemaining = Math.Max(PoweredUpTimeRemaining - delta, 0);
        if (PoweredUpTimeRemaining <= 0)
        {
            _projectiles.Clear();
            Player.CreateProjectile = null;
            ScoreMultiplier = 1;
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
        PoweredUpTimeRemaining = 0;
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
            Player.CreateProjectile = PlayerProjectileExtensions.BowlerHatProjectile;
            PoweredUpTimeRemaining = PoweredUpTime;
            return true;
        }

        return false;
    }

    public void AddProjectile(IProjectile toAdd) => _projectiles.Add(toAdd);
}