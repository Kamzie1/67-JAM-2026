using UnityEngine;
using System.Collections.Generic;
using YANG;

namespace YANG
{
    [CreateAssetMenu(fileName = "CardOnPlayerData", menuName = "Scriptable Objects/CardOnPlayerData")]
    public class CardOnPlayerData : CardData
    {
        [SerializeField] private int bonus;
        [SerializeField] private ActionOnPlayer actionOnPlayer;

        public int Bonus => bonus;
        public ActionOnPlayer Action => actionOnPlayer;
    }
}