namespace CapMan;

public class PatrolBehaviour(Tile first, Tile second) : IEnemyBehaviour
{
    public Tile First { get; } = first;
    public Tile Second { get; } = second;
    private Tile _current = first;
    private Tile _other = second;
    public Direction GetNextDirection(Game game, double deltaTime, EnemyActor enemy)
    {
        if (enemy.Tile == _current)
        {
            (_current, _other) = (_other, _current);
        }
        return DirectionWithShortestPath(game.Board, enemy.Tile, enemy.CurrentDirection, _current);
    }

    public static Direction DirectionWithShortestPath(Board board, Tile start, Direction current, Tile targetTile)
    {
        IEnumerable<Direction> Neighbors(Tile tile, Direction lastDir) =>
        Enum.GetValues<Direction>()
        // Cannot move into walls
        .NotWhere(d => board.IsWall(board.WrapTile(tile.Step(d))));
        return TargetTileBehaviour.DirectionWithShortestPath(board, start, current, targetTile, Neighbors);
    }
}