namespace CapMan.Raylib;

using Raylib_cs;

public class MenuScreen(string title, IEnumerable<MenuEntry> items) : IScreen
{
    const int ItemFontSize = 32;
    const int TitleFontSize = 48;
    private readonly MenuEntry[] _items = [.. items];
    private readonly string _title = title;
    private int _selectedIx = 0;
    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            _items[_selectedIx].OnSelect.Invoke();
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
            var ix when ix < 0 => _items.Length - 1,
            var ix when ix >= _items.Length => 0,
            var ix => ix,
        };
    }

    public void Render()
    {
        int menuHeight = TitleFontSize * 2 + ItemFontSize * _items.Length;
        int center = Raylib.GetScreenHeight() / 2;
        int top = center - menuHeight / 2;
        DrawCenteredText(_title, top, TitleFontSize, Color.White);

        top += TitleFontSize;
        for (int ix = 0; ix < _items.Length; ix++)
        {
            top += ItemFontSize;
            Color color = _selectedIx == ix ? Color.Yellow : Color.White;
            DrawCenteredText(_items[ix].Text, top, ItemFontSize, color);
        }
    }

    private void DrawCenteredText(string text, int top, int fontSize, Color color)
    {
        int width = Raylib.MeasureText(text, fontSize);
        int center = (Raylib.GetScreenWidth() / 2) - (width / 2);
        Raylib.DrawText(text, center + 2, top + 2, fontSize, Color.Black);
        Raylib.DrawText(text, center, top, fontSize, color);
    }

}