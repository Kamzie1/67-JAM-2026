using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneAsset quickGameScene;

    public void QuickGame()
    {
        SceneManager.LoadScene(quickGameScene.name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
