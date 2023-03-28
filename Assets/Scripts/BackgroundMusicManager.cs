using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonBigo
{    
    public class BackgroundMusicManager : MonoBehaviour
    {
        private Scene currentScene;
        AudioSource source;

        [SerializeField] int sceneIndex;
        
        private void Awake()
        {
            SetMusic(ChooseAudio(sceneIndex));   
        }

        private AudioClip ChooseAudio(int sceneIndex)
        {
            if (sceneIndex == 0)
            {
                return Resources.Load<AudioClip>("BGMusics/BGM_violino");
            }
            else if (sceneIndex == 1)
            {
                return Resources.Load<AudioClip>("BGMusics/BGM_harpa");
            }
            else
            {
                return null;
            }
        }

        private void SetMusic(AudioClip chosenMusic)
        {
            source = GetComponent<AudioSource>();
            source.clip = chosenMusic;
            source.Play();
        }
    }
}