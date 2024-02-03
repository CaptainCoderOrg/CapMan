public class Board
{
    public int Rows { get; private init; }
    public int Columns { get; private init; }
    private Dictionary<Position, Element> _elements;
    public IReadOnlyDictionary<Position, Element> Elements => _elements.AsReadOnly();

    public Board(IEnumerable<string> asciiLayout) 
    {
        string[] data = [.. asciiLayout];
        _elements = new();
        Rows = data.Length;
        Columns = data[0].Length;
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                char ch = data[row][col];
                if (ch == ' ') { continue; }
                _elements[new Position(row, col)] = ch.ToElement();
            }
        }
    }
    
    public void RemoveElement(Position pos) => _elements.Remove(pos);
    public Element GetElement(Position pos) => _elements[pos];
    public bool TryGetElement(Position pos, out Element element) => _elements.TryGetValue(pos, out element);
     
    public static readonly string[] StandardBoard = [
        "############################",
        "#............##............#",
        "#.####.#####.##.#####.####.#",
        "#O#  #.#   #.##.#   #.#  #O#",
        "#.####.#####.##.#####.####.#",
        "#..........................#",
        "#.####.##.########.##.####.#",
        "#.####.##.########.##.####.#",
        "#......##....##....##......#",
        "######.##### ## #####.######",
        "     #.##### ## #####.#     ",
        "     #.##          ##.#     ",
        "     #.## ######## ##.#     ",
        "######.## #      # ##.######",
        "      .   #      #   .      ",
        "######.## #      # ##.######",
        "     #.## ######## ##.#     ",
        "     #.##          ##.#     ",
        "     #.## ######## ##.#     ",
        "######.## ######## ##.######",
        "#............##............#",
        "#.####.#####.##.#####.####.#",
        "#.####.#####.##.#####.####.#",
        "#O..##................##..O#",
        "###.##.##.########.##.##.###",
        "###.##.##.########.##.##.###",
        "#......##....##....##......#",
        "#.##########.##.##########.#",
        "#.##########.##.##########.#",
        "#..........................#",
        "############################",
    ];
}