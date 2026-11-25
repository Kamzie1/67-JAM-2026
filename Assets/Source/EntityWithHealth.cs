namespace YANG
{
    public class EntityWithHealth : Entity
    {
        public int Health { get; set; }
        protected EntityWithHealth(int id, Position position, int playerId) : base(id, position, playerId)
        {
        }
    }
}