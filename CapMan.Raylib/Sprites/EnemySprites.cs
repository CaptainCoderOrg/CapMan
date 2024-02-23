namespace CapMan.Raylib;

public static class BlinkusSprites
{
    private static string AssetPath(string filename) => Path.Combine("assets", "sprites", filename);
    private static SpriteSheet Sheet => SpriteSheet.Get(AssetPath("blinkus.png"), 2, 5);
    private static AnimatedSprite? s_blinkusSearching;

    public static AnimatedSprite Searching
    {
        get
        {
            if (s_blinkusSearching is null)
            {
                #pragma warning disable format 
                // Disabled for readability of animation
                s_blinkusSearching = new AnimatedSprite(
                    Sheet,
                    [(0, 0), (0, 0),
                     (0, 1), (0, 2), (0, 3),
                     (0, 4), (0, 4),
                     (0, 3), (0, 2), (0, 1)]
                )
                { FramesPerSecond = 1f };
                #pragma warning restore format
            }
            return s_blinkusSearching;
        }
    }

    private static AnimatedSprite? s_blinkusChasing;
    public static AnimatedSprite Chasing
    {
        get
        {
            if (s_blinkusChasing is null)
            {
                s_blinkusChasing = new AnimatedSprite(
                    Sheet,
                    [(1, 0), (1, 1), (1, 2)]
                )
                { FramesPerSecond = 10f };
            }
            return s_blinkusChasing;
        }
    }

    private static AnimatedSprite? s_blinkusFleeing;
    public static AnimatedSprite Fleeing
    {
        get
        {
            if (s_blinkusFleeing is null)
            {
                s_blinkusFleeing = new AnimatedSprite(
                    Sheet,
                    [(1, 0)]
                )
                { FramesPerSecond = 10f };
            }
            return s_blinkusFleeing;
        }
    }

}
