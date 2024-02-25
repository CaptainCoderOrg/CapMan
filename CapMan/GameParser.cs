namespace CapMan;
public static class GameParser
{
    private static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s);

    public static Game Create(string lines) => Create(lines.ReplaceLineEndings().Split(Environment.NewLine));
    public static Game Create(IEnumerable<string> lines)
    {
        Dictionary<string, Actor> actors = [];

        foreach (string line in lines.TakeWhile(IsNotABlankLine))
        {
            string[] tokens = line.Split(',', StringSplitOptions.TrimEntries);
            if (tokens is [string name, string startX, string startY, string speed, string direction, string behaviour, .. string[] behaviourParams])
            {
                Position startPosition = new(double.Parse(startX), double.Parse(startY));
                double startSpeed = double.Parse(speed);
                Direction startDirection = Enum.Parse<Direction>(direction);
                Actor actor;
                if (name.Equals("CapMan", StringComparison.InvariantCultureIgnoreCase))
                {
                    actor = new PlayerActor()
                    {
                        Position = startPosition,
                        Speed = startSpeed,
                        CurrentDirection = startDirection,
                    };
                }
                else
                {
                    IEnemyBehaviour enemyBehaviour = behaviour.ToLowerInvariant() switch
                    {
                        "targetplayertile" => new TargetPlayerTile(),
                        "bob" => new BobAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5]))
                            ),
                        "clyde" => new ClydeAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5]))
                            ),
                        "whimsical" => new WhimsicalAIBehaviour(
                            new Tile(int.Parse(behaviourParams[0]), int.Parse(behaviourParams[1])),
                            new Tile(int.Parse(behaviourParams[2]), int.Parse(behaviourParams[3])),
                            new Tile(int.Parse(behaviourParams[4]), int.Parse(behaviourParams[5])),
                            (EnemyActor)actors[behaviourParams[6]]
                            ),
                        _ => throw new NotImplementedException(),
                    };
                    actor = new EnemyActor(startPosition, startSpeed, startDirection) { Behaviour = enemyBehaviour };
                }
                actors[name] = actor;
            }
        }

        IEnumerable<string> boardLayout = lines
            .SkipWhile(IsNotABlankLine)
            .Skip(1);

        return new Game(actors.Values, new Board(boardLayout));
    }
}
