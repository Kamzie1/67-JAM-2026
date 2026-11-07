using System;
using System.Collections.Generic;

namespace YANG
{
    public class Player
    {
        protected readonly List<CardData> originalDeck;
        protected readonly List<CardData> currentDeck;
        protected readonly List<CardData> handDeck = new();
        protected readonly List<CardData> discardedDeck = new();
        private readonly Random rng = new();
        public int handSize { get; }

        public int MaxActionPoints { get; private set; }
        public int CurrentActionPoints { get; private set; }
        public int ActionPointsRegen { get; private set; }

        public IReadOnlyList<CardData> Hand => handDeck;
        public int DeckCount => currentDeck.Count;
        public int DiscardCount => discardedDeck.Count;

        public event Action Changed;

        public Player(IEnumerable<CardData> deck, int maxActionPoints, int actionPointsRegen)
        {
            originalDeck = new List<CardData>(deck);
            currentDeck = new List<CardData>(originalDeck);
            Shuffle(currentDeck);

            MaxActionPoints = maxActionPoints;
            ActionPointsRegen = actionPointsRegen;
            CurrentActionPoints = 0;
            handSize = 5;
        }

        public virtual void StartTurn(int cardsToDraw)
        {
            CurrentActionPoints = Math.Min(MaxActionPoints, CurrentActionPoints + ActionPointsRegen);
            DrawCard(cardsToDraw);
            Changed?.Invoke();
        }

        public void EndTurn()
        {
            Changed?.Invoke();
        }

        public bool CanPlay(CardData card)
        {
            return card != null
                && handDeck.Contains(card)
                && card.Cost <= CurrentActionPoints;
        }

        public bool PayAndDiscard(CardData card)
        {
            if (!CanPlay(card)) return false;

            CurrentActionPoints -= card.Cost;
            handDeck.Remove(card);
            discardedDeck.Add(card);
            Changed?.Invoke();
            return true;
        }

        public void DrawCard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (handDeck.Count == handSize)
                    return;
                
                if (currentDeck.Count == 0)
                {
                    currentDeck.AddRange(discardedDeck);
                    discardedDeck.Clear();
                    Shuffle(currentDeck);
                }

                int last = currentDeck.Count - 1;
                if (last < 0)
                {
                    return;
                }
                handDeck.Add(currentDeck[last]);
                currentDeck.RemoveAt(last);
            }
            Changed?.Invoke();
        }

        public void AddRegen(int amount)
        {
            ActionPointsRegen += amount;
            Changed?.Invoke();
        }

        public void AddMaxActionPoints(int amount)
        {
            MaxActionPoints += amount;
            Changed?.Invoke();
        }

        private void Shuffle(List<CardData> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}