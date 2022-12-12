﻿using DonBigo;
using UnityEngine;

namespace DonBigo.Actions
{
    /// <summary>
    /// Ação responsável por mover uma entidade em 1 tile.
    /// </summary>
    public class MoveAction : Action
    {
        private Tile _target;
        public MoveAction(Entity doer, Tile target) : base(doer)
        {
            if (target == null)
            {
                Debug.LogError("Target nulo em MoveAction!");
            }
            _target = target;
        }

        public override void Execute()
        {
            Doer.Tile = _target;
        }
    }
}