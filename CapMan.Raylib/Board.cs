
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