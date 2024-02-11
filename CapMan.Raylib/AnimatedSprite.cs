namespace CapMan.Raylib;

public class AnimatedSprite
{
    public double FramesPerSecond { get; init; } = 10f;
    private (int Row, int Column)[] _frames;
    private SpriteSheet _sheet;
    public float Rotation { get; set; } = 0;
    public bool FlipX { get; set; } = false;
    public double CurrentTime { get; set; } = 0;

    public AnimatedSprite(SpriteSheet spriteSheet, IEnumerable<(int row, int col)> frames)
    {
        _sheet = spriteSheet;
        _frames = [.. frames];
    }

    public void Draw(int x, int y)
    {
        int currentFrame = ((int)(CurrentTime * FramesPerSecond)) % _frames.Length;
        (int row, int column) = _frames[currentFrame];
        _sheet.DrawSprite(row, column, x, y, Rotation, FlipX);
    }
}