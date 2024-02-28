namespace CapMan.Raylib;

using Raylib_cs;

public class MenuScreen : IScreen
{
    public static MenuScreen Shared { get; } = new();
    private static readonly (string Text, Action Action)[] Menu = [
        ("Insert Coin", () => Program.Screen = new GameScreen()),
        ("Exit", Program.Exit),
    ];

    private int _selectedIx = 0;
    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Menu[_selectedIx].Action();
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyPressed(KeyboardKey.S))
        {
            UpdateSelected(1);
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.W))
        {
            UpdateSelected(-1);
        }
    }

    private void UpdateSelected(int amount)
    {
        _selectedIx = (_selectedIx + amount) switch
        {
            var ix when ix < 0 => Menu.Length - 1,
            var ix when ix >= Menu.Length => 0,
            var ix => ix,
        };
    }

    public void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        DrawCenteredText("CapMan!", 32, 48, Color.White);
        const int fontSize = 32;
        int top = 48 * 2 + fontSize;

        for (int ix = 0; ix < Menu.Length; ix++)
        {
            top += fontSize;
            Color color = _selectedIx == ix ? Color.Yellow : Color.White;
            DrawCenteredText(Menu[ix].Text, top, fontSize, color);
        }

        Raylib.EndDrawing();
    }

    private void DrawCenteredText(string text, int top, int fontSize, Color color)
    {
        int width = Raylib.MeasureText(text, fontSize);
        int center = (Raylib.GetScreenWidth() / 2) - (width / 2);
        Raylib.DrawText(text, center, top, fontSize, color);
    }

}