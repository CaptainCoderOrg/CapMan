namespace CapMan;

public class GameBuilder
{
    private readonly Dictionary<string, Actor> _actors = [];
    private Board? _board;
    private string[] _gameConfig = [];

    public GameBuilder AddBoard(string layout) => AddBoard(layout.ReplaceLineEndings().Split(Environment.NewLine));

    public GameBuilder AddBoard(string[] layout)
    {
        _board = new(layout);
        return this;
    }

    public GameBuilder AddBoard(Board board)
    {
        _board = board;
        return this;
    }

    public GameBuilder AddEnemies(string enemies) => AddEnemies(enemies.ReplaceLineEndings().Split(Environment.NewLine));
    public GameBuilder AddEnemies(string[] enemies)
    {
        foreach (string enemy in enemies)
        {
            _ = AddEnemy(enemy);
        }
        return this;
    }
    public GameBuilder AddEnemies(IEnumerable<EnemyActor> enemies)
    {
        foreach (EnemyActor enemy in enemies)
        {
            _actors.Add(Guid.NewGuid().ToString(), enemy);
        }
        return this;
    }

    public GameBuilder AddEnemy(string? actorString)
    {
        if (!string.IsNullOrEmpty(actorString))
        {
            string[] tokens = actorString.Split(",()".ToCharArray(), StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (tokens is [string name, string startX, string startY, string speed, string direction, string behaviour, .. string[] behaviourParams])
            {
                Position startPosition = new(double.Parse(startX), double.Parse(startY));
                double startSpeed = double.Parse(speed);
                Direction startDirection = Enum.Parse<Direction>(direction);
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
                        (EnemyActor)_actors[behaviourParams[6]]
                        ),
                    _ => throw new NotImplementedException(),
                };
                EnemyActor enemy = new(startPosition, startSpeed, startDirection) { Behaviour = enemyBehaviour };
                _actors.Add(name, enemy);
            }
        }
        return this;
    }

    public GameBuilder AddEnemy(string name, EnemyActor enemy)
    {
        _actors.Add(name, enemy);
        return this;
    }

    public GameBuilder AddPlayer(string? playerString)
    {
        if (!string.IsNullOrEmpty(playerString))
        {
            string[] tokens = playerString.Split(",()".ToCharArray(), StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (tokens is [string name, string startX, string startY, string speed, string direction])
            {
                Position startPosition = new(double.Parse(startX), double.Parse(startY));
                double startSpeed = double.Parse(speed);
                Direction startDirection = Enum.Parse<Direction>(direction);
                Actor player;
                player = new PlayerActor(startPosition, startSpeed, startDirection);
                _actors.Add(name, player);
            }
        }
        return this;
    }

    public GameBuilder AddPlayer(PlayerActor player)
    {
        _actors.Add("CapMan", player);
        return this;
    }

    public GameBuilder Configure(string gameConfig)
    {
        _gameConfig = gameConfig.ReplaceLineEndings().Split(Environment.NewLine);
        return this;
    }

    public GameBuilder Configure(string[] gameConfig)
    {
        _gameConfig = gameConfig;
        return this;
    }

    public Game Build()
    {
        if (_gameConfig.Length != 0)
        {
            return new Game(_gameConfig);
        }

        ArgumentNullException.ThrowIfNull(_board, "board");
        ArgumentNullException.ThrowIfNull(_actors.Values.OfType<PlayerActor>().SingleOrDefault(), "player");

        return new(_actors.Values, _board);
    }
}
