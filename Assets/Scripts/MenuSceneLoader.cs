using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneLoader : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}