using UnityEngine;
using System.Collections.Generic;
using YANG;

namespace YANG
{
    public abstract class CardData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string cardName;
        [SerializeField] private string description;
        [SerializeField] private int cost;

        public int Id => id;
        public string CardName => cardName;
        public string Description => description;
        public int Cost => cost;
    }
}