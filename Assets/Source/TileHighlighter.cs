using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace YANG
{
    public class TileHighlighter : MonoBehaviour
    {
        public Tilemap highlightMap;

        public TileBase highlightTile;

        public void HighlightTiles(List<Position> tiles, int rows, int cols)
        {
            highlightMap.ClearAllTiles();

            foreach (var hexCoords in tiles)
            {
                Vector3Int position = new Vector3Int(hexCoords.Y-cols/2, rows/2 - hexCoords.X, 0);
                highlightMap.SetTile(position, highlightTile);
            }
        }

        public void ClearHighlights()
        {
            highlightMap.ClearAllTiles();
        }
    }
}