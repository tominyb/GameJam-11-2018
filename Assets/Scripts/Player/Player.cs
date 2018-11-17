using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player I;

    public delegate void HealthChanged(int newHealth);
    public event HealthChanged OnHealthChanged;

    public int MaxHealth
    {
        get { return m_maxHealth; }
    }

    [SerializeField] private int m_health;
    [SerializeField] private GameObject m_cameraFollowPoint;

    private Rigidbody m_rigidbody;
    private int m_maxHealth;

    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            OnHealthChanged?.Invoke(m_health);
        }
    }

    private void Awake()
    {
        I = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_maxHealth = m_health;
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
        Health = Mathf.Max(m_health - damage, 0);
        if (m_health <= 0)
        {
            Debug.Log("Player has died.");
        }
    }

    public void ResetForces()
    {
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.rotation = Quaternion.identity;
        m_rigidbody.angularVelocity = Vector3.zero;
    }
}
