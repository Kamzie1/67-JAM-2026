using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YANG
{
    public class EndGameManager : MonoBehaviour
    {
        [SerializeField] private SceneAsset MainMenu;
        [SerializeField] private TMP_Text Result;

        private void OnEnable()
        {
            GameEvents.Winner += HandleWinner;
        }

        private void OnDisable()
        {
            GameEvents.Winner -= HandleWinner;
        }

        private void HandleWinner(int player)
        {
            Result.text = $"Gracz {player} wygrał!";
        }
        
        public void Restart()
        {
            SceneManager.LoadScene(MainMenu.name);
        }
    }
}