using UnityEngine;
using System.Collections.Generic;
using YANG;

namespace YANG
{
    [CreateAssetMenu(fileName = "CardOnMapData", menuName = "Scriptable Objects/CardOnMapData")]
    public class CardOnMapData : CardData
    {
        [SerializeField] private List<int> idWorksOn;
        [SerializeField] private ActionOnMap actionOnMap;

        public IReadOnlyList<int> IdWorksOn => idWorksOn;
        public ActionOnMap Action => actionOnMap;
    }
}