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
            if (tokens is [string name, string startX, string startY, string speed, string direction, string behaviour])
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
                    string targetName = "";
                    if (behaviour.Contains('('))
                    {
                        string[] behaviourTokens = behaviour.Split(new char[] { '(', ')' }, StringSplitOptions.TrimEntries);
                        behaviour = behaviourTokens[0];
                        targetName = behaviourTokens[1];
                    }
                    IEnemyBehaviour enemyBehaviour = behaviour.ToLowerInvariant() switch
                    {
                        "bob" => new BobAIBehaviour(),
                        "clyde" => new ClydeAIBehaviour(),
                        "targetplayertile" => new TargetPlayerTile(),
                        "whimsical" => new WhimsicalAIBehaviour((EnemyActor)actors[targetName]),
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
