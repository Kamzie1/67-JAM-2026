using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace YANG
{
    public class Map
    {
        public List<Position> Highlights { get; private set; }
        private List<Object> _objects;
        private Board _board;

        public Map(int rows, int cols)
        {
            _board = new Board(rows, cols);
        }

        public bool TryGetObject(Position position, out Object obj)
        {
            obj = null;
            foreach (var o in _objects.Where(o => o.Position.Equals(position)))
            {
                obj = o;
                return true;
            }

            return false;
        }
        
        public int Rows  => _board.Rows;
        public int Cols => _board.Cols;

        public List<Position> MovablePositions(Position start)
        {
            List<Position> positions = new List<Position>();
            bool isObjectAtPosition = TryGetObject(start, out Object obj);
            if (isObjectAtPosition || obj is not Ship ship)
            {
                Highlights = positions;
                return Highlights;
            }

            var queue = new Queue<ValueTuple<Position, int>>();
            bool[][] visited = new bool[Rows][];
            for (int index = 0; index < Rows; index++)
            {
                visited[index] = new bool[Cols];
            }

            positions.Add(start);
            visited[start.X][start.Y] = true;
            queue.Enqueue(new ValueTuple<Position, int>(start, ship.Movement));
            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                Position position = tuple.Item1;
                int points = tuple.Item2;
                points--;
                foreach (Position n in position.Neighbours())
                {
                    if (!n.IsValid(Rows, Cols) || !visited[n.X][n.Y] || points < 0)
                    {
                        continue;
                    }

                    visited[n.X][n.Y] = true;
                    queue.Enqueue(new ValueTuple<Position, int>(n, points));
                    positions.Add(n);
                }
            }

            Highlights = positions;
            return Highlights;
        }

        public List<Position> AppliablePositions(Func<Object, bool> validateObject)
        {
            List<Position> positions = new List<Position>();
            foreach (var o in _objects.Where(o => validateObject(o)))
            {
                positions.Add(o.Position);
            }
            return positions;
        }

        public bool TryMove(Position start, Position end)
        {
            if (!Highlights.Contains(start) || !Highlights.Contains(end) || start.Equals(end))
            {
                return false;
            }
            bool isObjectAtPosition = TryGetObject(start, out Object obj);
            if (isObjectAtPosition || obj is not Ship ship)
            {
                return false;
            }
            ship.Position = end;
            ship.CanMove = false;
            return true;
        }
        
    }
}