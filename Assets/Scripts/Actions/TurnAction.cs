using UnityEngine;

namespace DonBigo.Actions
{
    public class TurnAction : Action
    {
        private readonly Vector2Int _direction;
        public TurnAction(Entity doer, Vector2Int direction) : base(doer)
        {
            _direction = direction.Sign();
        }

        public override void Execute()
        {
            Doer.LookDirection = _direction;
        }
    }
}