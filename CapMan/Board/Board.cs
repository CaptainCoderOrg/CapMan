using System.Drawing;

namespace CapMan;

public class Board
{
    public int Height { get; private init; }
    public int Width { get; private init; }
    private readonly Dictionary<Tile, Element> _elements;
    public IReadOnlyDictionary<Tile, Element> Elements => _elements.AsReadOnly();

    public Board(IEnumerable<string> asciiLayout)
    {
        string[] data = [.. asciiLayout];
        Height = data.Length;
        Width = data[0].Length;
        _elements = InitElements(asciiLayout);
    }

    public Board(string asciiLayout) : this(asciiLayout.ReplaceLineEndings().Split(Environment.NewLine)) { }

    public void RemoveElement(Tile pos) => _elements.Remove(pos);
    public bool TryGetElement(Tile pos, out Element element) => _elements.TryGetValue(pos, out element);

    public static Dictionary<Tile, Element> InitElements(IEnumerable<string> asciiLayout)
    {
        string[] data = [.. asciiLayout];
        Dictionary<Tile, Element> elements = new();
        int height = data.Length;
        int width = data[0].Length;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char ch = data[y][x];
                if (ch == ' ') { continue; }
                elements[new Tile(x, y)] = ch.ToElement();
            }
        }

        foreach ((Tile position, Element corner) in elements.Where(e => e.Value is Element.Corner))
        {
            var (x, y) = position;
            _ = elements.TryGetValue(new Tile(x, y - 1), out Element above);
            _ = elements.TryGetValue(new Tile(x, y + 1), out Element below);
            _ = elements.TryGetValue(new Tile(x - 1, y), out Element left);
            _ = elements.TryGetValue(new Tile(x + 1, y), out Element right);

            elements[position] = true switch
            {
                _ when left is Element.Horizontal && below is Element.Vertical => Element.TopRight,
                _ when left is Element.Horizontal && above is Element.Vertical => Element.BottomRight,
                _ when right is Element.Horizontal && below is Element.Vertical => Element.TopLeft,
                _ when right is Element.Horizontal && above is Element.Vertical => Element.BottomLeft,

                _ when left is Element.Corner or Element.TopLeft && below is Element.Vertical => Element.TopRight,
                _ when left is Element.Corner or Element.BottomLeft && above is Element.Vertical => Element.BottomRight,
                _ when right is Element.Corner or Element.TopRight && below is Element.Vertical => Element.TopLeft,
                _ when right is Element.Corner or Element.BottomRight && above is Element.Vertical => Element.BottomLeft,

                _ when left is Element.Horizontal && below is Element.Corner or Element.BottomRight => Element.TopRight,
                _ when left is Element.Horizontal && above is Element.Corner or Element.TopRight => Element.BottomRight,
                _ when right is Element.Horizontal && below is Element.Corner or Element.BottomLeft => Element.TopLeft,
                _ when right is Element.Horizontal && above is Element.Corner or Element.TopLeft => Element.BottomLeft,

                _ => throw new NotImplementedException(),
            };
        }

        return elements;
    }

    public bool IsDot(Tile position) => TryGetElement(position, out Element element) && element.IsDot();
    public bool IsPowerPill(Tile position) => TryGetElement(position, out Element element) && element.IsPowerPill();
    public bool IsWall(Tile position) => TryGetElement(position, out Element element) && element.IsWall();
    public bool Contains(Tile position) => position.Y >= 0 && position.Y < Height && position.X >= 0 && position.X < Width;

    public static readonly string StandardBoard = """
        ╭────────────╮╭────────────╮
        │............││............│
        │.╭──╮.╭───╮.││.╭───╮.╭──╮.│
        │O│  │.│   │.││.│   │.│  │O│
        │.╰──╯.╰───╯.╰╯.╰───╯.╰──╯.│
        │..........................│
        │.╭──╮.╭╮.╭──────╮.╭╮.╭──╮.│
        │.╰──╯.││.╰──╮╭──╯.││.╰──╯.│
        │......││....││....││......│
        ╰────╮.│╰──╮ ││ ╭──╯│.╭────╯
             │.│╭──╯ ╰╯ ╰──╮│.│     
             │.││          ││.│     
             │.││ ╭──────╮ ││.│     
        ─────╯.╰╯ │      │ ╰╯.╰─────
              .   │      │   .      
        ─────╮.╭╮ │      │ ╭╮.╭─────
             │.││ ╰──────╯ ││.│     
             │.││          ││.│     
             │.││ ╭──────╮ ││.│     
        ╭────╯.╰╯ ╰──╮╭──╯ ╰╯.╰────╮
        │............││............│
        │.╭──╮.╭───╮.││.╭───╮.╭──╮.│
        │.╰─╮│.╰───╯.╰╯.╰───╯.│╭─╯.│
        │O..││................││..O│
        ╰─╮.││.╭╮.╭──────╮.╭╮.││.╭─╯
        ╭─╯.╰╯.││.╰──╮╭──╯.││.╰╯.╰─╮
        │......││....││....││......│
        │.╭────╯╰──╮.││.╭──╯╰────╮.│
        │.╰────────╯.╰╯.╰────────╯.│
        │..........................│
        ╰──────────────────────────╯
        """;
}

