using UnityEngine.WSA;

namespace YANG
{
    public class Board
    {
        public int Cols { get; } 
        public int Rows { get; }
        private readonly int[,] _tiles;

        public Board(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _tiles = new int[rows, cols];
        }
        
        public int this[int i, int j] => _tiles[i,j];
        public int this[Position p] => _tiles[p.X, p.Y];
    }
}