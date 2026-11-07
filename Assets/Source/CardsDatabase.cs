using System.Collections.Generic;
using UnityEngine;

namespace YANG
{
    [CreateAssetMenu(fileName = "CardsDatabase", menuName = "Scriptable Objects/CardsDatabase")]
    public class CardsDatabase : ScriptableObject
    {
        [SerializeField] private List<CardData> cards = new();
        private Dictionary<int, CardData> lookup;

        public CardData GetCard(int id)
        {
            lookup ??= BuildLookup();
            return lookup.TryGetValue(id, out var card) ? card : null;
        }

        private Dictionary<int, CardData> BuildLookup()
        {
            var dict = new Dictionary<int, CardData>();
            foreach (var c in cards)
            {
                if (!dict.TryAdd(c.Id, c))
                    Debug.LogError($"Duplicate card id {c.Id} ({c.name})", c);
            }
            return dict;
        }

        private void OnValidate() => lookup = null;
    }
}