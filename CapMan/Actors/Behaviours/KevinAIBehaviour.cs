namespace CapMan;

public class KevinAIBehaviour(Tile patrol1, Tile patrol2, Tile houseExit)
    : TraditionalEnemyAI(patrol1, patrol2, houseExit, new TargetPlayerTile(), 0)
{
}