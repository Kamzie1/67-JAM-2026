using UnityEngine;

namespace YANG
{
    [CreateAssetMenu(fileName = "ShipsData", menuName = "Scriptable Objects/ShipsData")]
    public class ShipData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string shipName;
        [SerializeField] private string description;
        [SerializeField] private int movement;
        [SerializeField] private int range;
        [SerializeField] private int damage;
        [SerializeField] private int health;
        [SerializeField] private int[] cardIds;

        public int Id => id;
        public string ShipName => shipName;
        public string Description => description;
        public int Movement => movement;
        public int Range => range;
        public int Damage => damage;
        public int Health => health;
        public int[] CardIds => cardIds;
    }
}