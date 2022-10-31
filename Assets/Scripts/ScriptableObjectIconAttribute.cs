using System;
using UnityEngine;

namespace DonBigo
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptableObjectIconAttribute : PropertyAttribute
    {
        public string PropName { get; }
        public ScriptableObjectIconAttribute(string propName)
        {
            PropName = propName;
        }
    }
}