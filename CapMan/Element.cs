namespace CapMan;

public enum Element : ushort
{
    Dot = '.',
    PowerPill = 'O',
    Vertical = '│',
    Horizontal = '─',
    Corner = '+',
    TopLeft = '╭',
    TopRight = '╮',
    BottomLeft = '╰',
    BottomRight = '╯',
    Door = '=',
}

public static class ElementExtensions
{
    public static Element ToElement(this char ch)
    {
        return ch switch
        {
            _ when (Element)ch is Element el && Enum.IsDefined(el) => el,
            '|' => Element.Vertical,
            '-' => Element.Horizontal,
            _ => throw new ArgumentException($"Invalid Element '{ch}'"),
        };
    }

    public static bool IsWall(this Element element) => element is Element.Vertical or Element.Horizontal or Element.TopLeft or Element.TopRight or Element.BottomLeft or Element.BottomRight or Element.Corner or Element.Door;
    public static bool IsDot(this Element element) => element is Element.Dot;
    public static bool IsPowerPill(this Element element) => element is Element.PowerPill;
}