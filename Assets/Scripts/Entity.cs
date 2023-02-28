﻿using System;
using System.Collections;
using System.Collections.Generic;
using DonBigo.Actions;
using UnityEngine;
using Action = DonBigo.Actions.Action;

namespace DonBigo
{
    public abstract class Entity : TileObject, IVisibleTilesProvider
    {
        [field: SerializeField] public int VisionRange { get; set; } = 50;
        
        public Inventory Inventory { get; private set; }
        public HealthManager Health { get; private set; }
        public Memory Memory { get; } = new();
        public DirectionalSpriteSet SpriteSet { get; set; }
        
        public bool IsVenting { get; private set; }
        

        protected override void Awake()
        {
            base.Awake();
            Inventory = new Inventory(this);
            Health = new HealthManager(this);
        }

        public virtual void EnterVent(Vent vent)
        {
            if (IsVenting)
            {
                Debug.LogError("Entrou em vent sendo que já tá em vent!");
                return;
            }

            Tile = null;
            IsVenting = true;
        }

        public virtual void ExitVent(Vent vent)
        {
            if (!IsVenting)
            {
                Debug.LogError("Saiu de vent sem estar em vent!");
                return;
            }

            Tile = vent.UseTile;
            IsVenting = false;
        }


        private static Vector3 TileWorldPos(Tile tile)
        {
            Vector3 worldPos = tile.ParentGrid.TileToWorld(tile);
            worldPos.z = 2.5f;
            return worldPos;
        }
        private Tile _tile;
        public override Tile Tile
        {
            get => _tile;
            set
            {
                if (_tile == value) return;
                
                if (_tile != null)
                {
                    _tile.Entity = null;
                }

                Renderer.sprite = SpriteSet.GetDirectionalSprite(_tile, value);
                
                _tile = value;
                if (_tile == null)
                {
                    SetRenderVisibility(false);
                    VisibleTiles.Clear();
                    return;
                }
                transform.position = TileWorldPos(_tile);
                //_currentMoveCoroutine = StartCoroutine(MoveTransformCoroutine(worldPos));

                var oldVisible = VisibleTiles;
                VisibleTiles = ShadowCasting.Cast(_tile.ParentGrid, _tile.Pos, VisionRange);
                    
                OnUpdateViewEvent?.Invoke(oldVisible, VisibleTiles);
                UpdateRenderVisibility();
                    
                if (_tile.Entity != this)
                {
                    _tile.Entity = this;
                }
                Memory.RememberBeingAt(Tile);
            }
        }

        public void SetLookDirection(Vector2Int dir) => Renderer.sprite = SpriteSet.GetDirectionalSprite(dir);

        private Coroutine _currentMoveCoroutine;
        public void TranslateToTile(Tile tile, float time)
        {
            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
                _currentMoveCoroutine = null;
            }
            
            _currentMoveCoroutine = StartCoroutine(MoveTransformCoroutine(tile, time));
        }

        private IEnumerator MoveTransformCoroutine(Tile targetTile, float time)
        {
            Renderer.sprite = SpriteSet.GetDirectionalSprite(Tile, targetTile);
            Vector3 targetPos = TileWorldPos(targetTile);

            float distance = (targetPos - transform.position).magnitude;
            float speed = distance / time;
            while (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }
            _currentMoveCoroutine = null;
            Tile = targetTile;
        }

        public abstract Action GetAction();
        
        //Provavelmente seria mais pratico armazenar Tile ao inves de Vector2Int.
        public event IVisibleTilesProvider.OnUpdateViewDelegate OnUpdateViewEvent;
        public HashSet<Vector2Int> VisibleTiles { get; private set; }

        //Usar o SeesPlayer significa hardcodar o Player como o inimigo unico da IA.
        //Isso funciona pro projeto atualmente, mas pode complicar alguma coisa no futuro.
        public bool SeesPlayer => VisibleTiles.Contains(CharacterManager.DonBigo.Tile.Pos);
        public HashSet<Vector2Int> BlacklistedTiles { get; } = new();
    }
}