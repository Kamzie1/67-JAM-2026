namespace YANG
{
    public class Entity
    {
        public int Id{get;}
        public Position Position{get;set;}
        public int PlayerId{get;}

        protected Entity(int id, Position position, int playerId)
        {
            Id = id;
            Position = position;
            PlayerId = playerId;
        }
        
        public virtual string DisplayStats()
        {
            return "";
        }
    }
}