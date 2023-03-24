using System;
using UnityEngine;

namespace DonBigo.UI
{
    public class TileHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private int z = 1;

        private static TileHighlighter _instance;
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void HighlightPrivate(Tile tile)
        {
            if (tile == null)
            {
                highlight.gameObject.SetActive(false);
                return;
            }

            highlight.gameObject.SetActive(true);
            Vector3 pos = GridManager.Instance.Grid.TileToWorld(tile.Pos);
            pos.z = z;
            highlight.transform.position = pos;
        }

        public static void Highlight(Tile tile) => _instance.HighlightPrivate(tile);
    }
}