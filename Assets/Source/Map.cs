using UnityEngine;

namespace YANG
{
    public class Map : MonoBehaviour
    {
        Vector2Int TargetCoords1, TargetCoords2;

        public void Apply(ActionOnMap action)
        {
            switch (action)
            {
                case ActionOnMap.Move:
                    Move(TargetCoords1, TargetCoords2);
                    break;
            }
        }

        /////FUNTIONS ON MAP
        private void Move(Vector2Int startCoords, Vector2Int endCoords)
        {
            //TOOD
        }
    }
}