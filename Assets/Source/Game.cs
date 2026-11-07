using System;
using System.Collections.Generic;
using UnityEngine;

namespace YANG
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CardExecutor executor;

        [SerializeField] private int playerCount = 2;
        [SerializeField] private int maxActionPoints = 1;
        [SerializeField] private int startingRegen = 2;
        [SerializeField] private int cardsToDraw = 1;
        [SerializeField] private bool autoEndTurnAtZeroPoints = false;
        [SerializeField] private List<CardData> startingDeck = new();

        private readonly List<Player> players = new();

        public IReadOnlyList<Player> Players => players;
        public int CurrentPlayerId { get; private set; }
        public TurnPhase Phase { get; private set; } = TurnPhase.NotStarted;
        public Player CurrentPlayer => players[CurrentPlayerId];

        public event Action<int> TurnStarted;
        public event Action<int> TurnEnded;
        public event Action<int, CardData> CardPlayed;

        private void Start()
        {
            players.Clear();
            for (int i = 0; i < playerCount; i++)
                players.Add(new Player(startingDeck, maxActionPoints, startingRegen));

            CurrentPlayerId = 0;
            BeginTurn();
        }

        private void BeginTurn()
        {
            CurrentPlayer.StartTurn(handSize);
            Phase = TurnPhase.PlayerAction;
            TurnStarted?.Invoke(CurrentPlayerId);
        }

        public void EndTurn()
        {
            if (Phase != TurnPhase.PlayerAction) return;

            CurrentPlayer.EndTurn();
            TurnEnded?.Invoke(CurrentPlayerId);

            CurrentPlayerId = (CurrentPlayerId + 1) % players.Count;
            BeginTurn();
        }

        public bool TryPlayCard(int playerId, CardData card)
        {
            if (Phase != TurnPhase.PlayerAction) return false;
            if (playerId != CurrentPlayerId) return false;

            var player = players[playerId];
            if (!player.PayAndDiscard(card)) return false;

            Phase = TurnPhase.Resolving;
            try
            {
                executor.PlayCard(card, playerId);
            }
            finally
            {
                Phase = TurnPhase.PlayerAction;
            }

            CardPlayed?.Invoke(playerId, card);

            if (autoEndTurnAtZeroPoints && player.CurrentActionPoints <= 0)
                EndTurn();

            return true;
        }

        public void ApplyToPlayer(int playerId, ActionOnPlayer action, int bonus)
        {
            if (playerId < 0 || playerId >= players.Count)
            {
                Debug.LogError($"Invalid player id: {playerId}");
                return;
            }

            switch (action)
            {
                case ActionOnPlayer.IncreaseRegen:
                    players[playerId].AddRegen(bonus);
                    break;
            }
        }
    }
}