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

using Raylib_cs;

// Initialization
//--------------------------------------------------------------------------------------
bool DebugText = false;

// Board board = new (Board.StandardBoard);
Game game = new Game();
BoardRenderer boardRenderer = new ();

int screenWidth = game.Board.Columns * BoardRenderer.CellSize;
int screenHeight = game.Board.Rows * BoardRenderer.CellSize;

double lastX = 0;
double lastY = 0;

Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
Raylib.SetWindowMonitor(1);
Raylib.SetWindowSize(screenWidth, screenHeight);
Raylib.SetTargetFPS(60);


SpriteSheet sheet = SpriteSheet.Load("assets/sprites/capman.png", 1, 3);
AnimatedSprite capmanSprite = new AnimatedSprite(sheet, [(0, 0), (0, 1), (0, 2), (0, 1)]);
game.Player.Y = 23; // * BoardRenderer.CellSize;
game.Player.X = 14; // * BoardRenderer.CellSize;

// Main game loop
while (!Raylib.WindowShouldClose())
{
    HandleInput();
    game.Update(Raylib.GetFrameTime());
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    boardRenderer.Render(game.Board, 0, 0);
    RenderCapMan();
    RenderDebugText();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

void RenderCapMan()
{    
    if ((lastX, lastY) != (game.Player.X, game.Player.Y))
    {
        capmanSprite.CurrentTime += Raylib.GetFrameTime();
        (lastX, lastY) = (game.Player.X, game.Player.Y);
    }
    capmanSprite.Rotation = game.Player.CurrentDirection switch {
        Direction.Left => 0,
        Direction.Up => 90,
        Direction.Right => 180,
        Direction.Down => 270,
        _ => throw new Exception($"Unexpected direction {game.Player.CurrentDirection}"),
    };
    capmanSprite.Draw((int)(game.Player.X * BoardRenderer.CellSize) + BoardRenderer.CellSize/2, (int)(game.Player.Y * BoardRenderer.CellSize) + BoardRenderer.CellSize/2);
}

void RenderDebugText()
{
    if (Raylib.IsKeyPressed(KeyboardKey.I))
    {
        DebugText = !DebugText;
    }
    if (DebugText)
    {
        Raylib.DrawText($"X: {game.Player.X:#.##}, Y: {game.Player.Y:#.##}, {game.Player.CurrentDirection}", 0, 0, 24, Color.White);
        Raylib.DrawText($"Col: {game.Player.Column}, Row: {game.Player.Row}", 0, 24, 24, Color.White);
        Raylib.DrawText($"Current: {game.Player.CurrentDirection}, Next: {game.Player.NextDirection}", 0, 48, 24, Color.White);
    }
}


void HandleInput()
{
    if (Raylib.IsKeyDown(KeyboardKey.W))
    {
        game.Player.NextDirection = Direction.Up;
    }
    if (Raylib.IsKeyDown(KeyboardKey.D))
    {
        game.Player.NextDirection = Direction.Right;
    }
    if (Raylib.IsKeyDown(KeyboardKey.A))
    {
        game.Player.NextDirection = Direction.Left;
    }
    if (Raylib.IsKeyDown(KeyboardKey.S))
    {
        game.Player.NextDirection = Direction.Down;
    }
}

