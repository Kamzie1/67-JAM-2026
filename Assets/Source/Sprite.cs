using UnityEngine;

namespace YANG
{
    public class Sprite : MonoBehaviour
    {
        private Object _object;
        
        public int Id => _object.Id;

        public Sprite(Object obj)
        {
            _object = obj;
        }

        public static Sprite Create(Object obj, GameObject gameObject=null)
        {
            GameObject o = gameObject;
            if (o == null)
            {
                o = new GameObject(nameof(Object));
            }
            Sprite sprite = o.AddComponent<Sprite>();
            sprite._object = obj;
            return sprite;
        }
    }
}