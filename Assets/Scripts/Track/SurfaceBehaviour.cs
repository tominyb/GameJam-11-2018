using UnityEngine;

public class SurfaceBehaviour : MonoBehaviour
{
    public enum Type
    {
        Accelerating,
        Magnetic,
    }

    [SerializeField] float m_forceFactor;
    [SerializeField] Type  m_type;

    private void HandleContact(Rigidbody2D other)
    {
        switch (m_type)
        {
            case (Type.Accelerating):
                Accelerate(other);
                break;
            case (Type.Magnetic):
                Pull(other);
                break;
        }
    }

    private void Accelerate(Rigidbody2D other)
    {
        other.velocity *= m_forceFactor;
    }

    private void Pull(Rigidbody2D other)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.rigidbody)
            return;

        HandleContact(collision.rigidbody);
    }
}
