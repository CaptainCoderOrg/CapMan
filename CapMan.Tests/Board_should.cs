using Shouldly;

namespace CapMan.Tests;

public class Board_should
{
    [Theory]
    //                           x  y   d  x    y
    [InlineData(Direction.Right, 1, 1,  1, 2,   1)]
    [InlineData(Direction.Left,  3, 1,  1, 2,   1)]
    [InlineData(Direction.Down,  3, 1,  1, 3,   2)]
    [InlineData(Direction.Up,    3, 3,  1, 3,   2)]
    [InlineData(Direction.Right, 1, 1, .1, 1.1, 1)]
    [InlineData(Direction.Left,  3, 1, .1, 2.9, 1)]
    [InlineData(Direction.Down,  3, 1, .1, 3, 1.1)]
    [InlineData(Direction.Up,    3, 3, .1, 3, 2.9)]
    public void allow_move_when_space_is_empty(Direction direction, double startX, double startY, double distance, double endX, double endY)
    {
        Board board = new Board(
            [
                "╭───╮",
                "│...│",
                "│...│",
                "│...│",
                "╰───╯",
            ]
        );

        (double newX, double newY) = board.CalculateMove(direction, startX, startY, distance);

        newX.ShouldBe(endX, .01);
        newY.ShouldBe(endY, .01);
    }

    [Theory]
    //                             start     d    end
    [InlineData(Direction.Right, 2.5, 1.0, 1.0, 3.0, 1.0)]
    [InlineData(Direction.Left , 1.5, 1.0, 1.0, 1.0, 1.0)]
    [InlineData(Direction.Up   , 1.0, 1.5, 1.0, 1.0, 1.0)]    
    [InlineData(Direction.Down , 1.0, 2.5, 1.0, 1.0, 3.0)]
    [InlineData(Direction.Right, 2.9, 1.0, 0.2, 3.0, 1.0)]
    [InlineData(Direction.Left , 1.1, 1.0, 0.2, 1.0, 1.0)]
    [InlineData(Direction.Up   , 1.0, 1.1, 0.2, 1.0, 1.0)]
    [InlineData(Direction.Down , 1.0, 2.9, 0.2, 1.0, 3.0)]
    public void does_not_allow_move_when_space_is_wall(Direction direction, double startX, double startY, double distance, double endX, double endY)
    {

        Board board = new Board(
            [
                "╭───╮",
                "│...│",
                "│...│",
                "│...│",
                "╰───╯",
            ]
        );

        (double newX, double newY) = board.CalculateMove(direction, startX, startY, distance);

        newX.ShouldBe(endX, .01);
        newY.ShouldBe(endY, .01);

    }
}