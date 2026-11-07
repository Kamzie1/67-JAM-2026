using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string quickGameSceneName = "QuickGame";

    public void QuickGame()
    {
        SceneManager.LoadScene(quickGameSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
