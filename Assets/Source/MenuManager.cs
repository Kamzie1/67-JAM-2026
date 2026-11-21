using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YANG
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private string quickGameSceneName = "QuickGame";
        [SerializeField] private AudioClip clickClip;

        public void QuickGame()
        {
            AudioManager.Instance.PlaySound(clickClip, transform, 1f);
            SceneManager.LoadScene(quickGameSceneName);
        }

        public void Exit()
        {
            AudioManager.Instance.PlaySound(clickClip, transform, 1f);
            Application.Quit();
        }
    }
}
