using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private float      m_speed;
    [SerializeField] private Vector2    m_offset;
    [SerializeField] private GameObject m_target;

    private void LateUpdate()
    {
        if (m_target == null)
            return;

        var xy = Vector2.Lerp(
            transform.position,
            (Vector2)m_target.transform.position + m_offset,
            m_speed * Time.deltaTime
        );
        transform.position = new Vector3(xy.x, xy.y, transform.position.z);
    }

    public void SetTarget(GameObject newTarget)
    {
        m_target = newTarget;
    }
}
