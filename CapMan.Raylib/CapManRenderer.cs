namespace CapMan.Raylib;

using Raylib_cs;

public class CapManRenderer
{
    private static SpriteSheet? _spriteSheet;
    public static SpriteSheet s_SpriteSheet
    {
        get
        {
            if (_spriteSheet == null)
            {
                _spriteSheet = SpriteSheet.Load("assets/sprites/capman.png", 1, 3);
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

    private double lastX;
    private double lastY;

    public void Render(Game game)
    {
        if ((lastX, lastY) != (game.Player.X, game.Player.Y))
        {
            Sprite.CurrentTime += Raylib.GetFrameTime();
            (lastX, lastY) = (game.Player.X, game.Player.Y);
        }
        Sprite.Rotation = game.Player.CurrentDirection switch
        {
            Direction.Left => 0,
            Direction.Up => 90,
            Direction.Right => 0,
            Direction.Down => 270,
            _ => throw new Exception($"Unexpected direction {game.Player.CurrentDirection}"),
        };
        Sprite.FlipX = game.Player.CurrentDirection == Direction.Right ? true : false;
        int x = (int)(game.Player.X * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        int y = (int)(game.Player.Y * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        Sprite.Draw(x, y);
    }



}