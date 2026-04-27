using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int life = 3;

    public int Life => life;
    public bool IsDead => life <= 0;

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        life -= amount;

        if (life < 0)
        {
            life = 0;
        }
    }
}