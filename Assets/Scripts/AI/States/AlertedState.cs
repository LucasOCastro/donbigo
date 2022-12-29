﻿using UnityEngine;

namespace DonBigo.AI
{
    public class AlertedState : AIState
    {
        private AttackEntityObjective _attackObjective;
        private FleeObjective _fleeObjective;

        private int _minPower;
        public AlertedState()
        {
            _minPower = UnityEngine.Random.Range(50, 100);
        }
        
        private bool StrongerThan(Entity a, Entity b)
        {
            //return a.Inventory.CombatPower > b.Inventory.CombatPower;
            return a.Inventory.CombatPower >= _minPower;
        }

        protected override AIState OnTick(Entity entity, out AIObjective objective)
        {
            //TODO Desse jeito, ele para de fugir/perseguir quando perde o inimigo de vista, o que é bem ineficaz.
            if (!entity.SeesPlayer)
            {
                objective = null;
                return new WanderState();
            }

            var player = CharacterManager.DonBigo;
            if (_attackObjective == null || _fleeObjective == null)
            {
                _attackObjective = new AttackEntityObjective(entity, player);
                //TODO lidar com essa fightBackChance
                _fleeObjective = new FleeObjective(entity, player, 1);
            }
            
            if (player.Health.HasStatusOfType<StunStatus>())
            {
                Debug.Log($"Player stunnado e {(entity.Inventory.HasLethal ? "" : "não")} tenho lethal.");
                objective = entity.Inventory.HasLethal ? _attackObjective : _fleeObjective;
            }
            else
            {
                bool stronger = StrongerThan(entity, player);
                Debug.Log("Stronger = "+ stronger);
                objective = stronger ? _attackObjective : _fleeObjective;
            }

            return null;
        }
    }
}