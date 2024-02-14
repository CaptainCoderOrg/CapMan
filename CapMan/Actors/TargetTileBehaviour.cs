namespace CapMan;

public class TargetTileBehaviour(Tile target) : IEnemyBehaviour
{
    public Tile Target { get; } = target;
    public Direction GetNextDirection(Game game, double deltaTime, Actor actor)
    {
        var (board, direction) = (game.Board, actor.CurrentDirection);
        IEnumerable<Direction> options = board.ValidTurns(deltaTime, actor).Where(d => !d.IsOpposite(direction));
        // if (!board.IsWall(actor.Position.)) { options.Append(direction); }

        // Console.WriteLine($"@{actor.Tile}: {options.Length}");
        return DirectionWithShortestPath(game.Board, actor.Tile, [.. options], Target);

    }

    public static Direction DirectionWithShortestPath(Board board, Tile start, Direction[] options, Tile targetTile)
    {
        if (options.Length == 1) { return options[0]; }
        Console.WriteLine($"Searching: {string.Join(", ", options)}");
        HashSet<(Tile, Direction)> visited = new([.. options.Select(d => (start.Step(d), d))]);
        Queue<(Tile, Direction, Direction)> toVisit = new(options.Select(d => (start.Step(d), d, d)));
        while (toVisit.TryDequeue(out var result))
        {
            (Tile tile, Direction lastDir, Direction startDir) = result;
            if (tile == targetTile)
            {
                Console.WriteLine($"Chose: {startDir}");
                return startDir;
            }

            foreach (Direction stepDir in Enum.GetValues<Direction>())
            {
                // Can't turn around
                if (stepDir.IsOpposite(lastDir)) { continue; }
                Tile neighbor = tile.Step(stepDir);
                if (visited.Contains((neighbor, stepDir))) { continue; }
                visited.Add((neighbor, stepDir));
                toVisit.Enqueue((neighbor, stepDir, startDir));
            }
        }
        // If no valid path, select a direction at random
        Console.WriteLine("No path found.");
        return options[Random.Shared.Next(options.Length)];
    }
}
