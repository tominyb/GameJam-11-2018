using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player I;

    public delegate void HealthChanged(int newHealth);
    public static event HealthChanged OnHealthChanged;

    [SerializeField] private int m_health;
    [SerializeField] private GameObject m_cameraFollowPoint;
    private Rigidbody m_rigidbody;

    public int Health
    {
        get
        {
            return m_health;
        }
    }

    private void Awake()
    {
        I = this;
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var vel = m_rigidbody.velocity;
        Debug.DrawLine(m_rigidbody.position, transform.position + vel.normalized * 2f, Color.red);
        m_cameraFollowPoint.transform.position = m_rigidbody.position + vel.normalized * Mathf.Min(vel.magnitude, 10f);
        Debug.DrawLine(m_rigidbody.position, m_cameraFollowPoint.transform.position, Color.blue);
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;
        m_health = Mathf.Max(0, m_health);
        OnHealthChanged(m_health);

        if (m_health <= 0)
        {
            Debug.Log("Player has died.");
        }
    }
}
