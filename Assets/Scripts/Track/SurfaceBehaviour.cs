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

    public void HandleContact(Vector2 pointNormal, Rigidbody2D other)
    {
        if (m_type == Type.Accelerating)
            Accelerate(pointNormal, other);

        if (m_type == Type.Magnetic)
            Pull(pointNormal, other);
    }

    private void Accelerate(Vector2 normal, Rigidbody2D other)
    {
        other.velocity *= (1 + m_forceFactor * Time.fixedDeltaTime);
    }

    private void Pull(Vector2 normal, Rigidbody2D other)
    {
        Vector3 movement = other.velocity;
        var tangential = Vector3.ProjectOnPlane(movement, normal).normalized;
        other.velocity = tangential * movement.magnitude;
    }
}
