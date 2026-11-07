namespace YANG
{
    public class ObjectWithHealth : Object
    {
        public int Health { get; set; }
        protected ObjectWithHealth(int id, Position position, int playerId) : base(id, position, playerId)
        {
        }
    }
}