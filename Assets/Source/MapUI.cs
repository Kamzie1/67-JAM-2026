using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace YANG
{
    public class MapUI : MonoBehaviour
    {
        public Tilemap hexTilemap;
        public TileHighlighter highlighter;
        public ShipsDatabase shipsDatabase;
        
        private Map _map;
        private Position? _start;
        private Position? _end;
        private ActionOnMap? _action;
        private List<Sprite> _sprites;
        private int _player;
        
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _map = new Map(15, 15);
            CreateFleet();
            foreach (var obj in _map.Objects)
            {
                _sprites.Add(Sprite.Create(obj));
            }
        }
        
        private void CreateFleet()
        {
           SpawnBase(new Position(7, 0), 0);
           SpawnBase(new Position(7, 14), 1);
           
           SpawnShip(new Position(7, 1), 0, 0);
           SpawnShip(new Position(7, 13), 1, 0);
        }

        private void SpawnBase(Position position, int playerId)
        {
            _map.Spawn(new Base(_map.Objects.Count, position, playerId)); 
            
        }

        private void SpawnShip(Position position, int playerId, int shipId)
        {
            ShipData data = shipsDatabase.GetShip(shipId);
            _map.Spawn(new Ship(_map.Objects.Count, position, playerId, shipId, data.Health, data.Damage, data.Movement, data.Range));
        }

        private void DestroySprite(Position position)
        {
            _map.TryGetObject(position, out var obj);
            Sprite toDestroy = null;
            foreach (var sprite in _sprites)
            {
                if (sprite.Id == obj.Id)
                {
                  toDestroy = sprite;
                  break;
                }
            }
            if (toDestroy is not null)
            {
                Destroy(toDestroy.gameObject);
                _sprites.Remove(toDestroy);
            }
        }

        private void UpdateSprite(Position position)
        {
            _map.TryGetObject(position, out var obj);
            foreach (var sprite in _sprites)
            {
                if (sprite.Id == obj.Id)
                {
                    Vector3 newPos = hexTilemap.GetCellCenterWorld(position.ToVector3());
                    sprite.transform.position = newPos;
                }
            }
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }
        }

        private void HandleClick()
        {
            if (_action == null)
            { 
                HandleScroll(); 
            }
            else
            {
                HandleCardAction();
            }
        }

        private void HandleScroll()
        {
            Vector3 target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _mainCamera.transform.LookAt(target); 
        }

        private void HandleCardAction()
        {
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3Int cellPosition = hexTilemap.WorldToCell(mousePos);
            Position position = new Position(cellPosition);
            if (!_map.Highlights.Contains(position))
            {
                ClearHighlights();
            }
            if (_start == null)
            {
                HandleFirstClick(position);
            }
            else
            {
                HandleSecondClick(position);
            }
        }

        private void HandleFirstClick(Position position)
        {
            _start = position;
            if (_action == ActionOnMap.Move)
            {
                highlighter.HighlightTiles(_map.MovablePositions((Position)_start).ConvertAll(Position.PositionToVector3));
            }
            else if(_action == ActionOnMap.Attack)
            {
                highlighter.HighlightTiles(_map.AttackablePositions((Position)_start).ConvertAll(Position.PositionToVector3));
            }
            else
            {
                _map.TryApply((ActionOnMap)_action, (Position)_start);
                ClearHighlights();
            }
        }
        private void HandleSecondClick(Position position)
        {
            _end = position;
            if (_action == ActionOnMap.Move)
            {
                Move();
            }
            else if(_action == ActionOnMap.Attack)
            {
                Attack();
            }
            ClearHighlights();
        }

        private void ClearHighlights()
        {
            highlighter.ClearHighlights();
            _start = null;
            _end = null;
            _action = null;
        }

        private void Move()
        {
            if (_map.TryMove((Position)_start, (Position)_end, _player))
            {
                UpdateSprite((Position) _end);
            }
        }

        private void Attack()
        {
            if (_map.TryAttackKill((Position)_start, (Position)_end, _player))
            {
                DestroySprite((Position)_end);
            }
        }

        public void Apply(ActionOnMap action, IReadOnlyList<int> ids, int playerId)
        {
            _action = action;
            _player = playerId;
            highlighter.HighlightTiles(_map.AppliablePositions(ids, _player).ConvertAll(Position.PositionToVector3));
        }

        public void Reset()
        {
            _map.Reset(shipsDatabase);
        }

    }
    
}