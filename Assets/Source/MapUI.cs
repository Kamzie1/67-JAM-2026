using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace YANG
{
    public class MapUI : MonoBehaviour
    {
        public Tilemap hexTilemap;
        public TileHighlighter highlighter;
        public ShipsDatabase shipsDatabase;
        public Camera mainCamera;
        public TMP_Text Stats;

        private Vector3 origin;
        
        private Map _map;
        private Position? _start;
        private Position? _end;
        private ActionOnMap? _action;
        private List<Sprite> _sprites;
        private int _player;

        private void Awake()
        {
            mainCamera = Camera.main;
            _map = new Map(15, 15);
            CreateFleet();
            _sprites = new List<Sprite>();
            foreach (var obj in _map.Objects)
            {
                _sprites.Add(Sprite.Create(obj));
            }
        }
        
        void Start()
        {
            origin = transform.position;
        }
        
        private void CreateFleet()
        {
           SpawnBase(new Position(7, 0), 0);
           SpawnBase(new Position(8, 2), 1);
           
           SpawnShip(new Position(7, 1), 0, 0);
           SpawnShip(new Position(7, 3), 1, 0);
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
                _map.Objects.Remove(obj);
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
                    Vector3 vec = new Vector3(newPos.y - _map.Cols/2,_map.Rows/2  - newPos.x, 0);
                    sprite.transform.position = vec;
                }
            }
        }
        
        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleClick();
            }
            else if (Mouse.current.middleButton.wasPressedThisFrame)
            {
               SetOrigin(); 
            }
            
            if (Mouse.current.middleButton.isPressed)
            {
                MoveByOffset();
            }
        }
        
        void SetOrigin()
        {
            origin = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        private void MoveByOffset()
        {
            Vector3 currentMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 difference = origin - currentMousePos;
        
            mainCamera.transform.position += difference;
        }        
        private void HandleClick()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; 
            }
            Debug.Log(_action+ ", "+ _start + ", "+ _end);
            if (_action == null)
            { 
                HandleLeftClick(); 
            }
            else
            {
                HandleCardAction();
            }
        }

        private void HandleLeftClick()
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;

            Vector3Int cellPosition = hexTilemap.WorldToCell(mousePos);
            Position position = ToPosition(cellPosition);
            if (!position.IsValid(_map.Rows, _map.Cols))
            {
                ClearHighlights();
                return;
            }
            Debug.Log(position.X + " " + position.Y);
            if (_map.TryGetObject(position, out var obj))
            {
                Stats.text = ToText(obj);
                highlighter.HighlightTiles(new List<Position>(){ToPosition(cellPosition)}, _map.Rows, _map.Cols);
            }
            else
            {
                Debug.Log("Clearing");
                Stats.text = "";
                ClearHighlights();
            }
            
        }

        private Position ToPosition(Vector3Int cellPosition)
        {
            return new Position(_map.Cols/2 -cellPosition.y, cellPosition.x + _map.Rows / 2);
        }

        private string ToText(Object obj)
        {
            if (obj is Base baza)
            {
                return $@"Base: {baza.PlayerId}\nHealth: {baza.Health}";
            }
            if (obj is Ship ship)
            {
                int id = ship.ShipId;
                ShipData data = shipsDatabase.GetShip(id);
                if (data is null)
                {
                    return "";
                }
                return $@"\t{data.name}\n{data.Description}\n-------\nHealth: {ship.Health}\nDamage: {ship.Damage}\nMovement: {ship.Movement}\nRange: {ship.Range}";
            }

            return "";
        }

        private void HandleCardAction()
        {
            Debug.Log(_action + ", " + _start + ", " + _end);
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;

            Vector3Int cellPosition = hexTilemap.WorldToCell(mousePos);
            Position position = ToPosition(cellPosition);
            if (!_map.Highlights.Contains(position) || !position.IsValid(_map.Rows, _map.Cols))
            {
                Debug.Log("Clear not in high");
                ClearHighlights();
                return;
            }
            if (_start is null)
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
            Debug.Log(position.X + " " + position.Y);
            _start = position;
            if (_action == ActionOnMap.Move)
            {
                Debug.Log("highlight moves");
                highlighter.HighlightTiles(_map.MovablePositions((Position)_start), _map.Rows, _map.Cols);
            }
            else if(_action == ActionOnMap.Attack)
            {
                highlighter.HighlightTiles(_map.AttackablePositions((Position)_start), _map.Rows, _map.Cols);
            }
            else
            {
                _map.TryApply((ActionOnMap)_action, (Position)_start);
                ClearHighlights();
            }
        }
        private void HandleSecondClick(Position position)
        {
            Debug.Log(position.X + " " + position.Y);
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
            highlighter.HighlightTiles(_map.AppliablePositions(action, ids, _player), _map.Rows, _map.Cols);
        }

        public void Reset()
        {
            _map.Reset(shipsDatabase);
        }

    }
    
}