using System.Linq;
using UnityEngine;

public class TrackMeshGenerator : MonoBehaviour
{
    [SerializeField] private float m_trackWidth;
    [SerializeField] private float m_trackThickness;
    [SerializeField] private int m_maxWaypointCount;
    [SerializeField] private Material m_material;
    [SerializeField] private float m_uvXPerUnit;

    private MeshFilter m_meshFilter;
    private MeshCollider m_meshCollider;
    private int m_totalWaypointCount;
    private Vector2? m_previousWaypoint;
    private Vector3[] m_previousCuboidVertices;
    private float m_uvStartX;

    struct MeshData
    {
        public Vector3[] Vertices;
        public Vector2[] UV;
        public Vector3[] Normals;
        public int[] Triangles;
    }

    private void Start()
    {
        InitLayerMask();
        InitMeshRenderer();
        InitMeshFilter();
        InitMeshCollider();
    }

    public void ResetMesh()
    {
    }

    private void InitLayerMask()
    {
        gameObject.layer = LayerMask.NameToLayer("Surface");
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

    private void InitMeshCollider()
    {
        m_meshCollider = GetComponent<MeshCollider>();
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
            Vector2 previousWaypoint = (Vector2) m_previousWaypoint;
            if (m_previousCuboidVertices == null)
            {
                AddMeshData(GetLeadingFace(previousWaypoint, waypoint));
            }
            MeshData cuboid = GetCuboidBetweenWaypoints(previousWaypoint, waypoint);
            AddMeshData(cuboid);
            m_previousCuboidVertices = cuboid.Vertices;
        }
        m_previousWaypoint = waypoint;
    }

    private void AddMeshData(MeshData meshData)
    {
        Mesh previousMesh = m_meshFilter.sharedMesh;
        Mesh mesh = new Mesh
        {
            vertices = previousMesh.vertices.Concat(meshData.Vertices).ToArray(),
            uv = previousMesh.uv.Concat(meshData.UV).ToArray(),
            normals = previousMesh.normals.Concat(meshData.Normals).ToArray(),
            triangles = previousMesh.triangles.Concat(meshData.Triangles).ToArray()
        };
        m_meshFilter.sharedMesh = mesh;
        m_meshCollider.sharedMesh = mesh;
    }

    private MeshData GetCuboidBetweenWaypoints(Vector2 start, Vector2 end)
    {
        float halfWidth = m_trackWidth * 0.5f;
        float halfThickness = m_trackThickness * 0.5f;
        Vector2 direction = end - start;
        float magnitude = direction.magnitude;
        float uvEndX = m_uvStartX + (magnitude * m_uvXPerUnit);
        Vector2 surfaceTangent = direction / magnitude;
        Vector3 topNormal = Quaternion.Euler(0, 0, 90) * surfaceTangent;
        Vector2 offset = topNormal * halfThickness;
        Vector3 startTopFront, startTopBack, startBottomFront, startBottomBack;
        if (m_previousCuboidVertices != null)
        {
            startTopFront = m_previousCuboidVertices[1];
            startTopBack = m_previousCuboidVertices[2];
            startBottomFront = m_previousCuboidVertices[5];
            startBottomBack = m_previousCuboidVertices[6];
        }
        else
        {
            Vector2 startTop = start + offset;
            Vector2 startBottom = start - offset;
            startTopFront = new Vector3(startTop.x, startTop.y, -halfWidth);
            startTopBack = new Vector3(startTop.x, startTop.y, halfWidth);
            startBottomFront = new Vector3(startBottom.x, startBottom.y, -halfWidth);
            startBottomBack = new Vector3(startBottom.x, startBottom.y, halfWidth);
        }
        Vector2 endTop = end + offset;
        Vector2 endBottom = end - offset;
        Vector3 endTopFront = new Vector3(endTop.x, endTop.y, -halfWidth);
        Vector3 endTopBack = new Vector3(endTop.x, endTop.y, halfWidth);
        Vector3 endBottomFront = new Vector3(endBottom.x, endBottom.y, -halfWidth);
        Vector3 endBottomBack = new Vector3(endBottom.x, endBottom.y, halfWidth);
        MeshData meshData;
        meshData.Vertices = new Vector3[]
        {
            startTopFront, endTopFront, endTopBack, startTopBack, // Top
            startBottomFront, endBottomFront, endBottomBack, startBottomBack, // Bottom
            startTopFront, endTopFront, endBottomFront, startBottomFront, // Front
            startTopBack, endTopBack, endBottomBack, startBottomBack // Back
        };
        meshData.UV = new Vector2[]
        {
            new Vector2(m_uvStartX, 0), new Vector2(uvEndX, 0), new Vector2(uvEndX, 1), new Vector2(m_uvStartX, 1),
            new Vector2(m_uvStartX, 0), new Vector2(uvEndX, 0), new Vector2(uvEndX, 1), new Vector2(m_uvStartX, 1),
            Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero,
            Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero
        };
        m_uvStartX = uvEndX;
        {
            Vector3 bottomNormal = Quaternion.Euler(0, 0, -90) * surfaceTangent;
            Vector3 frontNormal = Vector3.back;
            Vector3 backNormal = Vector3.forward;
            meshData.Normals = new Vector3[]
            {
                topNormal, topNormal, topNormal, topNormal,
                bottomNormal, bottomNormal, bottomNormal, bottomNormal,
                frontNormal, frontNormal, frontNormal, frontNormal,
                backNormal, backNormal, backNormal, backNormal
            };
        }
        int triangleOffset = m_meshFilter.sharedMesh.vertices.Length;
        meshData.Triangles = new int[]
        {
            // Top
            triangleOffset, triangleOffset + 2, triangleOffset + 1,
            triangleOffset, triangleOffset + 3, triangleOffset + 2,
            // Bottom
            triangleOffset + 4, triangleOffset + 5, triangleOffset + 6,
            triangleOffset + 6, triangleOffset + 7, triangleOffset + 4,
            // Front
            triangleOffset + 8, triangleOffset + 9, triangleOffset + 10,
            triangleOffset + 10, triangleOffset + 11, triangleOffset + 8,
            // Back
            triangleOffset + 12, triangleOffset + 14, triangleOffset + 13,
            triangleOffset + 14, triangleOffset + 12, triangleOffset + 15
        };
        return meshData;
    }

