using UnityEngine;
using UnityEngine.EventSystems;

public class TrackDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float DistanceBetweenPoints = 4.0f;

    [SerializeField] private TrackMeshGenerator m_trackMeshGenerator;

    private Vector2 m_previousPoint;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_previousPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float distance = Vector2.Distance(m_previousPoint, eventData.position);
        if (distance >= DistanceBetweenPoints)
        {
            m_trackMeshGenerator.AddWaypoint(
                Camera.main.ScreenToWorldPoint((Vector3) eventData.position
                - new Vector3(0.0f, 0.0f, Camera.main.transform.position.z)));
            m_previousPoint = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_trackMeshGenerator.ClearPreviousWaypoint();
    }
}
