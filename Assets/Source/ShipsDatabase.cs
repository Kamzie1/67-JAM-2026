using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YANG
{
    [CreateAssetMenu(fileName = "ShipsDatabase", menuName = "Scriptable Objects/ShipsDatabase")]
    public class ShipsDatabase : ScriptableObject
    {
        [SerializeField] private List<ShipData> ships = new();
        private Dictionary<int, ShipData> _lookup;

        public ShipData GetShip(int id)
        {
            _lookup ??= BuildLookup();
            return _lookup.TryGetValue(id, out var ship) ? ship : null;
        }

        private Dictionary<int, ShipData> BuildLookup()
        {
            var dict = new Dictionary<int, ShipData>();
            foreach (var s in ships)
            {
                if (!dict.TryAdd(s.Id, s))
                    Debug.LogError($"Duplicate card id {s.Id} ({s.name})", s);
            }
            return dict;
        }

        private void OnValidate() => _lookup = null;
    }
}