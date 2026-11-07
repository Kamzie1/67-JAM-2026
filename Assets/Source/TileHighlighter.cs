using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace YANG
{
    public class TileHighlighter : MonoBehaviour
    {
        [Header("Tilemaps")] public Tilemap highlightMap;

        [Header("Tiles")] public TileBase highlightTile;

        public void HighlightTiles(List<Vector3Int> tiles)
        {
            highlightMap.ClearAllTiles();

            foreach (Vector3Int hexCoords in tiles)
            {
                highlightMap.SetTile(hexCoords, highlightTile);
            }
        }

        public void ClearHighlights()
        {
            highlightMap.ClearAllTiles();
        }
    }
}