namespace YANG
{
    public class Object
    {
        public int Id{get;}
        public Position Position{get;set;}
        public int PlayerId{get;}

        protected Object(int id, Position position, int playerId)
        {
            Id = id;
            Position = position;
            PlayerId = playerId;
        }
    }
}