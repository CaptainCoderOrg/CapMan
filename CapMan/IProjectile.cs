namespace CapMan;

public interface IProjectile : IActor
{
    public bool IsLethal { get; }
    public bool IsPickedUp { get; set; }
}