public enum Element : ushort
{
    Dot = '.',
    PowerPill = 'O',
    Vertical = '│',
    Horizontal = '─',
    TopLeft = '╭',
    TopRight = '╮',
    BottomLeft = '╰',
    BottomRight = '╯',
    // Wall = Vertical or Horizontal or TopLeft or TopRight or BottomLeftor BottomRight,
}

public static class ElementExtensions
{
    public static Element ToElement(this char ch)
    {
        
        Element el = (Element)ch;
        if (Enum.IsDefined(el))
        {
            return el;
        }
        throw new ArgumentException($"Invlaid Element '{ch}'");
    }

    public static bool IsWall(this Element element) => element is Element.Vertical or Element.Horizontal or Element.TopLeft or Element.TopRight or Element.BottomLeft or Element.BottomRight;
    
}