using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMeshGenerator : MonoBehaviour
{
    private const int VerticesPerStep = 4;
    private const int TrianglesPerStep = 2;
    private const int TriangleIndicesPerStep = TrianglesPerStep * 3;

    [SerializeField] private float m_trackWidth;
    [SerializeField] private Vector2[] m_waypoints = { };
    [SerializeField] private Material m_material;

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
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        if (m_waypoints.Length < 2)
        {
            Debug.LogError("At least 2 waypoints are necessary for vertex generation.");
            return null;
        }
        float halfTrackWidth = m_trackWidth * 0.5f;
        int stepCount = m_waypoints.Length - 1;
        Vector3[] vertices = new Vector3[stepCount * VerticesPerStep];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[stepCount * TriangleIndicesPerStep];
        for (int i = 0, j = 0, k = 0; i < stepCount; ++i, j += VerticesPerStep, k += TriangleIndicesPerStep)
        {
            Vector2 start = m_waypoints[i];
            Vector2 end = m_waypoints[i + 1];
            vertices[j] = new Vector3(start.x, start.y, -halfTrackWidth);
            vertices[j + 1] = new Vector3(end.x, end.y, -halfTrackWidth);
            vertices[j + 2] = new Vector3(end.x, end.y, halfTrackWidth);
            vertices[j + 3] = new Vector3(start.x, start.y, halfTrackWidth);
            {
                Vector3 normal = Quaternion.Euler(0, 0, 90) * (end - start);
                for (int n = 0; n < VerticesPerStep; ++n)
                {
                    normals[j + n] = normal;
                }
            }
            triangles[k] = j;
            triangles[k + 1] = j + 2;
            triangles[k + 2] = j + 1;
            triangles[k + 3] = j;
            triangles[k + 4] = j + 3;
            triangles[k + 5] = j + 2;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        return mesh;
    }
}
