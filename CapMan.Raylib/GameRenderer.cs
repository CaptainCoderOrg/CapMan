namespace CapMan.Raylib;

public class GameRenderer
{
    private readonly BoardRenderer _boardRenderer = new();
    private readonly CapManRenderer _capManRenderer = new();

    public void Render(Game game, int left, int top)
    {
        _boardRenderer.Render(game.Board, left, top);
        _capManRenderer.Render(game.Player, left, top);
    }

}