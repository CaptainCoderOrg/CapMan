public class Board
{
    public int Rows { get; private init; }
    public int Columns { get; private init; }
    private Dictionary<Position, Element> _elements;
    private Dictionary<Position, WallTile> _wallTiles;
    public IReadOnlyDictionary<Position, Element> Elements => _elements.AsReadOnly();

    public Board(IEnumerable<string> asciiLayout) 
    {
        string[] data = [.. asciiLayout];
        Rows = data.Length;
        Columns = data[0].Length;
        _elements = InitElements(asciiLayout);
        _wallTiles = InitWalls(asciiLayout);
    }
    
    public void RemoveElement(Position pos) => _elements.Remove(pos);
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
    public static Dictionary<Position, WallTile> InitWalls(IEnumerable<string> asciiLayout)
    {
        return InitWalls(InitElements(asciiLayout));
    }

    public static Dictionary<Position, WallTile> InitWalls(Dictionary<Position, Element> elements)
    {
        Dictionary<Position, WallTile> wallTiles = new ();
        HashSet<Position> walls = [.. elements.Where(kvp => kvp.Value == Element.Wall).Select(kvp => kvp.Key)];
        foreach(Position wallPosition in walls)
        {
            wallTiles[wallPosition] = FindNeighbors(wallPosition).ToWallTile();
        }
        return wallTiles;

        Neighbors FindNeighbors(Position wallPosition)
        {
            Neighbors neighbors = Neighbors.None;
            foreach (Neighbors neighbor in NeighborExtensions.OrthogonalNeighbors)
            {
                Position nPosition = wallPosition.Neighbor(neighbor);
                if (walls.Contains(nPosition))
                {
                    neighbors |= neighbor;
                }
            }
            return neighbors;
        }
    }

    

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