public static class BoardExtensions
{
    public const double WrapDistance = 1;

    public static Position NextPosition(this Board board, Direction moving, Position position, double distance)
    {
        Position next = position.Move(moving, distance);
        Tile step = next.NextTile(moving);

        // If there is no wall, return move
        if (!board.IsWall(step)) { return next; }

        // Otherwise, snap to the specific wall
        return moving switch
        {
            Direction.Right => next with { X = step.X - 1 },
            Direction.Left => next with { X = step.X + 1 },
            Direction.Up => next with { Y = step.Y + 1 },
            Direction.Down => next with { Y = step.Y - 1 },
            _ => throw new Exception($"Unknown direction {moving}."),
        };
    }

    /// <summary>
    /// Given a deltaTime representing the number of seconds passed, calculates
    /// the specified actor's next position.
    /// </summary>
    public static (Position, Direction) CalculateActorMove(this Board board, double deltaTime, Actor actor)
    {
        double distance = actor.Speed * deltaTime;
        Position nextPosition = NextPositionWithTurn(board, actor.CurrentDirection, actor.NextDirection, actor.Position, distance);
        Direction nextDir = NextDirection(board, actor.CurrentDirection, actor.NextDirection, actor.Position, distance);
        return (nextPosition, nextDir);
    }

    /// <summary>
    /// Calculates the next position of an actor on the board if they were to
    /// make a 90 degree turn from current to next.
    /// </summary>
    public static Position NextPositionWithTurn(this Board board, Direction current, Direction queuedDirection, Position position, double distance)
    {
        Direction nextDirection = NextDirection(board, current, queuedDirection, position, distance);
        // This method assumes the move is valid and the distance <= 1
        if (current == nextDirection) { return board.NextPosition(current, position, distance); }
        if (current.IsOpposite(nextDirection)) { return board.NextPosition(nextDirection, position, distance); }

        Position nextTile = position.NextTile(current).ToPosition();

        return nextDirection switch
        {
            Direction.Left => nextTile with { X = nextTile.X - RemainingXMovement() },
            Direction.Right => nextTile with { X = nextTile.X + RemainingXMovement() },
            Direction.Up => nextTile with { Y = nextTile.Y - RemainingYMovement() },
            Direction.Down => nextTile with { Y = nextTile.Y + RemainingYMovement() },
            _ => throw new Exception($"Unknown direction: {queuedDirection}"),
        };

        // The distance remaining after centering on the next position.
        double RemainingXMovement() => distance - Math.Abs(position.Y - nextTile.Y);
        double RemainingYMovement() => distance - Math.Abs(position.X - nextTile.X);
    }

    public static Direction[] ValidNextDirection(this Board board, double deltaTime, Actor actor) =>
        ValidNextDirection(board, actor.CurrentDirection, actor.Position, actor.Speed * deltaTime);

    public static Direction[] ValidNextDirection(this Board board, Direction currentDir, Position position, double distance)
    {
        Position end = board.NextPosition(currentDir, position, distance);
        bool isCrossing = currentDir switch
        {
            Direction.Up => (int)position.Y == (int)Math.Ceiling(end.Y),
            Direction.Down => (int)Math.Ceiling(position.Y) == (int)end.Y,
            Direction.Right => (int)Math.Ceiling(position.X) == (int)end.X,
            Direction.Left => (int)position.X == (int)Math.Ceiling(end.X),
            _ => throw new Exception($"Unknown direction {currentDir}"),
        };
        // If we are not crossing an intersection, we can only turn around or continue
        if (!isCrossing) { return [currentDir, currentDir.Opposite()]; }
        return [.. Enum.GetValues<Direction>().Where(IsValidDirection)];

        bool IsValidDirection(Direction dir)
        {
            var step = end.CurrentTile(currentDir).Step(dir);
            return board.Contains(step) && !board.IsWall(step);
        }
    }

    /// <summary>
    /// Calculates the direction at the end of this movement.
    /// </summary>
    public static Direction NextDirection(this Board board, Direction currentDir, Direction nextDir, Position position, double distance)
    {
        // You can always turn around / continue in the same direction
        if (currentDir == nextDir || currentDir.IsOpposite(nextDir)) { return nextDir; }
        Direction[] validTurns = ValidNextDirection(board, currentDir, position, distance);
        if (validTurns.Contains(nextDir)) { return nextDir; }
        return currentDir;
    }

    /// <summary>
    /// Given an actor, returns their position on this board applying a wrap if necessary.
    /// </summary>
    public static Position WrapPosition(this Board board, Actor actor)
    {
        return actor.Position switch
        {
            // Wrap on Left
            var (x, _) when x < -WrapDistance => actor.Position with { X = x + board.Width + 2 * WrapDistance },
            // Wrap on Right
            var (x, _) when x > (board.Width + WrapDistance) => actor.Position with { X = x - (board.Width + 2 * WrapDistance) },
            // Wrap on Top
            var (_, y) when y < -WrapDistance => actor.Position with { Y = y + board.Height + 2 * WrapDistance },
            // Wrap on Bottom
            var (_, y) when y > (board.Height + WrapDistance) => actor.Position with { Y = y - (board.Height + 2 * WrapDistance) },
            // No wrap
            _ => actor.Position
        };
    }
}