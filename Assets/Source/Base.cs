namespace YANG
{
    public class Base : ObjectWithHealth 
    {
        protected Base(int id, Position position, int playerId) : base(id, position, playerId)
        {
            Health = 100;
        }
    }
}