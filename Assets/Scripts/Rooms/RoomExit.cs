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
        public Direction ExitDirection => direction;
        public Vector2Int Position => localPos;

        public Vector2Int DirectionVector => ExitDirection switch {
                Direction.Up => Vector2Int.up,
                Direction.Down => Vector2Int.down,
                Direction.Right => Vector2Int.right,
                Direction.Left => Vector2Int.left,
                _ => throw new IndexOutOfRangeException()
            };

        public RoomExit(Vector2Int localPosition, Direction dir)
        {
            localPos = localPosition;
            direction = dir;
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