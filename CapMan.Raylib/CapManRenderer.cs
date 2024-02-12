namespace CapMan.Raylib;
using Raylib_cs;

public class CapManRenderer
{
    private double lastX;
    private double lastY;
    private static SpriteSheet? _spriteSheet;
    public static SpriteSheet s_SpriteSheet
    {
        get
        {
            if (_spriteSheet == null)
            {
                _spriteSheet = SpriteSheet.Get("assets/sprites/capman.png", 1, 3);
            }
            return _spriteSheet;
        }
    }

    private AnimatedSprite? _sprite;
    public AnimatedSprite Sprite
    {
        get
        {
            if (_sprite == null)
            {
                _sprite = new AnimatedSprite(s_SpriteSheet, [(0, 0), (0, 1), (0, 2), (0, 1)]);
            }
            return _sprite;
        }
    }

    public void Render(PlayerActor capman, int left, int top)
    {
        if ((lastX, lastY) != (capman.Position.X, capman.Position.Y))
        {
            Sprite.CurrentTime += Raylib.GetFrameTime();
            (lastX, lastY) = (capman.Position.X, capman.Position.Y);
        }
        Sprite.Rotation = capman.CurrentDirection switch
        {
            Direction.Left => 0,
            Direction.Up => 90,
            Direction.Right => 0,
            Direction.Down => 270,
            _ => throw new Exception($"Unexpected direction {capman.CurrentDirection}"),
        };
        Sprite.FlipX = capman.CurrentDirection == Direction.Right ? true : false;
        int x = (int)(capman.Position.X * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        int y = (int)(capman.Position.Y * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        Sprite.Draw(left + x, top + y);
    }
}