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

// Initialization
//--------------------------------------------------------------------------------------
bool DebugText = false;
bool DrawLines = false;
bool Paused = false;

// Board board = new (Board.StandardBoard);
Game game = new Game();
game.Player.Y = 23; // * BoardRenderer.CellSize;
game.Player.X = 14; // * BoardRenderer.CellSize;
BoardRenderer boardRenderer = new();

int boardWidth = game.Board.Columns * BoardRenderer.CellSize;
int boardHeight = game.Board.Rows * BoardRenderer.CellSize;
Console.WriteLine($"{boardWidth}x{boardHeight}");
int screenWidth = (int)(boardWidth * 1.5);
int screenHeight = (int)(boardHeight * 1.5);

Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
Raylib.SetWindowState(ConfigFlags.ResizableWindow);
Raylib.SetWindowMonitor(1);
Raylib.SetWindowSize(screenWidth, screenHeight);
Raylib.SetTargetFPS(60);

CapManRenderer capManRenderer = new();

RenderTexture2D boardTexture = Raylib.LoadRenderTexture(boardWidth, boardHeight);
Rectangle screenRect = new Rectangle(0, 0, boardWidth, -boardHeight);

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
    boardRenderer.Render(game.Board, 0, 0);
    capManRenderer.Render(game);
    DrawGrid();
    Raylib.EndTextureMode();

    Raylib.BeginDrawing();
    Rectangle scaledResolution = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    Raylib.DrawTexturePro(boardTexture.Texture, screenRect, scaledResolution, centerScreen, 0, Color.White);
    RenderDebugText();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

void DrawGrid()
{
    if (DrawLines)
    {
        int width = game.Board.Columns * BoardRenderer.CellSize;
        int height = game.Board.Rows * BoardRenderer.CellSize;
        for (int row = 0; row < game.Board.Rows; row++)
        {
            Raylib.DrawLine(0, row * BoardRenderer.CellSize, width, row * BoardRenderer.CellSize, Color.DarkGreen);
        }
        for (int col = 0; col < game.Board.Columns; col++)
        {
            Raylib.DrawLine(col * BoardRenderer.CellSize, 0, col * BoardRenderer.CellSize, height, Color.DarkGreen);
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
        Raylib.DrawText($"X: {game.Player.X:0.0}, Y: {game.Player.Y:0.0}", 0, 0, 24, Color.White);
        Raylib.DrawText($"Col: {game.Player.Column}, Row: {game.Player.Row}", 0, 24, 24, Color.White);
        Raylib.DrawText($"Current: {game.Player.CurrentDirection}, Next: {game.Player.NextDirection}", 0, 48, 24, Color.White);
    }
}

void Reset()
{
    game = new Game();
    game.Player.Y = 23; // * BoardRenderer.CellSize;
    game.Player.X = 14; // * BoardRenderer.CellSize;
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
        Reset();
    }
}