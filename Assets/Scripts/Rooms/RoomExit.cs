using System;
using DonBigo.Actions;
using UnityEngine;
using Action = DonBigo.Actions.Action;

namespace DonBigo.Rooms
{
    [Serializable]
    public struct RoomExit : IRoomExit
    {
        public enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }

        [SerializeField] private Direction direction;
        [SerializeField] private Vector2Int localPos;
        // Unity não serializa interfaces, então armazeno como um unity object
        // Eu ODEIO isso vtnc unity por alguma razao SerializeReference nao suporta interface ????
        [SerializeField] private UnityEngine.Object marker;

        public Direction ExitDirection => direction;

        public Direction OpposedDirection => ExitDirection switch {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Right => Direction.Left,
            Direction.Left => Direction.Right,
            _ => throw new IndexOutOfRangeException()
        };
        
        public Vector2Int Position => localPos;

        public IRoomEntranceMarker Marker => marker as IRoomEntranceMarker;

        public Vector2Int DirectionVector => ExitDirection switch {
                Direction.Up => Vector2Int.up,
                Direction.Down => Vector2Int.down,
                Direction.Right => Vector2Int.right,
                Direction.Left => Vector2Int.left,
                _ => throw new IndexOutOfRangeException()
            };

        public RoomExit(Vector2Int localPosition, Direction dir, IRoomEntranceMarker entranceMarker)
        {
            localPos = localPosition;
            direction = dir;
            marker = entranceMarker as UnityEngine.Object;
        }

        public bool IsOpposed(Direction other)
        {
            return ExitDirection switch
            {
                Direction.Up => other == Direction.Down,
                Direction.Down => other == Direction.Up,
                Direction.Right => other == Direction.Left,
                Direction.Left => other == Direction.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public Action GenAction(Entity doer)
        {
            return new UseDoorAction(doer, this);
        }

        public Tile UseTile(GameGrid grid)
        {
            return RoomExitUtility.FindWalkable(Position, -DirectionVector, grid);
        }

        public Tile FinalTile(GameGrid grid)
        {
            return RoomExitUtility.FindWalkable(Position + DirectionVector, DirectionVector, grid);
        }
    }
}