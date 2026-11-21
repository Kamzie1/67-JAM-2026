using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YANG
{
    public class EndGameManager : MonoBehaviour
    {
        [SerializeField] private string endGameSceneName = "MainMenu";
        [SerializeField] private TMP_Text Result;
        [SerializeField] private AudioClip clickClip;
        

        private void Awake()
        {
            Result.text = GameData.EndgameMessage;
        }


        private void HandleWinner(string text)
        {
            Debug.Log(text);
            Result.text = text;
        }
        
        public void Restart()
        {
            AudioManager.Instance.PlaySound(clickClip, transform, 1f);
            SceneManager.LoadScene(endGameSceneName);
        }
    }
}