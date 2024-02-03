public enum Element : ushort
{
    Dot = '.',
    PowerPill = 'O',
    Wall = '#',
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
}