using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Vector2 offset = new Vector2(0, 1);

        [SerializeField] private bool DEBUG_followPhantonette;
        public static bool DEBUG_PHANTONETTE;
        private void LateUpdate()
        {
            DEBUG_PHANTONETTE = DEBUG_followPhantonette;
            FieldOfViewRenderer.Origin = DEBUG_PHANTONETTE ? CharacterManager.Phantonette : CharacterManager.DonBigo;
            
            Entity player = CharacterManager.DonBigo;

            if (DEBUG_followPhantonette) player = CharacterManager.Phantonette;
            
            if (player == null) return;
            
            var pos = player.transform.position;
            transform.position = new Vector3(pos.x, pos.y, -10) + (Vector3)offset;
        }
    }
}