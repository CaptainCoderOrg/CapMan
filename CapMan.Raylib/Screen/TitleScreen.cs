namespace CapMan.Raylib;

using Raylib_cs;

public class TitleScreen : IScreen
{
    public static TitleScreen Shared { get; } = new();
    private static readonly MenuScreen Menu = new("Cap Man!", [
        new MenuEntry("Insert Coin", () => Program.Screen = new GameScreen()),
        new MenuEntry("Exit", Program.Exit),
    ]);

    public void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Menu.Render();
        Raylib.EndDrawing();
    }

    public void HandleUserInput()
    {
        Menu.HandleUserInput();
    }
}