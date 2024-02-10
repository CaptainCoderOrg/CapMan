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
        return elements;
    }

    public bool IsDot(int row, int col) => TryGetElement(new Position(row, col), out Element element) && element is Element.Dot;
    public bool IsPowerPill(int row, int col) => TryGetElement(new Position(row, col), out Element element) && element is Element.PowerPill;

    public bool IsWall(int row, int col) => TryGetElement(new Position(row, col), out Element element) && element.IsWall();
    public bool Contains(int row, int col) => row >= 0 && row < Rows && col >= 0 && col < Columns;

    public static readonly string[] StandardBoard = [
        "╭────────────╮╭────────────╮",
        "│............││............│",
        "│.╭──╮.╭───╮.││.╭───╮.╭──╮.│",
        "│O│  │.│   │.││.│   │.│  │O│",
        "│.╰──╯.╰───╯.╰╯.╰───╯.╰──╯.│",
        "│..........................│",
        "│.╭──╮.╭╮.╭──────╮.╭╮.╭──╮.│",
        "│.╰──╯.││.╰──╮╭──╯.││.╰──╯.│",
        "│......││....││....││......│",
        "╰────╮.│╰──╮ ││ ╭──╯│.╭────╯",
        "     │.│╭──╯ ╰╯ ╰──╮│.│     ",
        "     │.││          ││.│     ",
        "     │.││ ╭──────╮ ││.│     ",
        "─────╯.╰╯ │      │ ╰╯.╰─────",
        "      .   │      │   .      ",
        "─────╮.╭╮ │      │ ╭╮.╭─────",
        "     │.││ ╰──────╯ ││.│     ",
        "     │.││          ││.│     ",
        "     │.││ ╭──────╮ ││.│     ",
        "╭────╯.╰╯ ╰──╮╭──╯ ╰╯.╰────╮",
        "│............││............│",
        "│.╭──╮.╭───╮.││.╭───╮.╭──╮.│",
        "│.╰─╮│.╰───╯.╰╯.╰───╯.│╭─╯.│",
        "│O..││................││..O│",
        "╰─╮.││.╭╮.╭──────╮.╭╮.││.╭─╯",
        "╭─╯.╰╯.││.╰──╮╭──╯.││.╰╯.╰─╮",
        "│......││....││....││......│",
        "│.╭────╯╰──╮.││.╭──╯╰────╮.│",
        "│.╰────────╯.╰╯.╰────────╯.│",
        "│..........................│",
        "╰──────────────────────────╯",
    ];
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
}