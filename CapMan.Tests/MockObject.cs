namespace CapMan.Tests;

public static class MockObject
{
    /// <summary>
    /// A simple Game to be used when the state of the board / actors is not important.
    /// </summary>
    public static Game StateUnimportant
    {
        get
        {
            string[] gameConfig =
             [
                "CapMan, (1, 1), 1, Down , manual",
                "",
                "+---+",
                "|...|",
                "|...|",
                "|...|",
                "+---+",
            ];
            return new Game(gameConfig);
        }
    }
}