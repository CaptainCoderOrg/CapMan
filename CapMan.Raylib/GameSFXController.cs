namespace CapMan.Raylib;

using Raylib_cs;

public class GameSFXController
{
    public static GameSFXController Shared { get; } = new();
    private static Sound? s_dotSound;
    public static Sound DotSound = s_dotSound ??= Raylib.LoadSound(Path.Combine("assets", "sfx", "dot.wav"));
    private Game? _currentGame;
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
        if (evt is GameEvent.DotEaten && !Raylib.IsSoundPlaying(DotSound))
        {
            Raylib.PlaySound(DotSound);
        }
    }

    private void Deregister(Game? game)
    {
        if (game is null) { return; }
        game.OnEvent -= this.HandleEvent;
    }
}