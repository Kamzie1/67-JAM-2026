namespace YANG
{
    public class Object
    {
        public int Id{get;}
        public Position Position{get;set;}
        
        public Object(int id, Position position)
        {
            Id = id;
            Position = position;
        }
    }
}