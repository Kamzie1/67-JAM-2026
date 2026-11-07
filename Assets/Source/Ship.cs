using UnityEngine.UI;

namespace YANG
{
    public class Ship : ObjectWithHealth
    {
        public int ShipId { get; }
        public int Movement{get;set;}
        public int Range{get;set;}
        public int Damage{get;set;}
        public bool CanMove{get;set;}
        public Ship(int id, Position position, int playerId, int shipId) : base(id, position,  playerId)
        {
            ShipId = shipId;
            CanMove = true;
        }
        
        public Ship(int id, Position position, int playerId, int shipId, int health, int damage, int movement, int range) : base(id, position,  playerId)
        {
            Health = health;
            Damage = damage;
            Movement = movement;
            Range = range;
            ShipId = shipId;
            CanMove = true;
        }
    }
}