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
            Debug.Log("Starting tutorial");
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
            phantonette.LookDirection = phantonetteDir;

            donbigoPos += room.Bounds.min;
            donbigo.Tile = grid[donbigoPos];
            donbigo.LookDirection = donbigoDir;
        }

        private IEnumerator TutorialCoroutine()
        {
            yield return null;
            AdjustEntitiesPositions();
            
            foreach (var stage in stages)
            {
                yield return stage.UpdateCoroutine();
            }
            GameEnder.Instance.EndGame(GameEnder.Condition.Exit);
        }
        
    }
}