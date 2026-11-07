using System;
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
            Random rnd = new();
            for (int i = 0; i < 5; i++)
            {
                var card = handDeck[rnd.Next(0, handDeck.Count)];
                PayAndDiscard(card);
            }
            EndTurn();
        }
    }
}