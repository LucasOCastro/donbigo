﻿using System.Linq;
using DonBigo.Actions;
using Unity.VisualScripting;
using UnityEngine;

namespace DonBigo.AI
{
    public class AttackEntityObjective : GoToTargetObjective
    {
        private HealthManager _targetHealth;
        public AttackEntityObjective(AIWorker worker, Entity target) : base(worker, target)
        {
            _targetHealth = target.Health;
        }

        public override bool Completed => _targetHealth.Dead;

        private int ItemOffensivenessScore(Item item)
        {
            return item.Type.WeaponType.Score(t => t switch
            {
                WeaponUseType.Ranged => 2,
                WeaponUseType.Melee => 1,
                WeaponUseType.Lethal => _targetHealth.HasStatusOfType<StunStatus>() ? 20 : 0,
                _ => 0
            });
        }
        
        private Inventory.Handedness? GetAttackHandedness()
        {
            var inventory = Doer.Inventory;

            var possibleHandednesses =
                Inventory.AllHandednesses
                    .Where(h => inventory.GetHand(h) != null
                                && !inventory.GetHand(h).IsInCooldown
                                && inventory.GetHand(h).Type.WeaponType.HasFlag(WeaponUseType.Offensive)
                    );
                                
                
            if (!possibleHandednesses.Any()) return null;

            var chosenHandedness = possibleHandednesses
                .RandomElementByWeight(h => ItemOffensivenessScore(inventory.GetHand(h)));


            return chosenHandedness;
        }

        private Inventory.Handedness? _currentAttackHandedness;

        public override Action Tick()
        {
            //Melee: só ataca quando chega no player
            //Ranged: tenta atacar da distância que der
            //Trap: tenta colocar no caminho do player, ou entre si e o player (se tiver ranged)
            //Também tem que considerar usar uma netgun antes de dar tiro, por exemplo

            if (_currentAttackHandedness == null )
            {
                _currentAttackHandedness = GetAttackHandedness();
                if (_currentAttackHandedness == null)
                {
                    Debug.LogError("NAO ACHOU ITEM PARA ATAQUE!");
                    return null;
                }
            }
            
            Item attackItem = Doer.Inventory.GetHand(_currentAttackHandedness.Value);
            if (attackItem == null)
            {
                _currentAttackHandedness = null;
                return Tick();
            }

            if (attackItem.Type.WeaponType.HasFlag(WeaponUseType.Melee) && !IsAdjacentToTarget)
            {
                return base.Tick();
            }

            var action = new UseItemAction(Doer, attackItem, _targetHealth.Owner.Tile);
            _currentAttackHandedness = null;
            return action;
        }
    }
}