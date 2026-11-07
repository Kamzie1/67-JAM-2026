using UnityEngine;

namespace YANG
{
    public class TurnManager : MonoBehaviour
    {
        public event System.Action<int> TurnStarted;
        public event System.Action<int> TurnEnded;

        [SerializeField] private int playerCount = 2;
        public int CurrentPlayer { get; private set; }
        public TurnPhase Phase { get; private set; }

        public void StartGame()
        {
            CurrentPlayer = 0;
            BeginTurn();
        }

        private void BeginTurn()
        {
            Phase = TurnPhase.StartTurn;
            TurnStarted?.Invoke(CurrentPlayer);
            Phase = TurnPhase.PlayerAction;
        }

        public void EndTurn()
        {
            if (Phase != TurnPhase.PlayerAction) return;
            Phase = TurnPhase.EndTurn;
            TurnEnded?.Invoke(CurrentPlayer);

            CurrentPlayer = (CurrentPlayer + 1) % playerCount;
            BeginTurn();
        }
    }
}