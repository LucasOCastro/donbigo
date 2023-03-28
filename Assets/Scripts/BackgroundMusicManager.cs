using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonBigo
{    
    public class BackgroundMusicManager : MonoBehaviour
    {
        private Scene currentScene;
        private AudioSource source;

        [SerializeField] int sceneIndex;
        
        // Toca música on Awake da cena
        private void Awake()
        {
            SetMusic(ChooseAudio(sceneIndex));   
        }

        // Escolhe um clipe baseado na cena atual
        private AudioClip ChooseAudio(int sceneIndex)
        {
            if (sceneIndex == 0) // Game Scene
            {
                return Resources.Load<AudioClip>("BGMusics/BGM_violino");
            }
            else if (sceneIndex == 1) // Tutorial
            {
                return Resources.Load<AudioClip>("BGMusics/BGM_harpa");
            }
            else
            {
                return null;
            }
        }

        // Seta o clipe escolhido no emissor de audio e bota pra rodar
        private void SetMusic(AudioClip chosenMusic)
        {
            source = GetComponent<AudioSource>();
            source.clip = chosenMusic;
            source.Play();
        }
    }
}