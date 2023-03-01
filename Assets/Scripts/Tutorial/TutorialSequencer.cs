using System.Collections;
using UnityEngine;
using DonBigo.Tutorial.Stages;

namespace DonBigo.Tutorial
{
    public class TutorialSequencer : MonoBehaviour
    {
        [SerializeField] private TutorialStage[] stages;
        [SerializeField] private Vector2Int phantonettePos,phantonetteDir;
        [SerializeField] private Vector2Int donbigoPos, donbigoDir;

        private void Start()
        {
            StartCoroutine(TutorialCoroutine());
        }

        private void AdjustEntitiesPositions()
        {
            var grid = GridManager.Instance.Grid;
            var room = grid.AllRooms[0];
            var phantonette = CharacterManager.Phantonette;
            var donbigo = CharacterManager.DonBigo;
            phantonette.Tile = null;
            donbigo.Tile = null;

            phantonettePos += room.Bounds.min;
            phantonette.Tile = grid[phantonettePos];
            phantonette.SetLookDirection(phantonetteDir);

            donbigoPos += room.Bounds.min;
            donbigo.Tile = grid[donbigoPos];
            donbigo.SetLookDirection(donbigoDir);
        }

        private IEnumerator TutorialCoroutine()
        {
            AdjustEntitiesPositions();
            
            foreach (var stage in stages)
            {
                yield return stage.UpdateCoroutine();
            }
            GameEnder.Instance.EndGame(GameEnder.Condition.Exit);
        }
        
    }
}