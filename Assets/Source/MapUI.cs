using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace YANG
{
    public class MapUI : MonoBehaviour
    {
        public Tilemap HexTilemap;
        public TileHighlighter Highlighter; 
        
        private Map _map;
        private Position? _start;
        private Position? _end;
        private int? _cardId = null;
        
        private Camera _mainCamera;

        void Start()
        {
            _mainCamera = Camera.main;
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
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3Int cellPosition = HexTilemap.WorldToCell(mousePos);
            Position position = new Position(cellPosition);
            if (_start == null)
            {
                _start = position;
                Highlighter.HighlightTiles(_map.MovablePositions(position).ConvertAll(Position.PositionToVector3));
            }
            else
            {
                _end = position;
                Highlighter.ClearHighlights();
            }
        }

        public void CardClicked(int id)
        {
            _cardId = id;
            
        }

    }
    
}