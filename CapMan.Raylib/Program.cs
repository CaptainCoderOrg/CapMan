public class Program
{

    public static IScreen Screen { get; set; } = new MenuScreen();

    public static void Main()
    {
        InitWindow(0);
        Raylib.InitAudioDevice();

        Raylib.SetTargetFPS(60);

        // Main game loop
        while (!Raylib.WindowShouldClose())
        {
            Screen.HandleUserInput();
            Screen.Render();
        }

        Raylib.CloseWindow();


    }

    /// <summary>
    /// Initializes the game window on to the specified monitor and centers
    /// it on the screen
    /// </summary>
    private static void InitWindow(int monitor)
    {
        Raylib.InitWindow(GameConstants.DefaultScreenWidth, GameConstants.DefaultScreenHeight, "CapMan | Main Window");
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
        Raylib.SetWindowMinSize(GameConstants.DefaultScreenWidth, GameConstants.DefaultScreenHeight);

        (int mWidth, int mHeight) = (Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
        (int wWidth, int wHeight) = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        Raylib.SetWindowPosition((mWidth - wWidth) / 2, (mHeight - wHeight) / 2);
        Raylib.SetWindowMonitor(monitor);
    }

}