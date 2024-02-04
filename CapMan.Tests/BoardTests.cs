using Shouldly;

namespace CapMan.Tests;

public class BoardTests
{
    [Fact]
    public void init_walls_should_build_rectangle()
    {
        Dictionary<Position, WallTile> walls = 
                    Board.InitWalls([
                        "####",
                        "#  #",
                        "####",
                    ]);

        walls[new Position(0, 0)].ShouldBe(WallTile.TopLeft); 
        walls[new Position(1, 0)].ShouldBe(WallTile.Vertical);
        walls[new Position(2, 0)].ShouldBe(WallTile.BottomLeft);

        walls[new Position(0, 1)].ShouldBe(WallTile.Horizontal);
        walls[new Position(0, 2)].ShouldBe(WallTile.Horizontal);
        walls[new Position(0, 3)].ShouldBe(WallTile.TopRight);

        walls[new Position(1, 3)].ShouldBe(WallTile.Vertical);
        walls[new Position(2, 3)].ShouldBe(WallTile.BottomRight);

        walls[new Position(0, 1)].ShouldBe(WallTile.Horizontal);
        walls[new Position(0, 2)].ShouldBe(WallTile.Horizontal);
        
    }

}