using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SurfaceSticker : MonoBehaviour
{
    private readonly Vector2[] directions = new Vector2[]
    {
        Vector2.down + Vector2.right,
        Vector2.down,
        Vector2.down + Vector2.left,
        Vector2.left,
        Vector2.up + Vector2.left,
        Vector2.up,
        Vector2.up + Vector2.right,
        Vector2.right,
    };

    [SerializeField] private float m_raycastDistance;
    [SerializeField] private float m_minVelocity;
    [SerializeField] private float m_maxVelocity;

    private RaycastHit m_contactPoint;
    private Rigidbody m_rigidbody;
    private SurfaceBehaviour m_surface;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_surface = ScanForSurfaces(out m_contactPoint);

        if (m_surface)
        {
            m_surface.HandleContact(m_contactPoint.normal, m_rigidbody);
        }
        else
        {
            Physics.gravity = Vector3.down * Physics.gravity.magnitude;
        }

        m_rigidbody.velocity = m_rigidbody.velocity.normalized * 
            Mathf.Max(Mathf.Min(m_rigidbody.velocity.magnitude, m_maxVelocity), m_minVelocity);
    }

    private SurfaceBehaviour ScanForSurfaces(out RaycastHit point)
    {
        SurfaceBehaviour closest = null;
        var closestDistance = float.MaxValue;
        point = new RaycastHit();

        foreach (var dir in directions)
        {
            var ray = new Ray(transform.position, dir);
            RaycastHit hit;
            var result = Physics.Raycast(ray, out hit, m_raycastDistance, 1 << LayerMask.NameToLayer("Surface"));
            if (!result)
                continue;

            var surface = hit.collider.GetComponent<SurfaceBehaviour>();
            if (surface == null)
                continue;

            if (closest == null || hit.distance < closestDistance)
            {
                point = hit;
                closest = surface;
                closestDistance = hit.distance;
            }
        }

        return closest;
    }
}
