using Shouldly;

namespace CapMan.Tests;

public class Board_should
{
    [Theory]
    //                           x  y   d  x    y
    [InlineData(Direction.Right, 1, 1, 1, 2, 1)]
    [InlineData(Direction.Left, 3, 1, 1, 2, 1)]
    [InlineData(Direction.Down, 3, 1, 1, 3, 2)]
    [InlineData(Direction.Up, 3, 3, 1, 3, 2)]
    [InlineData(Direction.Right, 1, 1, .1, 1.1, 1)]
    [InlineData(Direction.Left, 3, 1, .1, 2.9, 1)]
    [InlineData(Direction.Down, 3, 1, .1, 3, 1.1)]
    [InlineData(Direction.Up, 3, 3, .1, 3, 2.9)]
    public void allow_move_when_space_is_empty(Direction direction, double startX, double startY, double distance, double endX, double endY)
    {
        Board board = new Board(
            """
            ╭───╮
            │...│
            │...│
            │...│
            ╰───╯
            """
        );

        (double newX, double newY) = board.CalculateMove(direction, startX, startY, distance);

        newX.ShouldBe(endX, .01);
        newY.ShouldBe(endY, .01);
    }

    [Theory]
    //                             start     d    end
    [InlineData(Direction.Right, 2.5, 1.0, 1.0, 3.0, 1.0)]
    [InlineData(Direction.Left, 1.5, 1.0, 1.0, 1.0, 1.0)]
    [InlineData(Direction.Up, 1.0, 1.5, 1.0, 1.0, 1.0)]
    [InlineData(Direction.Down, 1.0, 2.5, 1.0, 1.0, 3.0)]
    [InlineData(Direction.Right, 2.9, 1.0, 0.2, 3.0, 1.0)]
    [InlineData(Direction.Left, 1.1, 1.0, 0.2, 1.0, 1.0)]
    [InlineData(Direction.Up, 1.0, 1.1, 0.2, 1.0, 1.0)]
    [InlineData(Direction.Down, 1.0, 2.9, 0.2, 1.0, 3.0)]
    public void does_not_allow_move_when_space_is_wall(Direction direction, double startX, double startY, double distance, double endX, double endY)
    {

        Board board = new Board(
            """
            ╭───╮
            │...│
            │...│
            │...│
            ╰───╯
            """
        );

        (double newX, double newY) = board.CalculateMove(direction, startX, startY, distance);

        newX.ShouldBe(endX, .01);
        newY.ShouldBe(endY, .01);

    }

    [Theory]
    // Given a start position, distance to travel, and 90 degree turn
    // where should we end our move?
    //              start   d    end     90 degree turn direction
    [InlineData(2.0, 2.9, 1.0, 2.1, 2.0, Direction.Up, Direction.Right)]
    [InlineData(2.0, 2.2, 0.3, 2.1, 2.0, Direction.Up, Direction.Right)]
    [InlineData(2.0, 2.9, 1.0, 1.9, 2.0, Direction.Up, Direction.Left)]
    [InlineData(2.0, 2.2, 0.3, 1.9, 2.0, Direction.Up, Direction.Left)]
    [InlineData(2.0, 1.9, 0.3, 2.2, 2.0, Direction.Down, Direction.Right)]
    [InlineData(2.0, 1.5, 1.0, 2.5, 2.0, Direction.Down, Direction.Right)]
    [InlineData(2.0, 1.9, 0.3, 1.8, 2.0, Direction.Down, Direction.Left)]
    [InlineData(2.0, 1.5, 1.0, 1.5, 2.0, Direction.Down, Direction.Left)]
    [InlineData(1.5, 2.0, 1.0, 2.0, 2.5, Direction.Right, Direction.Down)]
    [InlineData(1.8, 2.0, 0.3, 2.0, 2.1, Direction.Right, Direction.Down)]
    [InlineData(1.5, 2.0, 1.0, 2.0, 1.5, Direction.Right, Direction.Up)]
    [InlineData(1.8, 2.0, 0.3, 2.0, 1.9, Direction.Right, Direction.Up)]
    [InlineData(2.5, 2.0, 1.0, 2.0, 2.5, Direction.Left, Direction.Down)]
    [InlineData(2.2, 2.0, 0.3, 2.0, 2.1, Direction.Left, Direction.Down)]
    [InlineData(2.5, 2.0, 1.0, 2.0, 1.5, Direction.Left, Direction.Up)]
    [InlineData(2.2, 2.0, 0.3, 2.0, 1.9, Direction.Left, Direction.Up)]
    public void allow_90degree_turn_when_intersection_is_open(
        double startX, double startY, double distance,
        double endX, double endY,
        Direction current, Direction next)
    {

        Board board = new Board(
            [
              // 01234  
                " │.│",  // 0
                "─╯.╰─", // 1
                ".....", // 2
                "─╮.╭─", // 3
                " │.│",  // 4
            ]
        );

        (double newX, double newY) = board.CalculateMoveWithTurn(current, next, startX, startY, distance);

        newX.ShouldBe(endX, 0.01);
        newY.ShouldBe(endY, 0.01);
    }

    [Theory]
    [InlineData(2.9, 2.0, 0.5, Direction.Up, Direction.Right)]
    [InlineData(2.9, 2.0, 0.5, Direction.Up, Direction.Left)]
    [InlineData(1.9, 2.0, 0.5, Direction.Down, Direction.Left)]
    [InlineData(1.9, 2.0, 0.5, Direction.Down, Direction.Right)]
    [InlineData(2.0, 1.9, 0.5, Direction.Right, Direction.Down)]
    [InlineData(2.0, 1.9, 0.5, Direction.Right, Direction.Up)]
    [InlineData(2.0, 2.1, 0.5, Direction.Left, Direction.Down)]
    [InlineData(2.0, 2.1, 0.5, Direction.Left, Direction.Up)]
    public void detect_direction_change_at_open_intersection(
         double x, double y, double distance, Direction currentDir, Direction nextDir)
    {
        Board board = new Board(
            [
              // 01234  
                " │.│",  // 0
                "─╯.╰─", // 1
                ".....", // 2
                "─╮.╭─", // 3
                " │.│",  // 4
            ]
        );

        Direction actual = board.NextDirection(currentDir, nextDir, x, y, distance);
        actual.ShouldBe(nextDir);
    }

    [Theory]
    [InlineData(2.0, 3.0, 0.5, Direction.Up, Direction.Right)]
    [InlineData(2.0, 3.0, 0.5, Direction.Up, Direction.Left)]
    [InlineData(2.0, 1.0, 0.5, Direction.Down, Direction.Left)]
    [InlineData(2.0, 1.0, 0.5, Direction.Down, Direction.Right)]
    [InlineData(1.0, 2.0, 0.5, Direction.Right, Direction.Down)]
    [InlineData(1.0, 2.0, 0.5, Direction.Right, Direction.Up)]
    [InlineData(3.0, 2.0, 0.5, Direction.Left, Direction.Down)]
    [InlineData(3.0, 2.0, 0.5, Direction.Left, Direction.Up)]
    public void not_detect_direction_change_against_wall(
         double x, double y, double distance, Direction currentDir, Direction nextDir)
    {
        Board board = new Board(
            [
                        // 01234  
                " │.│",  // 0
                "─╯.╰─", // 1
                ".....", // 2
                "─╮.╭─", // 3
                " │.│",  // 4
            ]
                );

        Direction actual = board.NextDirection(currentDir, nextDir, x, y, distance);
        actual.ShouldBe(currentDir);
    }
}