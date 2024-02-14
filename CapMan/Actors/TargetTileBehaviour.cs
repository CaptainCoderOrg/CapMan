namespace CapMan;

using FavoredMove = (Tile Tile, int Distance, Direction Direction);

public class TargetTileBehaviour(Tile target) : IEnemyBehaviour
{
    public Tile Target { get; } = target;
    public Direction GetNextDirection(Game game, double deltaTime, Actor actor)
    {
        var (board, direction) = (game.Board, actor.CurrentDirection);
        return DirectionWithShortestPath(board, actor.Position.NextTile(direction), direction, Target);

    }

    public static Direction DirectionWithShortestPath(Board board, Tile start, Direction current, Tile targetTile)
    {
        Direction[] options = [.. Enum.GetValues<Direction>()
                                    .Where(d => !current.IsOpposite(d) &&
                                                !board.IsWall(start.Step(d)) &&
                                                board.Contains(start.Step(d))) ];
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

            foreach (Direction stepDir in Enum.GetValues<Direction>())
            {
                // Can't turn around
                if (stepDir.IsOpposite(lastDir)) { continue; }
                Tile neighbor = board.WrapTile(tile.Step(stepDir));
                if (board.IsWall(neighbor)) { continue; }
                if (!board.Contains(neighbor)) { continue; }
                if (visited.Contains((neighbor, stepDir))) { continue; }
                visited.Add((neighbor, stepDir));
                toVisit.Enqueue((neighbor, stepDir, startDir));
            }
        }
        // If no valid path, select a direction at random
        Console.WriteLine("No path found.");
        return favoredMove.Direction;
    }
}