using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player I;

    [SerializeField] private int m_health;

    private void Awake()
    {
        I = this;
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;

        if (m_health <= 0)
        {
            Debug.Log("Player has died.");
        }
    }
}
