using UnityEngine;

namespace YANG
{
    public class ObjectUI : MonoBehaviour
        {
        private Object _object;
        
        public int Id => _object.Id;

        public ObjectUI(Object obj)
        {
            _object = obj;
        }

        public static ObjectUI Create(Object obj, GameObject gameObject=null)
        {
            GameObject _gameObject = gameObject;
            if (_gameObject == null)
            {
                _gameObject = new GameObject(nameof(Object));
            }
            ObjectUI objUi = _gameObject.AddComponent<ObjectUI>();
            objUi._object = obj;
            return objUi;
        }
    }
}