/*******************************************************************************************
*
*   raylib [core] example - Basic window
*
*   Welcome to raylib!
*
*   To test examples, just press F6 and execute raylib_compile_execute script
*   Note that compiled executable is placed in the same folder as .c file
*
*   You can find all basic examples on C:\raylib\raylib\examples folder or
*   raylib official webpage: www.raylib.com
*
*   Enjoy using raylib. :)
*
*   This example has been created using raylib 1.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013-2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/
using System.Numerics;

Raylib.InitWindow(0, 0, "CapMan | Main Window");
Raylib.InitAudioDevice();

// Initialization
//--------------------------------------------------------------------------------------
bool debugText = false;
bool drawLines = false;
bool paused = false;

Console.WriteLine(Environment.CurrentDirectory);

Game game = InitGame();
InfoRenderer infoRenderer = new();
game.Player.Position = new(14, 23);
int boardWidth = game.Board.Width * BoardRenderer.CellSize;
int boardHeight = game.Board.Height * BoardRenderer.CellSize + InfoRenderer.BlockedHeight;

InitWindow(1); // Monitor 1 is Captain Coder's Streaming Monitor (hack)
Raylib.SetTargetFPS(60);

GameRenderer gameRenderer = new();
RenderTexture2D boardTexture = Raylib.LoadRenderTexture(boardWidth, boardHeight);
Rectangle screenRect = new(0, 0, boardWidth, -boardHeight);
Rectangle scaleRect = GetScaledResolution();

Vector2 centerScreen = new(0, 0);
// Main game loop
while (!Raylib.WindowShouldClose())
{
    if (Raylib.IsWindowResized())
    {
        scaleRect = GetScaledResolution();
        centerScreen = GetBoardCenterOffset();
    }
    HandleInput();
    if (!paused)
    {
        game.Update(Raylib.GetFrameTime());
    }
    Raylib.BeginTextureMode(boardTexture);
    Raylib.ClearBackground(Color.Black);
    infoRenderer.Render(game, 0, 0);
    gameRenderer.Render(game, 0, InfoRenderer.BlockedHeight);
    DrawGrid(0, InfoRenderer.BlockedHeight);
    Raylib.EndTextureMode();

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawTexturePro(boardTexture.Texture, screenRect, scaleRect, centerScreen, 0, Color.White);
    RenderDebugText();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

/// <summary>
/// Initializes the game window on to the specified monitor and centers
/// it on the screen
/// </summary>
void InitWindow(int monitor)
{
    double initialScale = 1;
    int screenWidth = (int)(boardWidth * initialScale);
    int screenHeight = (int)(boardHeight * initialScale);

    Raylib.SetWindowState(ConfigFlags.ResizableWindow);
    Raylib.SetWindowSize(screenWidth, screenHeight);
    Raylib.SetWindowMinSize(boardWidth, boardHeight);

    (int mWidth, int mHeight) = (Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
    (int wWidth, int wHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

    Raylib.SetWindowPosition((mWidth - wWidth) / 2, (mHeight - wHeight) / 2);
    Raylib.SetWindowMonitor(monitor);
}

/// <summary>
/// Calculates the offset to position the board in the center of the game
/// window.
/// </summary>
Vector2 GetBoardCenterOffset()
{
    (double screenWidth, double screenHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    float x = (float)(scaleRect.Width - screenWidth) * 0.5f;
    float y = (float)(scaleRect.Height - screenHeight) * 0.5f;
    return new Vector2(x, y);
}

/// <summary>
/// Calculates the scaled resolution of the board based on the size of the game
/// window.
/// </summary>
Rectangle GetScaledResolution()
{
    (double screenWidth, double screenHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    double ratio = (double)boardWidth / boardHeight;
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

void DrawGrid(int left, int top)
{
    if (drawLines)
    {
        int width = game.Board.Width * BoardRenderer.CellSize;
        int height = game.Board.Height * BoardRenderer.CellSize;
        for (int y = 0; y < game.Board.Height; y++)
        {
            Raylib.DrawLine(left, top + y * BoardRenderer.CellSize, width, top + y * BoardRenderer.CellSize, Color.DarkGreen);
        }
        for (int x = 0; x < game.Board.Width; x++)
        {
            Raylib.DrawLine(left + x * BoardRenderer.CellSize, top, left + x * BoardRenderer.CellSize, top + height, Color.DarkGreen);
        }
    }
}


void RenderDebugText()
{
    if (Raylib.IsKeyPressed(KeyboardKey.I))
    {
        debugText = !debugText;
    }
    if (debugText)
    {
        Raylib.DrawText($"X: {game.Player.Position.X:0.0}, Y: {game.Player.Position.Y:0.0}", 0, 0, 24, Color.White);
        Raylib.DrawText($"BX: {game.Player.Tile.X}, BY: {game.Player.Tile.Y}", 0, 24, 24, Color.White);
        Raylib.DrawText($"Current: {game.Player.CurrentDirection}, Next: {game.Player.NextDirection}", 0, 48, 24, Color.White);
    }
}

Game InitGame()
{
    string gameInit = $"""
        CapMan         , 14, 23, 8, Left , manual
        targetsPlayer  , 14, 11, 4, Down , TargetPlayerTile
        clydeEnemy     , 11, 14, 4, Left , Clyde    , (11, 13), (11, 15), (13, 11)
        targetAhead    , 13, 15, 4, Right, Bob      , (13, 15), (13, 13), (13, 11)
        whimsicalEnemy , 16, 14, 4, Left , Whimsical, (16, 15), (16, 13), (13, 11), targetsPlayer
        
        {Board.StandardBoard}
        """;

    Game game = GameParser.Create(gameInit);
    GameSFXController.Shared.Game = game;
    return game;
}

void HandleInput()
{
    if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
    {
        game.Player.NextDirection = Direction.Up;
    }
    if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
    {
        game.Player.NextDirection = Direction.Right;
    }
    if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
    {
        game.Player.NextDirection = Direction.Left;
    }
    if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
    {
        game.Player.NextDirection = Direction.Down;
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Zero))
    {
        game.Player.Speed = Math.Min(++game.Player.Speed, GameConstants.DebugMaxGameSpeed);
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Nine))
    {
        game.Player.Speed = Math.Max(--game.Player.Speed, 0);
    }
    if (Raylib.IsKeyPressed(KeyboardKey.G))
    {
        drawLines = !drawLines;
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        paused = !paused;
    }
    if (Raylib.IsKeyDown(KeyboardKey.R))
    {
        game = InitGame();
    }
    if (Raylib.IsKeyPressed(KeyboardKey.B))
    {
        gameRenderer.ShowBoundingBoxes = !gameRenderer.ShowBoundingBoxes;
    }
    if (Raylib.IsKeyPressed(KeyboardKey.T))
    {
        gameRenderer.ShowTargetTiles = !gameRenderer.ShowTargetTiles;
    }
}