namespace CapMan;

public interface IActor
{
    public Position Position { get; }
    public void Update(IGame game, double deltaTime);
}