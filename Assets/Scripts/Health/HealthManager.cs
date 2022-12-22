﻿using System.Collections.Generic;
using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    public class HealthManager
    {
        public event System.Action OnDeathEvent;
        
        public Entity Owner { get; }
        public bool Dead { get; private set; }

        private readonly List<HealthStatus> _statusList = new List<HealthStatus>();
        public HealthManager(Entity owner)
        {
            Owner = owner;
        }

        public void Kill(Sprite icon = null)
        {
            if (Dead) return;
            
            foreach (var status in _statusList)
            {
                status.End(this);
            }
            _statusList.Clear();
            
            AddStatus(new DeadStatus(), icon);
            OnDeathEvent?.Invoke();
            Owner.Tile = null;
            Dead = true;
        }
        
        /// <summary>
        /// Ticka todos os status e tenta gerar uma ação mais prioritária dentre eles.
        /// </summary>
        public Action Tick()
        {
            if (Dead) return new IdleAction(Owner);
            
            for (var i = 0; i < _statusList.Count; i++)
            {
                var status = _statusList[i];
                bool shouldEnd = status.Tick(this);
                if (shouldEnd)
                {
                    status.End(this);
                    _statusList.RemoveAt(i);
                    i--;
                }
            }

            foreach (var status in _statusList)
            {
                var action = status.GenAction(this);
                if (action != null)
                {
                    return action;
                }
            }
            return null;
        }

        public void AddStatus(HealthStatus status, Sprite icon = null)
        {
            _statusList.Add(status);
            status.Start(this);
            if (icon != null)
            {
                StatusIconManager.Instance.MakeIcon(this, status, icon);
            }
        }
    }
}