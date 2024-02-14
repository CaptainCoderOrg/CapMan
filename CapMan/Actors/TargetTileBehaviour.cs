namespace CapMan;

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
        Direction[] options = [.. Enum.GetValues<Direction>().Where(d => !current.IsOpposite(d)).Where(d => !board.IsWall(start.Step(d)) && board.Contains(start.Step(d)))];
        if (options.Length == 0) { return current; }
        if (options.Length == 1) { return options[0]; }
        HashSet<(Tile, Direction)> visited = new([.. options.Select(d => (start.Step(d), d))]);
        Queue<(Tile, Direction, Direction)> toVisit = new(options.Select(d => (start.Step(d), d, d)));
        while (toVisit.TryDequeue(out var result))
        {
            (Tile tile, Direction lastDir, Direction startDir) = result;
            if (tile == targetTile)
            {
                return startDir;
            }

            foreach (Direction stepDir in Enum.GetValues<Direction>())
            {
                // Can't turn around
                if (stepDir.IsOpposite(lastDir)) { continue; }
                Tile neighbor = tile.Step(stepDir);
                if (board.IsWall(neighbor)) { continue; }
                if (!board.Contains(neighbor)) { continue; }
                if (visited.Contains((neighbor, stepDir))) { continue; }
                visited.Add((neighbor, stepDir));
                toVisit.Enqueue((neighbor, stepDir, startDir));
            }
        }
        // If no valid path, select a direction at random
        Console.WriteLine("No path found.");
        // TODO: Select nearest location?
        return options[Random.Shared.Next(options.Length)];
    }
}