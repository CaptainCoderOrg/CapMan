namespace CapMan;

public class ClydeAIBehaviour(Tile patrol1, Tile patrol2, Tile houseExit)
    : TraditionalEnemyAI(patrol1, patrol2, houseExit, new ClydeTargeting(), 7)
{

}