public class Board
{
    public int Rows { get; private init; }
    public int Columns { get; private init; }
    private Dictionary<Position, Element> _elements;
    public IReadOnlyDictionary<Position, Element> Elements => _elements.AsReadOnly();

    public Board(IEnumerable<string> asciiLayout) 
    {
        string[] data = [.. asciiLayout];
        Rows = data.Length;
        Columns = data[0].Length;
        _elements = InitElements(asciiLayout);
    }

    public Board(string asciiLayout) : this(asciiLayout.ReplaceLineEndings().Split(Environment.NewLine)) { }
  
    public void RemoveElement(Position pos) => _elements.Remove(pos);
    public void RemoveElement(int row, int col) => RemoveElement(new Position(row, col));
    public Element GetElement(Position pos) => _elements[pos];
    public bool TryGetElement(Position pos, out Element element) => _elements.TryGetValue(pos, out element);

    public static Dictionary<Position, Element> InitElements(IEnumerable<string> asciiLayout)
    {
        string[] data = [.. asciiLayout];
        Dictionary<Position, Element> elements = new();
        int rows = data.Length;
        int columns = data[0].Length;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                char ch = data[row][col];
                if (ch == ' ') { continue; }
                elements[new Position(row, col)] = ch.ToElement();
            }
        }

        foreach ((Position position, Element corner) in elements.Where(e => e.Value is Element.Corner))
        {
            int row = position.Row;
            int col = position.Col;
            _ = elements.TryGetValue(new Position(row - 1, col),     out Element above);
            _ = elements.TryGetValue(new Position(row + 1, col),     out Element below);
            _ = elements.TryGetValue(new Position(row    , col - 1), out Element left);
            _ = elements.TryGetValue(new Position(row    , col + 1), out Element right);

            elements[position] = true switch
            {
                _ when left  is Element.Horizontal && below is Element.Vertical => Element.TopRight,
                _ when left  is Element.Horizontal && above is Element.Vertical => Element.BottomRight,
                _ when right is Element.Horizontal && below is Element.Vertical => Element.TopLeft,
                _ when right is Element.Horizontal && above is Element.Vertical => Element.BottomLeft,

                _ when left  is Element.Corner or Element.TopLeft     && below is Element.Vertical => Element.TopRight,
                _ when left  is Element.Corner or Element.BottomLeft  && above is Element.Vertical => Element.BottomRight,
                _ when right is Element.Corner or Element.TopRight    && below is Element.Vertical => Element.TopLeft,
                _ when right is Element.Corner or Element.BottomRight && above is Element.Vertical => Element.BottomLeft,

                _ when left  is Element.Horizontal && below is Element.Corner or Element.BottomRight => Element.TopRight,
                _ when left  is Element.Horizontal && above is Element.Corner or Element.TopRight    => Element.BottomRight,
                _ when right is Element.Horizontal && below is Element.Corner or Element.BottomLeft  => Element.TopLeft,
                _ when right is Element.Horizontal && above is Element.Corner or Element.TopLeft     => Element.BottomLeft,

                _ => throw new NotImplementedException(),
            };
        }

        return elements;
    }

    public bool IsDot(int row, int col) => TryGetElement(new Position(row, col), out Element element) && element.IsDot();
    public bool IsPowerPill(int row, int col) => TryGetElement(new Position(row, col), out Element element) && element.IsPowerPill();

    public bool IsWall(int row, int col) => TryGetElement(new Position(row, col), out Element element) && element.IsWall();
    public bool Contains(int row, int col) => row >= 0 && row < Rows && col >= 0 && col < Columns;

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
    
    public static (double, double) CalculateMove(this Board board, Direction moving, double x, double y, double distance)
    {
        (double nextX, double nextY) = moving switch
        {
            Direction.Right => (x + distance, y),
            Direction.Left => (x - distance, y),
            Direction.Up => (x, y - distance),
            Direction.Down => (x, y + distance),
            _ => throw new Exception($"Unknown direction {moving}."),
        };

        (int nextCol, int nextRow) = moving switch
        {
            Direction.Right => ((int)Math.Ceiling(nextX), (int)nextY),
            Direction.Left => ((int)nextX, (int)nextY),
            Direction.Up => ((int)nextX, (int)nextY),
            Direction.Down => ((int)nextX, (int)Math.Ceiling(nextY)),
            _ => throw new Exception($"Unknown direction {moving}."),
        };

        // If there is no wall, return move
        if (!board.IsWall(nextRow, nextCol)) { return (nextX, nextY); }

        // Otherwise, snap to the specific wall
        return moving switch
        {
            Direction.Right => (nextCol - 1, nextY),
            Direction.Left => (nextCol + 1, nextY),
            Direction.Up => (nextX, nextRow + 1),
            Direction.Down => (nextX, nextRow - 1),
            _ => throw new Exception($"Unknown direction {moving}."),
        };
    }

    /// <summary>
    /// Given a deltaTime representing the number of seconds passed, calculates
    /// the specified actor's next position.
    /// </summary>
    public static (double, double, Direction) CalculateActorMove(this Board board, double deltaTime, Actor actor)
    {
        double distance = actor.Speed * deltaTime;
        var (nextX, nextY) = CalculateMoveWithTurn(board, actor.CurrentDirection, actor.NextDirection, actor.X, actor.Y, distance);
        var nextDir = NextDirection(board, actor.CurrentDirection, actor.NextDirection, actor.X, actor.Y, distance);
        return (nextX, nextY, nextDir);
    }
    
    /// <summary>
    /// Calculates the next position of an actor on the board if they were to
    /// make a 90 degree turn from current to next.
    /// </summary>
    public static (double, double) CalculateMoveWithTurn(this Board board, Direction current, Direction queuedDirection,  double x, double y, double distance)
    {
        Direction nextDirection = NextDirection(board, current, queuedDirection, x, y, distance);
        // This method assumes the move is valid and the distance <= 1
        if (current == nextDirection) { return board.CalculateMove(current, x, y, distance); }
        if (current.IsOpposite(nextDirection)) { return board.CalculateMove(nextDirection, x, y, distance); }

        (double snapToX, double snapToY) = current switch
        {
            Direction.Down => (Math.Floor(x), Math.Ceiling(y)),
            Direction.Right => (Math.Ceiling(x), Math.Floor(y)),
            Direction.Up or Direction.Left => (Math.Floor(x), Math.Floor(y)),
            _ => throw new Exception($"Unknown direction: {current}"),
        };

        // (distance - Math.Abs(y - snapToY)) represents the remaining distance
        // after the move this should be added in the new direction
        return queuedDirection switch
        {
            Direction.Left => (snapToX - (distance - Math.Abs(y - snapToY)), snapToY),
            Direction.Right => (snapToX + (distance - Math.Abs(y - snapToY)), snapToY),
            Direction.Up => (snapToX, snapToY - (distance - Math.Abs(x - snapToX))),
            Direction.Down => (snapToX, snapToY + (distance - Math.Abs(x - snapToX))),
            _ => throw new Exception($"Unknown direction: {queuedDirection}"),
        };
    }

    /// <summary>
    /// Calculates the direction at the end of this movement.
    /// </summary>
    public static Direction NextDirection
        (this Board board, Direction currentDir, Direction nextDir, 
         double x, double y, double distance)
    {
        
        // You can always turn around / continue in the same direction
        if (currentDir == nextDir || currentDir.IsOpposite(nextDir)) { return nextDir; }
        // Calculate the position, if you move straight
        (double endX, double endY) = board.CalculateMove(currentDir, x, y, distance);
        bool isCrossingCenter = currentDir switch
        {
            Direction.Up => (int)y == (int)Math.Ceiling(endY),
            Direction.Down => (int)Math.Ceiling(y) == (int)endY,
            Direction.Right => (int)Math.Ceiling(x) == (int)endX,
            Direction.Left => (int)x == (int)Math.Ceiling(endX),
            _ => throw new Exception($"Unknown direction {currentDir}"),
        };

        // If this move would not cross the center of a tile
        // it is not possible to turn.
        if (!isCrossingCenter) { return currentDir; }

        // Check possible 90 degree turns
        (int row, int col) = (currentDir, nextDir) switch
        {
            (Direction.Up, Direction.Left) => ((int)Math.Ceiling(endY), (int)x - 1),
            (Direction.Up, Direction.Right) => ((int)Math.Ceiling(endY), (int)x + 1),
            (Direction.Down, Direction.Left) => ((int)endY, (int)x - 1),
            (Direction.Down, Direction.Right) => ((int)endY, (int)x + 1),
            (Direction.Right, Direction.Up) => ((int)y - 1, (int)endX),
            (Direction.Right, Direction.Down) => ((int)y + 1, (int)endX),
            (Direction.Left, Direction.Up) => ((int)y - 1, (int)Math.Ceiling(endX)),
            (Direction.Left, Direction.Down) => ((int)y + 1, (int)Math.Ceiling(endX)),
            _ => throw new Exception($"Unknown 90 degree turn: {currentDir} to {nextDir}"),
        };

        // If there is a wall or if we are trying to move out of the board, keep going 
        // in the current direction
        if (!board.Contains(row, col) || board.IsWall(row, col)) { return currentDir; }

        // Otherwise, change directions
        return nextDir;
    }
}