using UnityEngine;

public class SurfaceBehaviour : MonoBehaviour
{
    [System.Flags]
    public enum Type
    {
        Accelerating,
        Decelerating,
        Magnetic,
    }
    
    [SerializeField] float m_forceFactor;
    [SerializeField] Type  m_type;

    private void HandleContact(float step, Rigidbody2D other)
    {

    }
}
