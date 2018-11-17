using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player I;

    [SerializeField] private int m_health;
    [SerializeField] private GameObject m_cameraFollowPoint;
    private Rigidbody m_rigidbody;

    private void Awake()
    {
        I = this;
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var vel = m_rigidbody.velocity;
        Debug.DrawLine(m_rigidbody.position, transform.position + vel.normalized * 2f, Color.red);
        //vel.x = -vel.x;
        m_cameraFollowPoint.transform.position = m_rigidbody.position + vel.normalized * Mathf.Min(vel.magnitude, 10f);
            /*Vector2.Lerp(
            m_cameraFollowPoint.transform.localPosition,
            vel.normalized * Mathf.Min(vel.magnitude, 4f),
            Time.fixedDeltaTime
        );*/
        Debug.DrawLine(m_rigidbody.position, m_cameraFollowPoint.transform.position, Color.blue);
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
