using UnityEngine;
using UnityEngine.EventSystems;

public class TrackDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float DistanceBetweenPoints = 1.0f;

    [SerializeField] private TrackMeshGenerator m_trackMeshGenerator;

    private int m_nonTrackLayerMask;
    private Vector2 m_previousPoint;

    private void Start()
    {
        m_nonTrackLayerMask = ~LayerMask.GetMask("Surface");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_previousPoint = eventData.position;
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
        StopDrawing();
    }

    private void StopDrawing()
    {
        m_trackMeshGenerator.EndPath();
    }
}
