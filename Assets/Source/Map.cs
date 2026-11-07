using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace YANG
{
    public class Map
    {
        public List<Position> Highlights { get; private set; } 
        public List<Object> Objects { get; }
        private readonly Board _board;

        public int Rows => _board.Rows;
        public int Cols => _board.Cols;

        public Map(int rows, int cols)
        {
            _board = new Board(rows, cols);
            Objects = new List<Object>();
        }
        

        public void Spawn(Object o)
        {
            Objects.Add(o);
        }

        public bool TryGetObject(Position position, out Object obj)
        {
            obj = null;
            foreach (var o in Objects.Where(o => o.Position.Equals(position)))
            {
                obj = o;
                return true;
            }

            return false;
        }

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
                    foreach (var enemy in Objects)
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
                    bool anyObject = false;
                    foreach (var o in Objects)
                    {
                        if (o.Position.Equals(n))
                        {
                            anyObject = true;
                            break;
                        }
                    }

                    if (!anyObject)
                    {
                        positions.Add(n);
                    }
                }
            }

            Highlights = positions;
            return Highlights;
        }

        public List<Position> AppliablePositions(IReadOnlyList<int> ids, int playerId)
        {
            List<Position> positions = new List<Position>();
            foreach (var o in Objects)
            {
                if (ids.Contains(o.Id))
                {
                    if (o.PlayerId != playerId)
                    {
                        continue;
                    }
                    positions.Add(o.Position);
                }
            }

            return positions;
        }

        public bool TryMove(Position start, Position end, int playerId)
        {
            if (!ValidateMove(start, end, playerId, out var ship)) return false;
            ship.Position = end;
            ship.CanMove = false;
            return true;
        }

        private bool ValidateMove(Position start, Position end, int playerId, out Ship ship)
        {
            ship = null;
            if (!Highlights.Contains(start) || !Highlights.Contains(end) || start.Equals(end))
            {
                return false;
            }

            bool isObjectAtPosition = TryGetObject(start, out Object obj);
            if (isObjectAtPosition || obj is not Ship getShip)
            {
                return false;
            }

            if (!getShip.CanMove || getShip.PlayerId != playerId)
            {
                return false;
            }
            ship = getShip;
            return true;
        }


        public bool TryAttackKill(Position start, Position end, int playerId)
        {
            if (!ValidateAttack(start, end, playerId, out var ship, out var enemy)) return false;
            enemy.Health -= ship.Damage;
            if (enemy.Health < 0)
            {
                Objects.Remove(enemy);
                if (enemy is Base)
                {
                    GameEvents.DeadPlayer?.Invoke(enemy.PlayerId);
                    return true;
                }
                CheckPlayerDeath(enemy.Id);
                return true;
            }


            return false;
        }

        private void CheckPlayerDeath(int playerId)
        {
            foreach (var obj in Objects )
            {
                if (obj is Ship ship && ship.PlayerId == playerId)
                {
                   return; 
                }
                
            }
            GameEvents.DeadPlayer?.Invoke(playerId);
        }

        private bool ValidateAttack(Position start, Position end, int playerId, out Ship ship, out ObjectWithHealth enemyObject)
        {
            ship = null;
            enemyObject = null;
            if (!Highlights.Contains(start) || !Highlights.Contains(end) || start.Equals(end))
            {
                return false;
            }

            bool isObjectAtPosition = TryGetObject(start, out Object obj);
            if (isObjectAtPosition || obj.PlayerId != playerId || obj is not Ship getShip)
            {
                return false;
            }

            ship = getShip;

            bool isEnemyAtPosition = TryGetObject(end, out Object enemy);
            if (isEnemyAtPosition || enemy.PlayerId == playerId || enemy is not ObjectWithHealth killableObject)
            {
                return false;
            }

            enemyObject = killableObject;

            return true;
        }

        public bool TryApply(ActionOnMap action, Position position)
        {
            if (!Highlights.Contains(position))
            {
                return false;
            }

            switch (action)
            {
                default:
                    return false;
            }

            ;

            return true;
        }

        public void Reset(ShipsDatabase shipsDatabase)
        {
            foreach (var o in Objects)
            {
                if (o is Ship ship)
                {
                    int id = ship.Id;
                    ShipData data = shipsDatabase.GetShip(id);
                    
                    ship.CanMove = true;
                    ship.Damage = data.Damage;
                    ship.Range = data.Range;
                    ship.Movement =  data.Movement;
                }
            }
        }
    }
}