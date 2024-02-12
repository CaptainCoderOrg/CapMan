namespace CapMan.Raylib;

public static class BlinkusSprites
{
    private static string AssetPath(string filename) => Path.Combine("assets", "sprites", filename);
    private static SpriteSheet s_sheet => SpriteSheet.Get(AssetPath("blinkus.png"), 2, 5);
    private static AnimatedSprite? _blinkusSearching;

    public static AnimatedSprite Searching
    {
        get
        {
            if (_blinkusSearching is null)
            {
                _blinkusSearching = new AnimatedSprite(
                    s_sheet,
                    [(0, 0), (0, 0),
                     (0, 1), (0, 2), (0, 3),
                     (0, 4), (0, 4),
                     (0, 3), (0, 2), (0, 1)]
                )
                { FramesPerSecond = 1f };
            }
            return _blinkusSearching;
        }
    }

    private static AnimatedSprite? _blinkusChasing;
    public static AnimatedSprite Chasing
    {
        get
        {
            if (_blinkusChasing is null)
            {
                _blinkusChasing = new AnimatedSprite(
                    s_sheet,
                    [(1, 0), (1, 1), (1, 2)]
                )
                { FramesPerSecond = 10f };
            }
            return _blinkusChasing;
        }
    }

    private static AnimatedSprite? _blinkusFleeing;
    public static AnimatedSprite Fleeing
    {
        get
        {
            if (_blinkusFleeing is null)
            {
                _blinkusFleeing = new AnimatedSprite(
                    s_sheet,
                    [(1, 0)]
                )
                { FramesPerSecond = 10f };
            }
            return _blinkusFleeing;
        }
    }

}
