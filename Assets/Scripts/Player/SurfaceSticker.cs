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

    public bool IsSticking { get { return m_surface != null; } }

    [SerializeField] private float m_distance;

    private int m_groundMask;
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

        m_surface.Pull(m_contactPoint.normal, m_rigidbody);
    }

    private SurfaceBehaviour ScanForSurfaces(out RaycastHit2D point)
    {
        foreach (var dir in directions)
        {
            var hit = Physics2D.Raycast(transform.position, dir, m_distance, 1 << LayerMask.NameToLayer("Surface"));
            if (!hit)
                continue;

            var surface = hit.collider.GetComponent<SurfaceBehaviour>();
            if (surface != null && surface.SurfaceType == SurfaceBehaviour.Type.Magnetic)
            {
                point = hit;
                return surface;
            }
        }

        point = new RaycastHit2D();
        return null;
    }
}
