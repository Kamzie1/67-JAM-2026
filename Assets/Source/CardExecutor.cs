using UnityEngine;

namespace YANG
{
    public class CardExecutor : MonoBehaviour
    {
        [SerializeField] private MapUI map;
        [SerializeField] private Game game;

        public void PlayCard(CardData card, int playerId)
        {
            switch (card)
            {
                case CardOnMapData m:
                    map.Apply(m.Action);
                    break;

                case CardOnPlayerData p:
                    game.ApplyToPlayer(playerId, p.Action, p.Bonus);
                    break;

                default:
                    Debug.LogError($"Unknown card type: {card?.GetType().Name}");
                    break;
            }
        }
    }
}