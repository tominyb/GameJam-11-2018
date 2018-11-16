using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SurfaceBehaviour : MonoBehaviour
{
    public enum Type
    {
        Accelerating,
        Magnetic,
    }

    public Type SurfaceType { get { return m_type; } }

    [SerializeField] private float m_forceFactor;
    [SerializeField] private Type  m_type;

    private void Awake()
    {
    }

    private void HandleContact(ContactPoint2D contactPoint, Rigidbody2D other)
    {
        switch (m_type)
        {
            case (Type.Accelerating):
                Accelerate(other);
                break;
            case (Type.Magnetic):
                Pull(contactPoint.normal, other);
                break;
        }
    }

    private void Accelerate(Rigidbody2D other)
    {
        other.velocity *= m_forceFactor;
    }

    public void Pull(Vector2 normal, Rigidbody2D other)
    {
        other.AddForce(-normal * m_forceFactor);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.rigidbody)
            return;

        HandleContact(collision.GetContact(0), collision.rigidbody);
    }
}
