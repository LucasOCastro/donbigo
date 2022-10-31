using UnityEngine;

namespace DonBigo
{
    [ScriptableObjectIcon("prefab")]
    [CreateAssetMenu(fileName = "NewItem", menuName = "New Item")]
    public class ItemType : ScriptableObject
    {
        [SerializeField] private string itemName;
        public string ItemName => itemName;
        
        [SerializeField] private bool twoHanded;
        public bool TwoHanded => twoHanded;

        [SerializeField] private Item prefab;
        public Item Instantiate(Tile tile)
        {
            Item instance = Instantiate(prefab);
            instance.Tile = tile;
            return instance;
        }
    }
}