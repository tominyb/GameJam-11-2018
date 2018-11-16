using System.Linq;
using UnityEngine;

public class TrackMeshGenerator : MonoBehaviour
{
    private const int VerticesPerQuad = 4;
    private const int NormalsPerQuad = VerticesPerQuad;
    private const int TrianglesPerQuad = 2;
    private const int TriangleIndicesPerQuad = TrianglesPerQuad * 3;

    [SerializeField] private float m_trackWidth;
    [SerializeField] private int m_maxWaypointCount;
    [SerializeField] private Material m_material;
    private MeshFilter m_meshFilter;
    private int m_totalWaypointCount;
    private Vector2? m_previousWaypoint;

    struct QuadBetweenWaypoints
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[] Triangles;

        public QuadBetweenWaypoints(Vector2 start, Vector2 end, float width, int triangleOffset)
        {
            float halfWidth = width * 0.5f;
            Vertices = new Vector3[]
            {
                new Vector3(start.x, start.y, -halfWidth),
                new Vector3(end.x, end.y, -halfWidth),
                new Vector3(end.x, end.y, halfWidth),
                new Vector3(start.x, start.y, halfWidth)
            };
            {
                Vector3 normal = Quaternion.Euler(0, 0, 90) * (end - start);
                Normals = new Vector3[NormalsPerQuad];
                for (int i = 0; i < NormalsPerQuad; ++i)
                {
                    Normals[i] = normal;
                }
            }
            Triangles = new int[]
            {
                triangleOffset, triangleOffset + 2, triangleOffset + 1,
                triangleOffset, triangleOffset + 3, triangleOffset + 2
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
            QuadBetweenWaypoints quad = new QuadBetweenWaypoints(
                (Vector2) m_previousWaypoint, waypoint,
                m_trackWidth, previousMesh.vertices.Length);
            mesh.vertices = previousMesh.vertices.Concat(quad.Vertices).ToArray();
            mesh.normals = previousMesh.normals.Concat(quad.Normals).ToArray();
            mesh.triangles = previousMesh.triangles.Concat(quad.Triangles).ToArray();
            m_meshFilter.mesh = mesh;
        }
        m_previousWaypoint = waypoint;
    }

    public void ClearPreviousWaypoint()
    {
        m_previousWaypoint = null;
    }
}
