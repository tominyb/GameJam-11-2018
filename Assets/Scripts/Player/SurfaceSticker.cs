using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    private RaycastHit2D m_contactPoint;
    private Rigidbody2D m_rigidbody;
    private SurfaceBehaviour m_surface;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_surface = ScanForSurfaces(out m_contactPoint);

        if (m_surface == null)
            return;

        m_surface.HandleContact(m_contactPoint.normal, m_rigidbody);

        Debug.DrawLine(
            transform.position,
            transform.position + (Vector3)m_rigidbody.velocity.normalized * 2f,
            Color.red
        );
    }

    private SurfaceBehaviour ScanForSurfaces(out RaycastHit2D point)
    {
        SurfaceBehaviour closest = null;
        var closestDistance = float.MaxValue;
        point = new RaycastHit2D();

        foreach (var dir in directions)
        {
            var hit = Physics2D.Raycast(transform.position, dir, m_raycastDistance, 1 << LayerMask.NameToLayer("Surface"));
            if (!hit)
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
