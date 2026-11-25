using UnityEngine;

namespace YANG
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Entity _entity;
        public int Id => _entity.Id;

        public void Initialize(Sprite unitSprite, Vector3 worldPosition, Entity entity)
        {
            spriteRenderer.sprite = unitSprite;
            transform.position = worldPosition;
            _entity = entity;
        }

        public void MoveTo(Vector3 newWorldPosition)
        {
            transform.position = newWorldPosition;
        }
       
    }
}