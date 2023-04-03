using System;
using UnityEngine;

namespace DonBigo.Tutorial
{
    [Serializable]
    public class LocalizedDialogueAsset
    {
        [SerializeField] private TextAsset asset;

        public void OnValidate()
        {
            if (asset != null) 
                JsonUtility.FromJsonOverwrite(asset.text, this);
        }
        
        [TextArea(2,5)]
        public string[] br;
        
        [TextArea(2,5)]
        public string[] en;

        [Header("Mobile")]
        [TextArea(2,5)]
        public string[] br_mo;
        
        [TextArea(2,5)]
        public string[] en_mo;

        
        public string[] Get(bool english, bool mobile)
        {
            var desk = english ? en : br;
            var mob = english ? en_mo : br_mo;
            return mobile && mob is { Length: > 0 } ? mob : desk;

            /*if (!Application.isMobilePlatform) return english ? en : br;
            return english switch
            {
                false when br_mo is { Length: > 0 } => br_mo,
                true when en_mo is { Length: > 0 } => en_mo,
                _ => english ? en : br
            };*/
        }
    }
}