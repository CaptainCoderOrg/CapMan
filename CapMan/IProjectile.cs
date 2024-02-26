namespace CapMan;

public interface IProjectile : IActor
{
    public bool IsLethal { get; }
}