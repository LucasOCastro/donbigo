using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class PlayerCamera : MonoBehaviour
    {
        public GameObject player;

        private void Start() 
        {
            // player = GameObject.FindGameObjectWithTag("Player");
        }
        private void LateUpdate()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            var pos = player.transform.position;
            transform.position = new Vector3(pos.x, pos.y + 1, -10);
        }
    }
}