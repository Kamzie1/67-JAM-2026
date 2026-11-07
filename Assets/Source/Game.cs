using UnityEngine;
using System.Collections.Generic;

namespace YANG
{
    public class Game : MonoBehaviour
    {
        private List<int> players;

        public void ApplyToPlayer(int playerId, ActionOnPlayer action, int bonus)
        {
            switch (action)
            {
                case ActionOnPlayer.IncreaseRegen:
                    IncreaseRegen(playerId, bonus);
                    break;
            }
        }

        private void IncreaseRegen(int playerId, int amount)
        {
            //TODO
        }
    }
}