using System;
using System.Collections;
using System.Collections.Generic;
using DonBigo.Actions;
using TMPro;
using UnityEngine;

namespace DonBigo.Tutorial
{
    public class DialogueShower : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueContainer;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private KeyCode skipKey;
        [SerializeField] private float secondsBetweenLetters;
        
        public static DialogueShower Instance { get; private set; }

        public bool Shown => dialogueContainer.activeSelf;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void SetShown(bool shown)
        {
            TurnManager.Instance.gameObject.SetActive(!shown);
            text.text = "";
            dialogueContainer.SetActive(shown);
        }

        public void ShowDialogue(IEnumerable<string> dialogues)
        {
            StopAllCoroutines();
            SetShown(true);
            StartCoroutine(DisplaySequenceCoroutine(dialogues));
        }

        private IEnumerator DisplaySequenceCoroutine(IEnumerable<string> dialogues)
        {
            foreach (var dialogue in dialogues)
            {
                yield return DisplayCoroutine(dialogue);
                yield return new WaitUntil(() => Input.GetKeyDown(skipKey));
                yield return null;
            }
            SetShown(false);
        }

        private static IEnumerator WaitOrInterrupt(float seconds, Func<bool> interrupt)
        {
            float t = 0;
            while (t < seconds && !interrupt())
            {
                t += Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator DisplayCoroutine(string dialogue)
        {
            if (secondsBetweenLetters == 0)
            {
                text.text = dialogue;
                yield break;
            }

            text.text = "";
            for (int i = 0; i < dialogue.Length; i++)
            {
                //Tags especiais no TMPro são marcadas por <>
                if (dialogue[i] == '<')
                {
                    int closeIndex = dialogue.IndexOf('>', i + 1);
                    text.text += dialogue.Substring(i, closeIndex - i + 1);
                    i = closeIndex;
                    continue;
                }
                
                text.text += dialogue[i];
                
                bool interrupt = false;
                yield return WaitOrInterrupt(secondsBetweenLetters, () => interrupt = Input.GetKeyDown(skipKey));
                if (interrupt)
                {
                    text.text = dialogue;
                    yield return null;
                    break;
                }
            }
        }
    }
}