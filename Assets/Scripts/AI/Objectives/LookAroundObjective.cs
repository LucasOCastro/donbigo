using System.Linq;
using DonBigo.Actions;
using UnityEngine;

namespace DonBigo.AI
{
    public class LookAroundObjective : AIObjective
    {
        private static readonly Vector2Int[] Directions = new[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1)
        };
        
        private readonly int[] _turns;
        public LookAroundObjective(AIWorker worker, RangeInt idleCountRange, RangeInt idleTurnsRange) : base(worker)
        {
            int idleCount = Random.Range(idleCountRange.start, idleCountRange.end);
            _turns = new int[idleCount];
            for (int i = 0; i < idleCount; i++)
            {
                int idleTurns = Random.Range(idleTurnsRange.start, idleTurnsRange.end);
                _turns[i] = idleTurns;
            }
        }

        public override bool Completed => _count >= _turns.Length;
        private int _count = -1;
        private int _timer;

        public override Action Tick()
        {
            if (Completed) return null;

            _timer++;
            if (_count >= 0 && _timer < _turns[_count]) return new IdleAction(Doer);

            
            _timer = 0;
            _count++;
            var direction = Directions.Where(d => d != Doer.LookDirection).Random();
            return new TurnAction(Doer, direction);
        }
    }
}