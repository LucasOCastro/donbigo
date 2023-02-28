﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    public abstract class TutorialStage : ScriptableObject
    {
        public abstract IEnumerator UpdateCoroutine();

        protected static IEnumerator WaitForDialogue(IEnumerable<string> dialogue)
        {
            DialogueShower.Instance.ShowDialogue(dialogue);
            yield return new WaitUntil(() => !DialogueShower.Instance.Shown);
        } 
    }
}