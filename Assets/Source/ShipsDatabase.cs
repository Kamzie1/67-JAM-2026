using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YANG
{
    [CreateAssetMenu(fileName = "ShipsDatabase", menuName = "Scriptable Objects/ShipsDatabase")]
    public class ShipsDatabase : ScriptableObject
    {
        [SerializeField] private readonly List<ShipData> _ships = new();
        private Dictionary<int, ShipData> _lookup;

        public ShipData GetShip(int id)
        {
            _lookup ??= BuildLookup();
            return _lookup.GetValueOrDefault(id);
        }

        private Dictionary<int, ShipData> BuildLookup()
        {
            var dict = new Dictionary<int, ShipData>();
            foreach (var s in _ships.Where(s => !dict.TryAdd(s.Id, s)))
            {
                Debug.LogError($"Duplicate Ship id {s.Id} ({s.name})", s);
            }
            return dict;
        }

        private void OnValidate() => _lookup = null;
    }
}