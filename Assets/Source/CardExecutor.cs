using UnityEngine;

namespace YANG
{
    public class CardExecutor : MonoBehaviour
    {
        [SerializeField] private MapUI map;
        [SerializeField] private Game game;
        public CardsDatabase cardsDatabase;

        public void PlayCard(CardData card, int playerId)
        {
            switch (card)
            {
                case CardOnMapData m:
                    map.Apply(m.Action, m.IdWorksOn, playerId);
                    break;

                case CardOnPlayerData p:
                    game.ApplyToPlayer(playerId, p.Action, p.Bonus);
                    break;

                default:
                    Debug.LogError($"Unknown card type: {card?.GetType().Name}");
                    break;
            }
        }

        public void Reset()
        {
            map.Reset();
        }
        
        public void Move()
        {
            Debug.Log("Move!");
            var card = cardsDatabase.GetCard(0);
            if (card == null || card is not CardOnMapData m)
            {
                Debug.LogError($"Card {card} not found");
                return;
            }
            map.Apply(m.Action, m.IdWorksOn, 0);     
        }
        
        public void Attack()
        {
            Debug.Log("Attack!");
            var card = cardsDatabase.GetCard(1);
            if (card == null || card is not CardOnMapData m)
            {
                Debug.LogError($"Card {card} not found");
                return;
            }
            map.Apply(m.Action, m.IdWorksOn, 0);     
        }
    }
}