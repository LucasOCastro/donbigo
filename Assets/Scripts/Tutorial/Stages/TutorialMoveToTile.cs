using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "MoveTutorialStage", menuName = "Tutorial/Move")]
    public class TutorialMoveToTile : TutorialStage
    {
        [SerializeField] private Vector2Int localTile;
        public override IEnumerator UpdateCoroutine()
        {
            yield return new WaitUntil(() => CharacterManager.DonBigo.Tile == GetTile(localTile));
        }
    }
}