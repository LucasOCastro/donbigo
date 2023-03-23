using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = DonBigo.Actions.Action;

namespace DonBigo
{
    public abstract class Entity : TileObject, IVisibleTilesProvider
    {
        [field: SerializeField] public int VisionRange { get; private set; } = 50;
     
        [field: Range(0,360)]
        [field: SerializeField] public float VisionAngle { get; private set; } = 90;
        
        [field: SerializeField] public float AimAccuracyBonus { get; private set; }
        
        [field: SerializeField] public DirectionalSpriteSet SpriteSet { get; private set; }
        
        public Inventory Inventory { get; private set; }
        public HealthManager Health { get; private set; }
        public Memory Memory { get; } = new();
        
        
        public bool IsVenting { get; private set; }
        
        public delegate void ExecuteActionDelegate(Action action);
        public ExecuteActionDelegate OnExecuteAction;

        protected override void Awake()
        {
            base.Awake();
            Inventory = new Inventory(this);
            Health = new HealthManager(this);
        }

        private void OnEnable()
        {
            OnExecuteAction += Memory.RememberAction;
        }
        private void OnDisable()
        {
            OnExecuteAction -= Memory.RememberAction;
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
                
                //Tira a entidade da tile atual
                if (_tile != null)
                {
                    _tile.Entity = null;
                }

                
                //Atualizar a direção da entidade
                if (_tile != null && value != null)
                {
                    LookDirection = (value.Pos - _tile.Pos).Sign();
                }
                else
                {
                    LookDirection = _defaultLookDirection;
                }
                
                //Atualiza o valor
                _tile = value;
                
                //Se settou a tile pra nulo, desativa o renderer porque está fora da grid.
                if (_tile == null)
                {
                    SetRenderVisibility(false);
                    VisibleTiles.Clear();
                    return;
                }
                
                //Caso contrário, move pra tile final e atualiza o FoV.
                transform.position = TileWorldPos(_tile);
                
                
                RefreshVisibleTiles(_tile, _lookDirection);
                    
                if (_tile.Entity != this)
                {
                    _tile.Entity = this;
                }
                Memory.RememberBeingAt(Tile);
            }
        }

        private static Vector2Int _defaultLookDirection = -Vector2Int.one;
        private Vector2Int _lookDirection;
        public Vector2Int LookDirection
        {
            get => _lookDirection;
            set
            {
                if (value == Vector2Int.zero)
                {
                    Debug.LogError("Settou direção de entidade pra vetor nulo.");
                    value = _defaultLookDirection;
                }
                _lookDirection = value.Sign();
                Renderer.sprite = SpriteSet.GetDirectionalSprite(_lookDirection);
                if (Tile != null)
                {
                    RefreshVisibleTiles(Tile, _lookDirection);
                }
            }
        }

        protected void RefreshVisibleTiles(Tile newTile, Vector2Int newDirection)
        {
            var oldVisible = VisibleTiles;
            VisibleTiles = ShadowCasting.Cast(newTile.ParentGrid, newTile.Pos, newDirection, VisionRange, VisionAngle);
            UpdateView(oldVisible, VisibleTiles);    
        }
        protected virtual void UpdateView(HashSet<Vector2Int> oldVisible, HashSet<Vector2Int> newVisible)
        {
            OnUpdateViewEvent?.Invoke(oldVisible, newVisible);
            UpdateRenderVisibility();
        }

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
            LookDirection = (targetTile.Pos - Tile.Pos).Sign();
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

        public override void Delete()
        {
            if (Inventory.LeftHand) Inventory.LeftHand.Delete();
            if (Inventory.RightHand) Inventory.RightHand.Delete();
            Destroy(gameObject);
        }
    }
}