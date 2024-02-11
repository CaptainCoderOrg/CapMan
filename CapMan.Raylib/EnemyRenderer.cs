namespace CapMan.Raylib;

using Raylib_cs;

public class EnemyRenderer
{
    private static SpriteSheet? _spriteSheet;
    public static SpriteSheet s_SpriteSheet
    {
        get
        {
            if (_spriteSheet == null)
            {
                _spriteSheet = SpriteSheet.Load("assets/sprites/blinkus.png", 2, 5);
            }
            return _spriteSheet;
        }
    }

    private AnimatedSprite? _searching;
    public AnimatedSprite SearchingSprite
    {
        get
        {
            if (_searching == null)
            {
                _searching = new AnimatedSprite(
                    s_SpriteSheet, 
                    [(0, 0), (0, 0),
                     (0, 1), (0, 2), (0, 3),
                     (0, 4), (0, 4),
                     (0, 3), (0, 2), (0, 1)]
                ) { FramesPerSecond = 3f };
            }
            return _searching;
        }
    }

     private AnimatedSprite? _attacking;
    public AnimatedSprite AttackingSprite
    {
        get
        {
            if (_attacking == null)
            {
                _attacking = new AnimatedSprite(
                    s_SpriteSheet, 
                    [(1, 0), (1, 1), (1, 2)]
                ){ FramesPerSecond = 10f };
            }
            return _attacking;
        }
    }

    public void Render(Game game)
    {
        SearchingSprite.CurrentTime += Raylib.GetFrameTime();
        AttackingSprite.CurrentTime += Raylib.GetFrameTime();
        SearchingSprite.Draw(50, 50);
        AttackingSprite.Draw(80, 50);
        // if ((lastX, lastY) != (game.Player.X, game.Player.Y))
        // {
        //     Sprite.CurrentTime += Raylib.GetFrameTime();
        //     (lastX, lastY) = (game.Player.X, game.Player.Y);
        // }
        // Sprite.Rotation = game.Player.CurrentDirection switch
        // {
        //     Direction.Left => 0,
        //     Direction.Up => 90,
        //     Direction.Right => 0,
        //     Direction.Down => 270,
        //     _ => throw new Exception($"Unexpected direction {game.Player.CurrentDirection}"),
        // };
        // Sprite.FlipX = game.Player.CurrentDirection == Direction.Right ? true : false;
        // int x = (int)(game.Player.X * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        // int y = (int)(game.Player.Y * BoardRenderer.CellSize) + BoardRenderer.CellSize / 2;
        // Sprite.Draw(x, y);
    }



}