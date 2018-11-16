using System.Collections;
using System.Collections.Generic;
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
    private List<Vector2> m_waypoints;
    private MeshFilter m_meshFilter;

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
        InitWaypoints();
        InitMeshRenderer();
        InitMeshFilter();
    }

    private void InitWaypoints()
    {
        m_waypoints = new List<Vector2>(m_maxWaypointCount);
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
        if (m_waypoints.Count == m_maxWaypointCount)
        {
            Debug.LogWarning("Max waypoint count reached. Discarding new waypoint.");
            return;
        }
        m_waypoints.Add(waypoint);
        if (m_waypoints.Count > 1)
        {
            Mesh previousMesh = m_meshFilter.sharedMesh;
            Mesh mesh = new Mesh();
            QuadBetweenWaypoints quad = new QuadBetweenWaypoints(
                waypoint, m_waypoints[m_waypoints.Count - 2],
                m_trackWidth, previousMesh.vertices.Length);
            mesh.vertices = previousMesh.vertices.Concat(quad.Vertices).ToArray();
            mesh.normals = previousMesh.normals.Concat(quad.Normals).ToArray();
            mesh.triangles = previousMesh.triangles.Concat(quad.Triangles).ToArray();
            m_meshFilter.mesh = mesh;
        }
    }
}
