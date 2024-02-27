/*******************************************************************************************
*
*   raylib [core] example - Basic wind
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
InitWindow(0); // Monitor 1 is Captain Coder's Streaming Monitor (hack)
Raylib.InitAudioDevice();

// Initialization
//--------------------------------------------------------------------------------------
Console.WriteLine(Environment.CurrentDirectory);
IScreen screen = new GameScreen();

Raylib.SetTargetFPS(60);

// Main game loop
while (!Raylib.WindowShouldClose())
{
    screen.HandleUserInput();
    screen.Render();
}

Raylib.CloseWindow();

/// <summary>
/// Initializes the game window on to the specified monitor and centers
/// it on the screen
/// </summary>
static void InitWindow(int monitor)
{
    Raylib.InitWindow(GameConstants.DefaultScreenWidth, GameConstants.DefaultScreenHeight, "CapMan | Main Window");
    Raylib.SetWindowState(ConfigFlags.ResizableWindow);
    Raylib.SetWindowMinSize(GameConstants.DefaultScreenWidth, GameConstants.DefaultScreenHeight);

    (int mWidth, int mHeight) = (Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
    (int wWidth, int wHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

    Raylib.SetWindowPosition((mWidth - wWidth) / 2, (mHeight - wHeight) / 2);
    Raylib.SetWindowMonitor(monitor);
}