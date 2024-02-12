namespace CapMan.Raylib;

using Raylib_cs;

public class EnemyRenderer(AnimatedSprite searchingSprite, AnimatedSprite chasingSprite, AnimatedSprite fleeingSprite)
{
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
        sprite.Draw(boardLeft + x, boardTop + y);
    }
}