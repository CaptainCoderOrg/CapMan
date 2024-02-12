namespace CapMan.Raylib;
using Raylib_cs;
public class SpriteSheet
{
    private Texture2D _spriteSheet;
    public int Rows { get; }
    public int Columns { get; }
    public int SpriteWidth { get; }
    public int SpriteHeight { get; }

    public SpriteSheet(Texture2D spriteSheet, int rows, int columns)
    {
        // TODO: Only allow sprites with even number of textures
        (_spriteSheet, Rows, Columns) = (spriteSheet, rows, columns);
        SpriteWidth = spriteSheet.Width / columns;
        SpriteHeight = spriteSheet.Height / rows;
    }
        
    /// <summary>
    /// Given a path to an image file, returns a sprite sheet that is
    /// evenly sliced into the specified number of rows and columns.
    /// </summary>
    public static SpriteSheet Load(string path, int rows, int columns)
    {
        Texture2D image = Raylib.LoadTexture(path);
        return new SpriteSheet(image, rows, columns);
    }

    public void DrawSprite(int row, int col, int x, int y, float rotation, bool flipX)
    {
        float width = flipX ? -SpriteWidth : SpriteWidth;
        Rectangle crop = new (col * SpriteWidth, row * SpriteHeight, width, SpriteHeight);
        Rectangle dest = new (x, y, SpriteWidth, SpriteHeight);
        System.Numerics.Vector2 center = new (SpriteWidth/2, SpriteHeight/2);
        Raylib.DrawTexturePro(_spriteSheet, crop, dest, center, rotation, Color.White);
    }

    public void DrawSprite(int row, int col, int x, int y) => DrawSprite(row, col, x, y, 0, false);
}