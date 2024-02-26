namespace CapMan.Raylib;

using Raylib_cs;

public class EnemyRenderer(AnimatedSprite searchingSprite, AnimatedSprite chasingSprite, AnimatedSprite fleeingSprite)
{
    public Color BoxColor { get; set; } = Color.Orange;
    public AnimatedSprite FleeingSprite { get; } = fleeingSprite;
    public AnimatedSprite SearchingSprite { get; } = searchingSprite;
    public AnimatedSprite ChasingSprite { get; } = chasingSprite;

    public void Render(EnemyActor enemy, int boardLeft, int boardTop)
    {
        AnimatedSprite sprite = enemy.State switch
        {
            EnemyState.Searching => SearchingSprite,
            EnemyState.Chasing => ChasingSprite,
            _ => throw new NotImplementedException($"No sprite implemented for state {enemy.State}."),
        };
        sprite.CurrentTime += Raylib.GetFrameTime();

        sprite.FlipX = enemy.CurrentDirection switch
        {
            Direction.Left when sprite.FlipX is false => true,
            Direction.Right when sprite.FlipX is true => false,
            _ => sprite.FlipX,
        };

        int x = (int)(enemy.Position.X * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        int y = (int)(enemy.Position.Y * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        Color tint = enemy.IsAlive ? Color.White : Color.DarkGray;
        sprite.Draw(boardLeft + x, boardTop + y, tint);
    }

    public void RenderBoundingBox(Actor actor, int left, int top) =>
        actor.BoundingBox().ToBoard().Translate(left, top).Render(BoxColor);

    public void RenderTargetTile(EnemyActor enemy, int left, int top) =>
        enemy.LastTarget?.BoundingBox().ToBoard().Translate(left, top).Render(BoxColor);
}