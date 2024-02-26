namespace CapMan;

public class BobAIBehaviour(Tile patrol1, Tile patrol2, Tile houseExit) : TraditionalEnemyAI(patrol1, patrol2, houseExit, new TargetAheadOfPlayer(4), 14)
{
}