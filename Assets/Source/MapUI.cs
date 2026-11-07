using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Debug = System.Diagnostics.Debug;

namespace YANG
{
    public class MapUI : MonoBehaviour
    {
        public Tilemap hexTilemap;
        public TileHighlighter highlighter; 
        
        private Map _map;
        private Position? _start;
        private Position? _end;
        private ActionOnMap? _action;
        private List<ObjectUI> _objects;
        private List<Object> starting_objects;
        
        private Camera _mainCamera;

        void Start()
        {
            _mainCamera = Camera.main;
            starting_objects= new List<Object>();
            starting_objects.Add( new Ship(0, new Position(0,0)));
            _map = new Map(15, 15, starting_objects);
        }

        void Awake()
        {
            foreach (var obj in starting_objects)
            {
                _objects.Add(ObjectUI.Create(obj));
            }
        }

        public void DestroyObject()
        {
            
        }

        public void UpdateObject(Position end)
        {
            Object obj;
            _map.TryGetObject(end, out obj);
            foreach (var objectUI in _objects)
            {
                if (objectUI.Id == obj.Id)
                {
                    Vector3 new_pos = hexTilemap.GetCellCenterWorld(end.ToVector3());
                    objectUI.transform.position = new_pos;
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

        public void HandleClick()
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

        public void HandleScroll()
        {
            Vector3 target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _mainCamera.transform.LookAt(target); 
        }

        public void HandleCardAction()
        {
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3Int cellPosition = hexTilemap.WorldToCell(mousePos);
            Position position = new Position(cellPosition);
            if (_start == null)
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
                    _map.TryApply((ActionOnMap)_action!, (Position)_start);
                    highlighter.ClearHighlights();
                    _start = null;
                    _action = null;
                }
            }
            else
            {
                _end = position;
                if (_action == ActionOnMap.Move)
                {
                    bool success = _map.TryMove((Position)_start, (Position)_end);
                    if (success)
                    {
                        UpdateObject((Position) _end);
                    }
                }
                else if(_action == ActionOnMap.Attack)
                {
                    bool success = _map.TryAttack((Position)_start, (Position)_end);
                }
                highlighter.ClearHighlights();
                _start = null;
                _end = null;
                _action = null;
            }
        }

        public void Highlight(List<int> ids)
        {
            highlighter.HighlightTiles(_map.AppliablePositions(ids).ConvertAll(Position.PositionToVector3));
        }
        
        public void Apply(ActionOnMap action)
        {
            _action = action;
        }

    }
    
}