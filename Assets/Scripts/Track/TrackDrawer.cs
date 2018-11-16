using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private List<Vector3> m_points = new List<Vector3>();

    private const float DistanceBetweenPoints = 50.0f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_points.Clear();
        m_points.Add(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float distance = Vector3.Distance(m_points[m_points.Count - 1], eventData.position);

        if (distance >= DistanceBetweenPoints)
        {
            m_points.Add(eventData.position);
            Draw(eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_points.Add(eventData.position);
    }

    public void Draw(Vector3 point)
    {
        GameObject cube         = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = Camera.main.ScreenToWorldPoint(point - new Vector3(0.0f, 0.0f, Camera.main.transform.position.z));
    }
}
