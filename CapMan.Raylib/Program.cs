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
Raylib.InitWindow(800, 600, "CapMan | Main Window");

// Initialization
//--------------------------------------------------------------------------------------
bool DebugText = false;
bool DrawLines = false;
bool Paused = false;

Console.WriteLine(Environment.CurrentDirectory);

Game game = InitGame();
InfoRenderer infoRenderer = new();
game.Player.Position = new(14, 23);
int boardWidth = game.Board.Width * BoardRenderer.CellSize;
int boardHeight = game.Board.Height * BoardRenderer.CellSize + InfoRenderer.BlockedHeight;
double screenScale = 1;
int screenWidth = (int)(boardWidth * screenScale);
int screenHeight = (int)(boardHeight * screenScale);

Raylib.SetWindowState(ConfigFlags.ResizableWindow);
Raylib.SetWindowMonitor(1);
Raylib.SetWindowSize(screenWidth, screenHeight);

Raylib.SetTargetFPS(60);

GameRenderer gameRenderer = new();
RenderTexture2D boardTexture = Raylib.LoadRenderTexture(boardWidth, boardHeight);
Rectangle screenRect = new(0, 0, boardWidth, -boardHeight);

System.Numerics.Vector2 centerScreen = new(0, 0);
// Main game loop
while (!Raylib.WindowShouldClose())
{
    HandleInput();
    if (!Paused)
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
    Rectangle scaledResolution = new(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    Raylib.DrawTexturePro(boardTexture.Texture, screenRect, scaledResolution, centerScreen, 0, Color.White);
    RenderDebugText();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

void DrawGrid(int left, int top)
{
    if (DrawLines)
    {
        int width = game.Board.Width * BoardRenderer.CellSize;
        int height = game.Board.Height * BoardRenderer.CellSize;
        for (int y = 0; y < game.Board.Height; y++)
        {
            Raylib.DrawLine(left, top + y * BoardRenderer.CellSize, width, y * BoardRenderer.CellSize, Color.DarkGreen);
        }
        for (int x = 0; x < game.Board.Width; x++)
        {
            Raylib.DrawLine(left + x * BoardRenderer.CellSize, 0, top + x * BoardRenderer.CellSize, height, Color.DarkGreen);
        }
    }
}


void RenderDebugText()
{
    if (Raylib.IsKeyPressed(KeyboardKey.I))
    {
        DebugText = !DebugText;
    }
    if (DebugText)
    {
        Raylib.DrawText($"X: {game.Player.Position.X:0.0}, Y: {game.Player.Position.Y:0.0}", 0, 0, 24, Color.White);
        Raylib.DrawText($"BX: {game.Player.Tile.X}, BY: {game.Player.Tile.Y}", 0, 24, 24, Color.White);
        Raylib.DrawText($"Current: {game.Player.CurrentDirection}, Next: {game.Player.NextDirection}", 0, 48, 24, Color.White);
    }
}

Game InitGame()
{
    EnemyActor blinkus1 = new(new Position(14, 11), 4, Direction.Down);
    blinkus1.Behaviour = new TargetPlayerTile();
    EnemyActor blinkus2 = new(new Position(11, 14), 4, Direction.Left);
    blinkus2.Behaviour = new ClydeTargeting();
    EnemyActor blinkus3 = new(new Position(12, 14), 4, Direction.Right);
    blinkus3.Behaviour = new TargetAheadOfPlayer(4);
    EnemyActor blinkus4 = new(new Position(13, 14), 4, Direction.Left);
    blinkus4.Behaviour = new WhimsicalTargeting(blinkus1);
    Game game = new([blinkus1, blinkus2, blinkus3, blinkus4]);
    game.Player.Position = new(14, 23);
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
        game.Player.Speed++;
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Nine))
    {
        game.Player.Speed--;
    }
    if (Raylib.IsKeyPressed(KeyboardKey.G))
    {
        DrawLines = !DrawLines;
    }
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        Paused = !Paused;
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