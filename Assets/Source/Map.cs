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
        
        public Map(int rows, int cols, List<Object> objects)
        {
            _board = new Board(rows, cols);
            _objects = objects;
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

        public List<Position> AttackablePositions(Position start)
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
            queue.Enqueue(new ValueTuple<Position, int>(start, ship.Range));
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
                    foreach (var enemy in _objects)
                    {
                        if (enemy.PlayerId != ship.PlayerId && enemy.Position.Equals(n))
                        {
                            positions.Add(n);
                        }
                    }
                }
            }

            Highlights = positions;
            return Highlights;
        }

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
                    bool any_object = false;
                    foreach (var o in _objects)
                    {
                        if (o.Position.Equals(n))
                        {
                            any_object = true;
                            break;
                        }
                    }

                    if (!any_object)
                    {
                        positions.Add(n);
                    }
                }
            }

            Highlights = positions;
            return Highlights;
        }

        public List<Position> AppliablePositions(IReadOnlyList<int> ids)
        {
            List<Position> positions = new List<Position>();
            foreach (var o in _objects)
            {
                if (ids.Contains(o.Id))
                {
                    positions.Add(o.Position);
                }
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

        
        public bool TryAttack(Position start, Position end)
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
            bool isEnemyAtPosition = TryGetObject(end, out Object enemy);
            if (isEnemyAtPosition || enemy is not Ship enemyShip)
            {
                return false;
            }
            enemyShip.Health -= ship.Attack;
            if (enemyShip.Health < 0)
            {
                _objects.Remove(enemyShip);
            }
            return true;
        }

        public bool TryApply(ActionOnMap action, Position position)
        {
            if (!Highlights.Contains(position))
            {
                return false;
            }

            switch(action)
            {
                default:
                    return false;
            };

            return true;
        }
    }
}