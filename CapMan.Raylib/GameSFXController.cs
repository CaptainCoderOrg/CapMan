namespace CapMan.Raylib;
using Raylib_cs;

public class GameSFXController
{
    public static GameSFXController Shared { get; } = new();
    private static Sound[]? s_dotSound;
    public static Sound[] DotSound = s_dotSound ??= [Raylib.LoadSound(Path.Combine("assets", "sfx", "dot0.wav")), Raylib.LoadSound(Path.Combine("assets", "sfx", "dot1.wav"))];
    public bool Muted { get; set; } = false;
    private Game? _currentGame;
    private int _dots = 1;
    public Game Game
    {
        get => _currentGame ?? throw new InvalidOperationException("Cannot access Game prior to setting it.");
        set
        {
            Deregister(_currentGame);
            _currentGame = value;
            _currentGame.OnEvent += HandleEvent;
        }
    }

    private void HandleEvent(GameEvent evt)
    {
        if (Muted) { return; }
        if (evt is GameEvent.DotEaten)
        {
            _dots = (_dots + 1) % 2;
            Raylib.PlaySound(DotSound[_dots]);
        }
    }

    private void Deregister(Game? game)
    {
        if (game is null) { return; }
        game.OnEvent -= this.HandleEvent;
    }
}