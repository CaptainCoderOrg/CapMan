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


// Board board = new (Board.StandardBoard);
Game game = new Game();
BoardRenderer boardRenderer = new ();

int screenWidth = game.Board.Columns * BoardRenderer.CellSize;
int screenHeight = game.Board.Rows * BoardRenderer.CellSize;

Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
Raylib.SetWindowMonitor(1);
Raylib.SetWindowSize(screenWidth, screenHeight);
Raylib.SetTargetFPS(60);

int ticks = 0;

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
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

void RenderCapMan()
{
    
    capmanSprite.Draw((int)(game.Player.X * BoardRenderer.CellSize) - 5, (int)(game.Player.Y * BoardRenderer.CellSize) - 5);
    Raylib.DrawRectangleLines((int)(game.Player.X * BoardRenderer.CellSize), (int)(game.Player.Y * BoardRenderer.CellSize), BoardRenderer.CellSize, BoardRenderer.CellSize, Color.Yellow);

    Raylib.DrawText($"X: {game.Player.X:#.##}, Y: {game.Player.Y:#.##}, {game.Player.CurrentDirection}", 0, 0, 24, Color.White);
    Raylib.DrawText($"Col: {game.Player.Column}, Row: {game.Player.Row}", 0, 24, 24, Color.White);
    Raylib.DrawText($"Current: {game.Player.CurrentDirection}, Next: {game.Player.NextDirection}", 0, 48, 24, Color.White);
}

// void DoThing()
// {
//     capMan.Update(Raylib.GetFrameTime());
//     capmanSprite.Draw((int)(capMan.X * BoardRenderer.CellSize), (int)(capMan.Y * BoardRenderer.CellSize));
// }

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

