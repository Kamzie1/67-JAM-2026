namespace YANG
{
    public class Base : ObjectWithHealth 
    {
        public Base(int id, Position position, int playerId) : base(id, position, playerId)
        {
            Health = 10;
        }
    }
}