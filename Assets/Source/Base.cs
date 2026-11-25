namespace YANG
{
    public class Base : EntityWithHealth 
    {
        public Base(int id, Position position, int playerId) : base(id, position, playerId)
        {
            Health = 10;
        }

        public override string DisplayStats()
        {
            return $"HEALTH: {Health}";
        }
    }
}