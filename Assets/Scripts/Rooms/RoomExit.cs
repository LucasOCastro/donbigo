using System;
using UnityEngine;

namespace DonBigo.Rooms
{
    [Serializable]
    public struct RoomExit
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
    }
}