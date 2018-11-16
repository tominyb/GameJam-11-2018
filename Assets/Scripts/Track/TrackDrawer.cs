using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private List<Vector3> m_points = new List<Vector3>();

    private float m_distanceTraversed = 0.0f;

    private const float DistanceBetweenPoints = 0.5f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_points.Clear();
        m_points.Add(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_distanceTraversed += Vector3.Distance(m_points[m_points.Count - 1], eventData.position);

        if (m_distanceTraversed >= DistanceBetweenPoints)
        {
            m_points.Add(eventData.position);
            m_distanceTraversed = 0.0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_points.Add(eventData.position);
        Draw();
    }

    public void Draw()
    {
        foreach (Vector3 point in m_points)
        {
            GameObject cube         = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = Camera.main.ScreenToWorldPoint(point);
        }
    }
}
