﻿using System.Linq;
using UnityEngine;

namespace DonBigo.AI
{
    public class AlertedState : AIState
    {
        private AttackEntityObjective _attackObjective;
        private FleeObjective _fleeObjective;

        protected override AIState OnTick(AIWorker worker, out AIObjective objective)
        {
            var entity = worker.Owner;
            if (!entity.SeesPlayer)
            {
                objective = null;
                return new WanderState();
            }

            var player = CharacterManager.DonBigo;
            if (_attackObjective == null || _fleeObjective == null)
            {
                _attackObjective = new AttackEntityObjective(worker, player);
                //TODO lidar com essa fightBackChance
                _fleeObjective = new FleeObjective(worker, player, 1);
            }

            if (player.Health.HasStatusOfType<StunStatus>())
            {
                Debug.Log($"Player stunnado e {(entity.Inventory.HasLethal ? "" : "não ")}tenho lethal.");
                objective = entity.Inventory.HasLethal ? _attackObjective : _fleeObjective;
            }
            else
            {
                Debug.Log("Stronger = "+ worker.FeelsStrong);
                objective = worker.FeelsStrong ? _attackObjective : _fleeObjective;
            }

            return null;
        }
    }
}