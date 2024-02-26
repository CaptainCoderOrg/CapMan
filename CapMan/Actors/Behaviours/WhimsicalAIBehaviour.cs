namespace CapMan;

public class WhimsicalAIBehaviour(Tile patrol1, Tile patrol2, Tile houseExit, EnemyActor target) : 
    TraditionalEnemyAI(patrol1, patrol2, houseExit, new WhimsicalTargeting(target), 21)
{

}