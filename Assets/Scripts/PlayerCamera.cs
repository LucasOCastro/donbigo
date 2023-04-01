using System;
using System.Collections;
using System.Collections.Generic;
using DonBigo.UI;
using UnityEngine;

namespace DonBigo
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Vector2 offset = new Vector2(0, 1);

        [SerializeField] private float jumpSeconds = 1.5f;

        [SerializeField] private bool DEBUG_followPhantonette;
        public static bool DEBUG_PHANTONETTE;
        
        private Vector3 ToWorldPos(Vector2 pos) => new Vector3(pos.x, pos.y, -10) + (Vector3)offset;

        private bool _jump;

        private Shaker _shaker;
        private void Awake()
        {
            _shaker = GetComponent<Shaker>();
        }

        private void LateUpdate()
        {
            if (_jump) return;
            
            DEBUG_PHANTONETTE = DEBUG_followPhantonette;
            FieldOfViewRenderer.Origin = DEBUG_PHANTONETTE ? CharacterManager.Phantonette : CharacterManager.DonBigo;
            
            Entity player = CharacterManager.DonBigo;

            if (DEBUG_followPhantonette) player = CharacterManager.Phantonette;
            
            if (player == null) return;

            transform.position = ToWorldPos(player.transform.position);
            if (_shaker) transform.position += (Vector3)_shaker.ShakeOffset;
        }

        public void Jump(IVisibleTilesProvider origin) 
            => StartCoroutine(JumpCoroutine(origin));

        private IEnumerator JumpCoroutine(IVisibleTilesProvider origin)
        {
            _jump = true;
            
            //Salva os valores atuais pra restaurar ao final do pulo
            var previousPos = transform.position;
            var previousOrigin = FieldOfViewRenderer.Origin;

            //Pula para o alvo
            FieldOfViewRenderer.Origin = origin;
            var targetPos = origin.Tile.ParentGrid.TileToWorld(origin.Tile);
            transform.position = ToWorldPos(targetPos);

            //Espera
            yield return new WaitForSeconds(jumpSeconds);

            //Restaura para os valores originais
            transform.position = previousPos;
            FieldOfViewRenderer.Origin = previousOrigin;

            _jump = false;
        }
    }
}