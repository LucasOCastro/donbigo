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
            if (asset != null) JsonUtility.FromJsonOverwrite(asset.text, this);
        }
        
        [TextArea(2,5)]
        public string[] br;
        
        [TextArea(2,5)]
        public string[] en;

        public string[] Get(bool english) => english ? en : br;
    }
}