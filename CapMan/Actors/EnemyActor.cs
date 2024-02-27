namespace CapMan;

public class EnemyActor(Position position, double speed, Direction direction) : Actor(position, speed, direction)
{
    public EnemyState State { get; set; } = EnemyState.Searching;
    public IEnemyBehaviour Behaviour { get; set; } = new TargetTileBehaviour(new Tile(1, 1));
    public Tile? LastTarget { get; set; }
    public bool IgnoreDoors { get; set; } = false;
    private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            if (_isAlive == value) { return; }
            _isAlive = value;
            if (_isAlive == false)
            {
                OnDeath?.Invoke();
            }
        }
    }
    private bool _isFleeing = false;
    public bool IsFleeing
    {
        get => _isFleeing;
        set
        {
            if (_isFleeing == value) { return; }
            _switchDirection = value;
            _isFleeing = value;
        }
    }

    public Action? OnDeath { get; set; }
    private bool _switchDirection = false;
    public override void Update(IGame game, double deltaTime)
    {
        IsFleeing = game.IsPoweredUp;
        if (_switchDirection)
        {
            CurrentDirection = CurrentDirection.Opposite();
            _switchDirection = false;
        }
        if (CollidingWithProjectile(game))
        {
            IsAlive = false;
        }
        if (IgnoreDoors)
        {
            game = new DelegateBoardGame(game, game.Board.WithoutDoors());
        }
        NextDirection = Behaviour.GetNextDirection(game, deltaTime, this);

        base.Update(game, deltaTime);
    }

    private bool CollidingWithProjectile(IGame game) =>
        game.Projectiles
            .Where(p => p.IsLethal)
            .Select(p => p.BoundingBox())
            .Any(p => p.IntersectsWith(this.BoundingBox()));

    public void Reset()
    {
        Position = StartPosition;
        IsAlive = true;
    }

    private class DelegateBoardGame(IGame baseGame, Board board) : IGame
    {
        public IGame DelegateGame { get; set; } = baseGame;
        public Board Board { get; } = board;

        public GameState State { get => DelegateGame.State; set => DelegateGame.State = value; }
        public double RespawnTime => DelegateGame.RespawnTime;
        public double RespawnCountDown => DelegateGame.RespawnCountDown;
        public int Lives { get => DelegateGame.Lives; set => DelegateGame.Lives = value; }
        public PlayerActor Player => DelegateGame.Player;
        public EnemyActor[] Enemies => DelegateGame.Enemies;
        public int Score => DelegateGame.Score;
        public double PlayTime => DelegateGame.PlayTime;
        public IReadOnlyList<IProjectile> Projectiles => DelegateGame.Projectiles;
        public double PoweredUpTime => DelegateGame.PoweredUpTime;
        public bool IsPoweredUp => DelegateGame.IsPoweredUp;
        public int Level => DelegateGame.Level;
        public void Update(double delta) => DelegateGame.Update(delta);
    }
}