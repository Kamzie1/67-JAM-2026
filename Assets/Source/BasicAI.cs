using System.Collections.Generic;

namespace YANG
{
    public class BasicAI : Player
    {
        public BasicAI(IEnumerable<CardData> deck, int maxActionPoints, int actionPointsRegen) : base(deck, maxActionPoints, actionPointsRegen)
        {
        }

        public override void StartTurn(int cardsToDraw)
        {
            base.StartTurn(cardsToDraw);
            DoTurn();
        }

        public void DoTurn()
        {
            foreach (var card in handDeck)
            {
                PayAndDiscard(card);
            }
            EndTurn();
        }
    }
}