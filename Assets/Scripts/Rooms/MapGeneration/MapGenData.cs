using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo.Rooms.MapGeneration
{
    [System.Serializable]
    public struct MapGenData
    {
        [System.Serializable]
        public struct DecorationData
        {
            [Range(0f,1f)] public float chance;
            public Vector2Int elevationRange;
            public StructureTileType[] decorations;
        }
        
        public Tilemap tilemap;
        
        public int mapSize;
        public Vector2 normalizedGenStart;
        public Room startingRoom;
        public ItemType[] necessaryItems;
        
        [Header("Fillers")]
        public TileType fillerTile;
        public EntranceMarkerTile fillerMat;

        [Header("Traps")]
        [Range(0f,1f)] public float doorTrapChance;  
        public ItemType doorTrap;

        [Header("Decorations")] 
        public DecorationData floorDecorationData;
        public DecorationData wallDecorationData;
        public DecorationData cornerDecorationData;
    }
}