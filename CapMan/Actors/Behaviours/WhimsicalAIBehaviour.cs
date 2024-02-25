namespace CapMan;

public class WhimsicalAIBehaviour(EnemyActor target) : TraditionalEnemyAI(new Tile(16, 15), new Tile(16, 13), new Tile(13, 11), new WhimsicalTargeting(target), 21)
{

}