namespace CapMan.Raylib;

using static Element;

public static class ElementExtensions
{
    public static bool IsRenderable(this Element element)
    {
        return element is Dot or PowerPill or Vertical or Horizontal or TopLeft or TopRight or BottomLeft or BottomRight or Door;
    }
}