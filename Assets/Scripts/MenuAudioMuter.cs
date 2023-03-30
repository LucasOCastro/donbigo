using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class MenuAudioMuter : MonoBehaviour
    {
        private AudioSource _source;

        private void Awake() 
        {
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Settings.MuteMusic) _source.mute = true;
            else _source.mute = false;
        }
    }
}