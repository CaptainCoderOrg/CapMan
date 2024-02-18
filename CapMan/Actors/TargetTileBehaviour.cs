namespace CapMan;

using FavoredMove = (Tile Tile, int Distance, Direction Direction);

public class TargetTileBehaviour(Tile target) : IEnemyBehaviour
{
    public Tile Target { get; } = target;
    public Direction GetNextDirection(Game game, double deltaTime, EnemyActor enemy)
    {
        var (board, direction) = (game.Board, enemy.CurrentDirection);
        enemy.LastTarget = Target;
        return DirectionWithShortestPath(board, enemy.Position.NextTile(direction), direction, Target);

    }

    public static Direction DirectionWithShortestPath(Board board, Tile start, Direction current, Tile targetTile)
    {
        Direction[] options = [.. Neighbors(start, current)];
        if (options.Length == 0) { return current; }
        if (options.Length == 1) { return options[0]; }
        FavoredMove favoredMove = (start.Step(options[0]), targetTile.ManhattanDistance(start.Step(options[0])), options[0]);
        HashSet<(Tile, Direction)> visited = new([.. options.Select(d => (start.Step(d), d))]);
        Queue<(Tile, Direction, Direction)> toVisit = new(options.Select(d => (start.Step(d), d, d)));
        while (toVisit.TryDequeue(out var result))
        {
            (Tile tile, Direction lastDir, Direction startDir) = result;
            if (tile == targetTile)
            {
                return startDir;
            }

            int distance = targetTile.ManhattanDistance(tile);
            if (distance < favoredMove.Distance)
            {
                favoredMove = (tile, distance, startDir);
            }

            foreach (Direction stepDir in Neighbors(tile, lastDir))
            {
                // Can't turn around
                Tile neighbor = board.WrapTile(tile.Step(stepDir));
                if (visited.Contains((neighbor, stepDir))) { continue; }
                visited.Add((neighbor, stepDir));
                toVisit.Enqueue((neighbor, stepDir, startDir));
            }
        }

        // If no valid path, select the closest position to our target that we
        // can reach
        return favoredMove.Direction;

        IEnumerable<Direction> Neighbors(Tile tile, Direction lastDir) =>
            Enum.GetValues<Direction>()
            // Cannot turn around
            .NotWhere(d => d.IsOpposite(lastDir))
            // Cannot move into walls
            .NotWhere(d => board.IsWall(board.WrapTile(tile.Step(d))));
    }
}