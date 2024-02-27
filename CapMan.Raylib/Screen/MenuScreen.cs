namespace CapMan.Raylib;

using Raylib_cs;

public class MenuScreen : IScreen
{
    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Program.Screen = new GameScreen();
        }
    }

    public void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawText("CapMan!", 0, 0, 16, Color.White);
        Raylib.DrawText("Press Enter to Insert Coin", 0, 32, 16, Color.White);
        Raylib.EndDrawing();
    }

}