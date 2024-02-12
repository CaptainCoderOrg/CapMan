namespace CapMan.Raylib;

public class GameRenderer
{
    private readonly BoardRenderer _boardRenderer = new();
    private readonly CapManRenderer _capManRenderer = new();
    private readonly EnemyRenderer _enemyRenderer = new();

    public void Render(Game game, int left, int top)
    {
        _boardRenderer.Render(game.Board, left, top);
        _capManRenderer.Render(game.Player, left, top);
        foreach (EnemyActor enemy in game.Enemies)
        {
            _enemyRenderer.Render(enemy, left, top);
        }
    }

}