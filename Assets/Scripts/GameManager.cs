using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string victorySceneName = "Victory";

    private bool hasWon;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != gameSceneName)
        {
            return;
        }

        if (hasWon)
        {
            return;
        }

        if (GetEnemyCount() > 0)
        {
            return;
        }

        hasWon = true;
        SceneManager.LoadScene(victorySceneName);
    }

    private int GetEnemyCount()
    {
        try
        {
            return GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        catch (UnityException)
        {
            return 0;
        }
    }
}