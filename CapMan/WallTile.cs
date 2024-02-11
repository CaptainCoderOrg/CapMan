namespace CapMan;

public enum WallTile
{
    Vertical = '│',
    Horizontal = '─',
    TopLeft = '╭',
    TopRight = '╮',
    BottomLeft = '╰',
    BottomRight = '╯',
}

public static class WallTileExtensions
{
    public static bool IsWallChar(char ch) => Enum.IsDefined((WallTile)ch);
}
