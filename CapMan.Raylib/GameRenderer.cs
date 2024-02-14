namespace CapMan.Raylib;
using Raylib_cs;

public class GameRenderer
{
    public bool ShowBoundingBoxes { get; set; } = false;
    private readonly BoardRenderer _boardRenderer = new();
    private readonly CapManRenderer _capManRenderer = new();
    private readonly EnemyRenderer _blinkusRenderer = new(BlinkusSprites.Searching, BlinkusSprites.Chasing, BlinkusSprites.Fleeing);
    public void Render(Game game, int left, int top)
    {
        _boardRenderer.Render(game.Board, left, top);
        _capManRenderer.Render(game.Player, left, top);
        foreach (EnemyActor enemy in game.Enemies)
        {
            _blinkusRenderer.Render(enemy, left, top);
        }
        RenderBoundingBoxes(game, left, top);
    }

    private void RenderBoundingBoxes(Game game, int left, int top)
    {
        if (!ShowBoundingBoxes) { return; }
        _capManRenderer.RenderBoundingBox(game.Player, left, top);
        foreach (EnemyActor enemy in game.Enemies)
        {
            _blinkusRenderer.RenderBoundingBox(enemy, left, top);
        }
    }
}