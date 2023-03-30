using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonBigo
{    
    public class BackgroundMusicManager : MonoBehaviour
    {
        [SerializeField] int sceneIndex;
        private AudioSource _source;
        
        // Toca música on Awake da cena
        private void Awake()
        {
            SetMusic(ChooseAudio(sceneIndex));   
        }

        private void Update()
        {
            if (Settings.MuteMusic) _source.mute = true;
            else _source.mute = false;
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
            _source = GetComponent<AudioSource>();
            _source.clip = chosenMusic;
            _source.Play();
        }
    }
}