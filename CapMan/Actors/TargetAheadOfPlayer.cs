﻿namespace CapMan;

public class TargetAheadOfPlayer(int steps) : IEnemyBehaviour
{
    public int Steps { get; } = steps;
    public Direction GetNextDirection(Game game, double deltaTime, Actor actor)
    {
        var (board, direction, player) = (game.Board, actor.CurrentDirection, game.Player);
        Tile target = player.Tile;
        
        for (int step = 0; step < Steps; step++)
        {
            target = target.Step(player.CurrentDirection);
        }
        
        var(x,y) = board.WrapPosition(target.ToPosition());
        target = new((int)x, (int)y);

        return TargetTileBehaviour.DirectionWithShortestPath(board, actor.Position.NextTile(direction), direction, target);

    }
}
