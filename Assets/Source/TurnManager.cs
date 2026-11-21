using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace YANG
{
    public class TurnManager : MonoBehaviour
    {
        public event System.Action<int> TurnStarted;
        public event System.Action<int> TurnEnded;
        
        [SerializeField] private string EndGameSceneName="EndGame";
        [SerializeField] private int playerCount = 2;
        [SerializeField] private AudioClip clickClip;
        [SerializeField] private TMP_Text currentPlayerBaner;
        [SerializeField] private TMP_Text TurnBanner;
        public int CurrentPlayer { get; private set; }
        public int Turn { get; private set; }
        public TurnPhase Phase { get; private set; }

        private void OnEnable()
        {
            GameEvents.PlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            GameEvents.PlayerDeath -= HandlePlayerDeath;
        } 

        public void StartGame()
        {
            CurrentPlayer = 0;
            Turn = 0;
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
            AudioManager.Instance.PlaySound(clickClip, transform, 1f);
            Phase = TurnPhase.EndTurn;
            TurnEnded?.Invoke(CurrentPlayer);

            CurrentPlayer = (CurrentPlayer + 1) % playerCount;
            Turn += 1;
            currentPlayerBaner.text = $"Player {CurrentPlayer}";
            TurnBanner.text = $"Turn {Turn}";
            BeginTurn();
        }

        private void HandlePlayerDeath(int playerId)
        {
            SceneManager.LoadScene(EndGameSceneName);
            GameData.EndgameMessage = $"PLAYER {playerCount - 1 -  playerId} WON";
        }
    }
}