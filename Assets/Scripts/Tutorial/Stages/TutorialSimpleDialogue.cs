using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "DialogueTutorialStage", menuName = "Tutorial/Dialogue")]
    public class TutorialSimpleDialogue : TutorialStage
    {
        /*[MenuItem("Assets/Gen Json Assets")]
        public static void GenJsonAssets()
        {
            foreach (var obj in Selection.objects)
            {
                if (obj is not TutorialSimpleDialogue dialogue) continue;
                
                string text = JsonUtility.ToJson(dialogue, prettyPrint: true)
                    .Replace(nameof(dialogue.dialogues), "br")
                    .Replace(nameof(dialogue.englishDialogues), "en");

                string assetPath = AssetDatabase.GetAssetPath(dialogue)
                    .Replace("Assets", "")
                    .Replace(".asset", ".json");
                File.WriteAllText(Application.dataPath + assetPath, text);
            }
            AssetDatabase.Refresh();
        }*/

        [SerializeField] private LocalizedDialogueAsset dialogueAsset;
        
        private void OnValidate() => dialogueAsset?.OnValidate();

        public override IEnumerator UpdateCoroutine()
        {
            yield return WaitForDialogue(dialogueAsset.Get(Settings.English));
        }
    }
}