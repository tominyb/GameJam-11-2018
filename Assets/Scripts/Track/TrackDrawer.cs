using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float DistanceBetweenPoints = 1.0f;

    [SerializeField] [Range(0, 1)] private float m_smoothingFactor = 0.2f;
    [SerializeField] private int m_smoothLimit = 4;
    [SerializeField] private TrackMeshGenerator m_trackMeshGenerator;

    private int m_nonTrackLayerMask;
    private List<Vector2> m_points = new List<Vector2>();

    private void Start()
    {
        m_nonTrackLayerMask = ~LayerMask.GetMask("Surface");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_points.Clear();
        m_points.Add(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, m_nonTrackLayerMask))
            {
                StopDrawing();
                return;
            }
        }
        float distance = Vector2.Distance(m_points[m_points.Count - 1], eventData.position);
        if (distance >= DistanceBetweenPoints)
        {
            m_points.Add(eventData.position);
            if (m_points.Count > m_smoothLimit)
            {
                SmoothWaypoints();
                m_trackMeshGenerator.AddWaypoint(
                    Camera.main.ScreenToWorldPoint((Vector3) m_points[m_points.Count - 1]
                    - new Vector3(0.0f, 0.0f, Camera.main.transform.position.z)));
            }
        }
    }

    private void SmoothWaypoints()
    {
        int pointCount = m_points.Count;
        Vector2 p0 = m_points[pointCount - 1];
        Vector2 p1 = m_points[pointCount - 2];
        float oneMinusSmoothingFactor = 1.0f - m_smoothingFactor;
        m_points[pointCount - 1] = new Vector2(
            (p0.x * m_smoothingFactor) + (p1.x * oneMinusSmoothingFactor),
            (p0.y * m_smoothingFactor) + (p1.y * oneMinusSmoothingFactor));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StopDrawing();
    }

    private void StopDrawing()
    {
        m_trackMeshGenerator.EndPath();
    }
}
