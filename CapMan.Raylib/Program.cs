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


Board board = new (Board.StandardBoard);
BoardRenderer boardRenderer = new ();

int screenWidth = board.Columns * BoardRenderer.CellSize;
int screenHeight = board.Rows * BoardRenderer.CellSize;

Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
Raylib.SetWindowMonitor(1);
Raylib.SetWindowSize(screenWidth, screenHeight);
Raylib.SetTargetFPS(60);


// Main game loop
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    boardRenderer.Render(board, 0, 0);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();