using DonBigo;
using UnityEngine;

namespace DonBigo.Actions
{
    public abstract class Action
    {
        public Entity Doer { get; }
        public Action(Entity doer)
        {
            if (doer == null)
            {
                Debug.LogError("Doer nulo em ação!");
            }
            Doer = doer;
        }

        public abstract void Execute();
    }
}