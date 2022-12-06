using UnityEngine;

namespace DonBigo
{
    public abstract class Action
    {
        protected Entity Doer { get; }
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