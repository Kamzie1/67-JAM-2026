using System;
using UnityEngine;

namespace YANG
{
    public struct Position : IEquatable<Position>
    {
        public int X;
        public int Y;
        
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Position(Vector3Int cellPosition)
        {
            this.X = cellPosition.x;
            this.Y = cellPosition.y;
        }

        public Position[] Neighbours()
        {
            Position[] res = new Position[6];
            res[0].X = X;
            res[0].Y = Y - 1;

            res[1].X = X + 1;
            res[1].Y = Y - (((X & 1) == 1) ? 1 : 0);

            res[2].X = X + 1;
            res[2].Y = Y + (((X & 1) == 0) ? 1 : 0);

            res[3].X = X;
            res[3].Y = Y + 1;

            res[4].X = X - 1;
            res[4].Y = Y + (((X & 1) == 0) ? 1 : 0);

            res[5].X = X - 1;
            res[5].Y = Y - (((X & 1) == 1) ? 1 : 0);
            return res;
        }

        public readonly bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }
        
        public bool IsValid(int rows, int cols)
        {
            return X >= 0 && Y >= 0 && X < rows && Y < cols;
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Vector3Int PositionToVector3(Position p)
        {
           return new Vector3Int(p.X, p.Y, 0); 
        }

        public Vector3Int ToVector3()
        {
            return new Vector3Int(X , Y, 0);
        }
    }
}