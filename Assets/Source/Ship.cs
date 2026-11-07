using UnityEngine.UI;

namespace YANG
{
    public class Ship : Object
    {
        public int Movement{get;set;}
        public int Range{get;set;}
        public int Attack{get;set;}
        public int Health{get;set;}
        public bool CanMove{get;set;}
        public Ship(int id, Position position) : base(id, position)
        {
            CanMove = true;
        }
    }
}