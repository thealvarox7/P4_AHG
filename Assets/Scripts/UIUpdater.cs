using TMPro;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private TMP_Text playerLifeText;
    [SerializeField] private TMP_Text enemyCountText;

    private void Start()
    {
        if (playerHealth == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerHealth = playerObject.GetComponent<Health>();
            }
        }
    }

    private void Update()
    {
        UpdatePlayerLife();
        UpdateEnemyCount();
    }

    private void UpdatePlayerLife()
    {
        if (playerHealth == null || playerLifeText == null)
        {
            return;
        }

        playerLifeText.text = playerHealth.Life.ToString();
    }

    private void UpdateEnemyCount()
    {
        if (enemyCountText == null)
        {
            return;
        }

        enemyCountText.text = GetEnemyCount().ToString();
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