    public void EndPath()
    {
        if (m_previousCuboidVertices != null)
        {
            AddMeshData(GetTrailingFace());
            m_previousCuboidVertices = null;
        }
        m_previousWaypoint = null;
        m_uvStartX = 0.0f;
    }

    private MeshData GetLeadingFace(Vector2 start, Vector2 end)
    {
        float halfWidth = m_trackWidth * 0.5f;
        float halfThickness = m_trackThickness * 0.5f;
        Vector2 surfaceTangent = (end - start).normalized;
        Vector3 topNormal = Quaternion.Euler(0, 0, 90) * surfaceTangent;
        Vector2 offset = topNormal * halfThickness;
        Vector2 startTop = start + offset;
        Vector2 startBottom = start - offset;
        MeshData meshData;
        meshData.Vertices = new Vector3[]
        {
            new Vector3(startTop.x, startTop.y, -halfWidth),
            new Vector3(startTop.x, startTop.y, halfWidth),
            new Vector3(startBottom.x, startBottom.y, -halfWidth),
            new Vector3(startBottom.x, startBottom.y, halfWidth)
        };
        meshData.UV = new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        Vector3 normal = (start - end).normalized;
        meshData.Normals = new Vector3[] { normal, normal, normal, normal };
        int triangleOffset = m_meshFilter.sharedMesh.vertices.Length;
        meshData.Triangles = new int[]
        {
            triangleOffset, triangleOffset + 3, triangleOffset + 1,
            triangleOffset + 2, triangleOffset + 3, triangleOffset
        };
        return meshData;
    }

    private MeshData GetTrailingFace()
    {
        MeshData meshData;
        meshData.Vertices = new Vector3[]
        {
            m_previousCuboidVertices[1],
            m_previousCuboidVertices[2],
            m_previousCuboidVertices[5],
            m_previousCuboidVertices[6]
        };
        meshData.UV = new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        Vector3 normal = m_previousCuboidVertices[1] - m_previousCuboidVertices[0];
        meshData.Normals = new Vector3[] { normal, normal, normal, normal };
        int triangleOffset = m_meshFilter.sharedMesh.vertices.Length;
        meshData.Triangles = new int[]
        {
            triangleOffset, triangleOffset + 1, triangleOffset + 2,
            triangleOffset + 2, triangleOffset + 1, triangleOffset + 3
        };
        return meshData;
    }
}
