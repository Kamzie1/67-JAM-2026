using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YANG
{
    public class TurnManager : MonoBehaviour
    {
        public event System.Action<int> TurnStarted;
        public event System.Action<int> TurnEnded;
        
        [SerializeField] private string EndGameSceneName="EndGame";
        [SerializeField] private int playerCount = 2;
        public int CurrentPlayer { get; private set; }
        public TurnPhase Phase { get; private set; }

        private void OnEnable()
        {
            GameEvents.DeadPlayer += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            GameEvents.DeadPlayer -= HandlePlayerDeath;
        } 

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

        private void HandlePlayerDeath(int playerId)
        {
            Debug.Log($"Player {playerId} has been death");
            SceneManager.LoadScene(EndGameSceneName);
            if (playerId == 0)
            {
               GameData.EndgameMessage = "Przegrałeś!";
            }
            else
            {
                GameData.EndgameMessage = "Wygrałeś!";
            }
        }
    }
}