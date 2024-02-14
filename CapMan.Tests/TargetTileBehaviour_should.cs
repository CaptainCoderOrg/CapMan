namespace CapMan.Tests;

public class TargetTileBehaviour_should
{

    [Theory]
    [InlineData(Direction.Right, Direction.Right,
    """
    ╭────╯.╰
    │.....T.
    │.╭──╮.╭
    │.╰─╮│.╰
    │O..││..
    ╰─╮.││.╭
    ╭─╯.╰╯.│
    │..S...│
    """
    )]
    [InlineData(Direction.Left, Direction.Left,
    """
    |T.....|
    |.╭──╮.|
    |.│╭─╯.|
    |.││..O|
    |.││...|
    |.╰╯...|
    |...S..|
    """
    )]
    [InlineData(Direction.Up, Direction.Left,
    """
    |......|
    |.╭──╮.|
    |.│╭─╯.|
    .S││..T.
    |.││...|
    |.╰╯...|
    |......|
    """
    )]
    [InlineData(Direction.Up, Direction.Right,
    """
    |......|
    |.----.|
    |.|..|.|
    |.|.T|.|
    |.|--|.|
    |S.....|
    |.----.|
    |......|
    """
    )]
    public void select_best_direction(Direction facing, Direction expected, string boardData)
    {
        Board board = new(boardData.Replace('S', '.').Replace('T', '.'));
        Tile start = boardData.FindTile('S');
        Tile target = boardData.FindTile('T');
        for (int count = 0; count < 10; count++)
        {
            Direction actual = TargetTileBehaviour.DirectionWithShortestPath(board, start, facing, target);
            actual.ShouldBe(expected);
        }
    }

}

static class Extension
{
    public static Tile FindTile(this string boardData, char symbol)
    {
        string[] rows = boardData.ReplaceLineEndings().Split(Environment.NewLine);
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                if (rows[y][x] == symbol) { return new Tile(x, y); }
            }
        }
        throw new Exception($"Could not find symbol: '{symbol}'");
    }
}
