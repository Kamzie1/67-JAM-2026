using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Analytics;

namespace YANG
{
    public class Map
    {
        public List<Position> Highlights { get; private set; } 
        public List<Entity> Entities { get; }
        private readonly Board _board;

        public int Rows => _board.Rows;
        public int Cols => _board.Cols;

        public Map(int rows, int cols)
        {
            _board = new Board(rows, cols);
            Entities = new List<Entity>();
            Highlights = new List<Position>();
        }
        

        public void Spawn(Entity o)
        {
            Entities.Add(o);
        }

        public bool TryGetObject(Position position, out Entity obj)
        {
            obj = null;
            foreach (var o in Entities.Where(o => o.Position.Equals(position)))
            {
                obj = o;
                return true;
            }

            return false;
        }

        public List<Position> AttackablePositions(Position start)
        {
            List<Position> positions = new List<Position>();
            bool isObjectAtPosition = TryGetObject(start, out Entity obj);
            if (!isObjectAtPosition || obj is not Ship ship)
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
                    if (!n.IsValid(Rows, Cols) || visited[n.X][n.Y] || points < 0)
                    {
                        continue;
                    }

                    visited[n.X][n.Y] = true;
                    queue.Enqueue(new ValueTuple<Position, int>(n, points));
                    foreach (var enemy in Entities)
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
            bool isObjectAtPosition = TryGetObject(start, out Entity obj);
            if (!isObjectAtPosition || obj is not Ship ship)
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
                    if (!n.IsValid(Rows, Cols) || visited[n.X][n.Y] || points < 0)
                    {
                        continue;
                    }

                    visited[n.X][n.Y] = true;
                    bool anyObject = false;
                    bool enemy = false;
                    foreach (var o in Entities)
                    {
                        if (o.Position.Equals(n))
                        {
                            anyObject = true;
                            if (o.PlayerId != ship.PlayerId)
                            {
                                enemy = true;
                            }
                            break;
                        }
                    }

                    if (!enemy)
                    {
                        queue.Enqueue(new ValueTuple<Position, int>(n, points));
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

        public List<Position> AppliablePositions(ActionOnMap action,IReadOnlyList<int> ids, int playerId)
        {
            List<Position> positions = new List<Position>();
            foreach (var o in Entities)
            {
                if (o is Ship ship && ids.Contains(ship.ShipId))
                {
                    if (o.PlayerId != playerId)
                    {
                        continue;
                    }

                    if (action == ActionOnMap.Move && !ship.CanMove)
                    {
                        continue;
                    }
                    positions.Add(ship.Position);
                }
            }

            Highlights = positions;
            return Highlights;
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

            bool isObjectAtPosition = TryGetObject(start, out Entity obj);
            if (!isObjectAtPosition || obj is not Ship getShip)
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


        public int TryAttackKill(Position start, Position end, int playerId)
        {
            if (!ValidateAttack(start, end, playerId, out var ship, out var enemy)) return 0;
            enemy.Health -= ship.Damage;
            if (enemy.Health <= 0)
            {
                if (enemy is Base)
                {
                    GameEvents.PlayerDeath?.Invoke(enemy.PlayerId);
                }

                return 2;
            }


            return 1;
        }

        public void CheckPlayerDeath(int playerId)
        {
            foreach (var obj in Entities )
            {
                if (obj is Ship ship && ship.PlayerId == playerId)
                {
                    return; 
                }
                
            }
            GameEvents.PlayerDeath?.Invoke(playerId);
        }

        private bool ValidateAttack(Position start, Position end, int playerId, out Ship ship, out EntityWithHealth enemyEntity)
        {
            ship = null;
            enemyEntity = null;
            if (!Highlights.Contains(start) || !Highlights.Contains(end) || start.Equals(end))
            {
                return false;
            }

            bool isObjectAtPosition = TryGetObject(start, out Entity obj);
            if (!isObjectAtPosition || obj.PlayerId != playerId || obj is not Ship getShip)
            {
                return false;
            }

            ship = getShip;

            bool isEnemyAtPosition = TryGetObject(end, out Entity enemy);
            if (!isEnemyAtPosition || enemy.PlayerId == playerId || enemy is not EntityWithHealth killableObject)
            {
                return false;
            }

            enemyEntity = killableObject;

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

            return true;
        }

        public void Reset(ShipsDatabase shipsDatabase)
        {
            foreach (var o in Entities)
            {
                if (o is Ship ship)
                {
                    int id = ship.ShipId;
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