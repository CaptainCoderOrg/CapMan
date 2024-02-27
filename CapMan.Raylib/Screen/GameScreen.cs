namespace CapMan.Raylib;

using System.Numerics;

using Raylib_cs;

public class GameScreen : IScreen
{
    public Game CurrentGame { get; private set; }
    public Func<Game> InitGame { get; init; } = DefaultGameInitializer;
    private readonly GameRenderer _gameRenderer = new();
    private bool _drawLines = false;
    private bool _paused = false;
    private bool _debugText = false;
    public int Width => CurrentGame.Board.Width * BoardRenderer.CellSize;
    public int Height => CurrentGame.Board.Height * BoardRenderer.CellSize + InfoRenderer.BlockedHeight + InfoRenderer.LivesSpriteHeight;

    private RenderTexture2D _boardTexture; // = Raylib.LoadRenderTexture(Width, Height);
    private Rectangle _screenRect; // => new(0, 0, Width, -Height);
    private Rectangle _scaleRect; // => GetScaledResolution();
    private Vector2 _centerScreen; // = new(0, 0);

    public GameScreen()
    {
        CurrentGame = InitGame();
        _boardTexture = Raylib.LoadRenderTexture(Width, Height);
        _screenRect = new(0, 0, Width, -Height);
        _scaleRect = GetScaledResolution();
        _centerScreen = new(0, 0);
    }

    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (CurrentGame.Player.CreateProjectile is not null)
            {
                Projectile created = CurrentGame.Player.CreateProjectile(CurrentGame.Player);
                CurrentGame.Player.CreateProjectile = null;
                CurrentGame.AddProjectile(created);
            }
        }

        if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
        {
            CurrentGame.Player.NextDirection = Direction.Up;
        }
        if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
        {
            CurrentGame.Player.NextDirection = Direction.Right;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
        {
            CurrentGame.Player.NextDirection = Direction.Left;
        }
        if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
        {
            CurrentGame.Player.NextDirection = Direction.Down;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Zero))
        {
            CurrentGame.Player.Speed = Math.Min(++CurrentGame.Player.Speed, GameConstants.DebugMaxGameSpeed);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Nine))
        {
            CurrentGame.Player.Speed = Math.Max(--CurrentGame.Player.Speed, 0);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.G))
        {
            _drawLines = !_drawLines;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.P))
        {
            _paused = !_paused;
        }
        if (Raylib.IsKeyDown(KeyboardKey.R))
        {
            CurrentGame = InitGame();
        }
        if (Raylib.IsKeyPressed(KeyboardKey.B))
        {
            _gameRenderer.ShowBoundingBoxes = !_gameRenderer.ShowBoundingBoxes;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.T))
        {
            _gameRenderer.ShowTargetTiles = !_gameRenderer.ShowTargetTiles;
        }

        if (!_paused)
        {
            CurrentGame.Update(Raylib.GetFrameTime());
        }
    }

    public void Render()
    {
        if (Raylib.IsWindowResized())
        {
            _scaleRect = GetScaledResolution();
            _centerScreen = GetBoardCenterOffset();
        }
        Raylib.BeginTextureMode(_boardTexture);
        Raylib.ClearBackground(Color.Black);
        InfoRenderer.Shared.Render(CurrentGame, 0, 0);
        GameRenderer.Shared.Render(CurrentGame, 0, InfoRenderer.BlockedHeight);
        DrawGrid(0, InfoRenderer.BlockedHeight);
        Raylib.EndTextureMode();

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawTexturePro(_boardTexture.Texture, _screenRect, _scaleRect, _centerScreen, 0, Color.White);
        RenderDebugText();
        Raylib.EndDrawing();
    }

    private static Game DefaultGameInitializer()
    {
        string gameInit = $"""
        CapMan         , (14, 23), 8, Left , manual
        kevinEnemy     , (14, 11), 8, Down , Kevin    , (12, 13), (12, 15), (13, 11)
        clydeEnemy     , (11, 14), 8, Left , Clyde    , (11, 13), (11, 15), (13, 11)
        targetAhead    , (13, 15), 8, Right, Bob      , (13, 15), (13, 13), (13, 11)
        whimsicalEnemy , (16, 14), 8, Left , Whimsical, (16, 15), (16, 13), (13, 11), kevinEnemy
        
        {Board.StandardBoard}
        """;

        Game game = new(gameInit);
        GameSFXController.Shared.Game = game;
        return game;
    }

    private void DrawGrid(int left, int top)
    {
        if (_drawLines)
        {
            int width = CurrentGame.Board.Width * BoardRenderer.CellSize;
            int height = CurrentGame.Board.Height * BoardRenderer.CellSize;
            for (int y = 0; y < CurrentGame.Board.Height; y++)
            {
                Raylib.DrawLine(left, top + y * BoardRenderer.CellSize, width, top + y * BoardRenderer.CellSize, Color.DarkGreen);
            }
            for (int x = 0; x < CurrentGame.Board.Width; x++)
            {
                Raylib.DrawLine(left + x * BoardRenderer.CellSize, top, left + x * BoardRenderer.CellSize, top + height, Color.DarkGreen);
            }
        }
    }

    void RenderDebugText()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.I))
        {
            _debugText = !_debugText;
        }
        if (_debugText)
        {
            Raylib.DrawText($"X: {CurrentGame.Player.Position.X:0.0}, Y: {CurrentGame.Player.Position.Y:0.0}", 0, 0, 24, Color.White);
            Raylib.DrawText($"BX: {CurrentGame.Player.Tile.X}, BY: {CurrentGame.Player.Tile.Y}", 0, 24, 24, Color.White);
            Raylib.DrawText($"Current: {CurrentGame.Player.CurrentDirection}, Next: {CurrentGame.Player.NextDirection}", 0, 48, 24, Color.White);
            Raylib.DrawText($"HasProjectile: {CurrentGame.Player.HasProjectile}", 0, 72, 24, Color.White);
            Raylib.DrawText($"Power Up Time Remaining: {CurrentGame.PoweredUpTimeRemaining:0.00}", 0, 96, 24, Color.White);
        }
    }

    /// <summary>
    /// Calculates the scaled resolution of the board based on the size of the game
    /// window.
    /// </summary>
    private Rectangle GetScaledResolution()
    {
        (double screenWidth, double screenHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        double ratio = (double)Width / Height;
        double width = screenHeight * ratio;
        double height = screenWidth / ratio;

        double widthDiff = screenWidth - width;
        double heightDiff = screenHeight - height;

        if (widthDiff < heightDiff)
        {
            width = screenWidth;
        }
        else
        {
            height = screenHeight;
        }

        Rectangle scaledResolution = new(0, 0, (int)width, (int)height);

        return scaledResolution;
    }

    /// <summary>
    /// Calculates the offset to position the board in the center of the game
    /// window.
    /// </summary>
    private Vector2 GetBoardCenterOffset()
    {
        (double screenWidth, double screenHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        float x = (float)(Width - screenWidth) * 0.5f;
        float y = (float)(Height - screenHeight) * 0.5f;
        return new Vector2(x, y);
    }
}