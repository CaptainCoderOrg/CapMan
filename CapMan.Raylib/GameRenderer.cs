namespace CapMan.Raylib;
using Raylib_cs;

public class GameRenderer
{
    public const int FontSize = 20;
    public bool ShowBoundingBoxes { get; set; } = false;
    public bool ShowTargetTiles { get; set; } = false;

    private readonly BoardRenderer _boardRenderer = new();
    private readonly CapManRenderer _capManRenderer = new();
    private readonly BowlerHatRenderer _bowlerHatRenderer = new();
    private readonly EnemyRenderer _blinkusRenderer = new(BlinkusSprites.Searching, BlinkusSprites.Chasing, BlinkusSprites.Fleeing);

    public void Render(Game game, int left, int top)
    {
        _boardRenderer.Render(game.Board, left, top);
        _capManRenderer.Render(game.Player, left, top);
        foreach (Projectile projectile in game.Projectiles)
        {
            _bowlerHatRenderer.Render(projectile, left, top);
        }
        foreach (EnemyActor enemy in game.Enemies)
        {
            _blinkusRenderer.Render(enemy, left, top);
        }
        RenderBoundingBoxes(game, left, top);
        RenderTargetTiles(game, left, top);
        RenderText(game, left, top);
    }

    private void RenderText(Game game, int left, int top)
    {
        Action action = game.State switch
        {
            GameState.Respawning => () => CenterTextOnBoard($"Respawning in: {game.RespawnCountDown:0.0}"),
            GameState.GameOver => () => CenterTextOnBoard("Game Over"),
            GameState.LevelComplete => () => CenterTextOnBoard($"Next Level in {game.StartNextLevelCountDown:0.0}"),
            _ => () => { }
        };
        action.Invoke();

        void CenterTextOnBoard(string text)
        {
            double centerX = game.Board.Width * BoardRenderer.CellSize * 0.5;
            double centerY = game.Board.Height * BoardRenderer.CellSize * 0.5;
            int x = (int)(centerX - Raylib.MeasureText(text, 20) * 0.5);
            int y = (int)(centerY - FontSize * 0.5);
            Raylib.DrawText(text, left + x, top + y, FontSize, Color.Yellow);
        }
    }

    private void RenderTargetTiles(Game game, int left, int top)
    {
        if (!ShowTargetTiles) { return; }
        foreach (EnemyActor enemy in game.Enemies)
        {
            _blinkusRenderer.RenderTargetTile(enemy, left, top);
        }
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