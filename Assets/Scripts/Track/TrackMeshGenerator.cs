using System.Linq;
using UnityEngine;

public class TrackMeshGenerator : MonoBehaviour
{
    private const int VerticesPerQuad = 4;
    private const int NormalsPerQuad = VerticesPerQuad;
    private const int TrianglesPerQuad = 2;
    private const int TriangleIndicesPerQuad = TrianglesPerQuad * 3;

    [SerializeField] private float m_trackWidth;
    [SerializeField] private float m_trackThickness;
    [SerializeField] private int m_maxWaypointCount;
    [SerializeField] private Material m_material;
    private MeshFilter m_meshFilter;
    private int m_totalWaypointCount;
    private Vector2? m_previousWaypoint;
    private CuboidBetweenWaypoints? m_previousCuboid;

    struct CuboidBetweenWaypoints
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[] Triangles;

        public CuboidBetweenWaypoints(
            Vector2 start, Vector2 end, float width, float thickness, int triangleOffset,
            CuboidBetweenWaypoints? previousCuboid)
        {
            // TODO: Clean up.
            float halfWidth = width * 0.5f;
            float halfThickness = thickness * 0.5f;
            Vector2 surfaceTangent = (end - start).normalized;
            Vector3 topNormal = Quaternion.Euler(0, 0, 90) * surfaceTangent;
            Vector2 offset = topNormal * halfThickness;
            Vector3 startTopFront, startTopBack, startBottomFront, startBottomBack;
            if (previousCuboid != null)
            {
                Vector3[] vertices = ((CuboidBetweenWaypoints) previousCuboid).Vertices;
                startTopFront = vertices[1];
                startTopBack = vertices[2];
                startBottomFront = vertices[5];
                startBottomBack = vertices[6];
            }
            else
            {
                Vector2 startTop = start + offset;
                Vector2 startBottom = start - offset;
                startTopFront = new Vector3(startTop.x, startTop.y, -halfThickness);
                startTopBack = new Vector3(startTop.x, startTop.y, halfThickness);
                startBottomFront = new Vector3(startBottom.x, startBottom.y, -halfThickness);
                startBottomBack = new Vector3(startBottom.x, startBottom.y, halfThickness);
            }
            Vector2 endTop = end + offset;
            Vector2 endBottom = end - offset;
            Vector3 endTopFront = new Vector3(endTop.x, endTop.y, -halfThickness);
            Vector3 endTopBack = new Vector3(endTop.x, endTop.y, halfThickness);
            Vector3 endBottomFront = new Vector3(endBottom.x, endBottom.y, -halfThickness);
            Vector3 endBottomBack = new Vector3(endBottom.x, endBottom.y, halfThickness);
            Vertices = new Vector3[]
            {
                // Top
                startTopFront,
                endTopFront,
                endTopBack,
                startTopBack,
                // Bottom
                startBottomFront,
                endBottomFront,
                endBottomBack,
                startBottomBack,
                // Front
                startTopFront,
                endTopFront,
                endBottomFront,
                startBottomFront
            };
            {
                Vector3 bottomNormal = Quaternion.Euler(0, 0, 90) * surfaceTangent;
                Vector3 frontNormal = Vector3.back;
                Normals = new Vector3[]
                {
                    topNormal, topNormal, topNormal, topNormal,
                    bottomNormal, bottomNormal, bottomNormal, bottomNormal,
                    frontNormal, frontNormal, frontNormal, frontNormal
                };
            }
            Triangles = new int[]
            {
                // Top
                triangleOffset, triangleOffset + 2, triangleOffset + 1,
                triangleOffset, triangleOffset + 3, triangleOffset + 2,
                // Bottom
                triangleOffset + 4, triangleOffset + 5, triangleOffset + 6,
                triangleOffset + 6, triangleOffset + 7, triangleOffset + 4,
                // Front
                triangleOffset + 8, triangleOffset + 9, triangleOffset + 10,
                triangleOffset + 10, triangleOffset + 11, triangleOffset + 8
            };
        }
    }

    private void Start()
    {
        InitMeshRenderer();
        InitMeshFilter();
    }

    private void InitMeshRenderer()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = m_material;
    }

    private void InitMeshFilter()
    {
        m_meshFilter = gameObject.AddComponent<MeshFilter>();
        m_meshFilter.mesh = new Mesh();
    }

    public void AddWaypoint(Vector2 waypoint)
    {
        ++m_totalWaypointCount;
        if (m_totalWaypointCount >= m_maxWaypointCount)
        {
            Debug.LogWarning("Max waypoint count reached. Discarding new waypoint.");
            return;
        }
        if (m_previousWaypoint != null)
        {
            Mesh previousMesh = m_meshFilter.sharedMesh;
            Mesh mesh = new Mesh();
            CuboidBetweenWaypoints cuboid = new CuboidBetweenWaypoints(
                (Vector2) m_previousWaypoint, waypoint,
                m_trackWidth, m_trackThickness,
                previousMesh.vertices.Length,
                m_previousCuboid);
            mesh.vertices = previousMesh.vertices.Concat(cuboid.Vertices).ToArray();
            mesh.normals = previousMesh.normals.Concat(cuboid.Normals).ToArray();
            mesh.triangles = previousMesh.triangles.Concat(cuboid.Triangles).ToArray();
            m_meshFilter.mesh = mesh;
            m_previousCuboid = cuboid;
        }
        m_previousWaypoint = waypoint;
    }

    public void ClearPreviousWaypoint()
    {
        m_previousWaypoint = null;
        m_previousCuboid = null;
    }
}
