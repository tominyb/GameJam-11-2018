using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowTarget : MonoBehaviour
{
    public bool IsFollowing { get; set; }

    [SerializeField] private float          m_speed;
    [SerializeField] private float          m_threshold;
    [SerializeField] private Vector2        m_offset;
    [SerializeField] private GameObject     m_target;

    private void Start()
    {
        IsFollowing = true;
    }

    private void FixedUpdate()
    {
        if (m_target == null || !IsFollowing)
            return;

        Vector2 pos = transform.position;
        Vector2 targetPos = m_target.transform.position;
        if ((targetPos - pos).sqrMagnitude < m_threshold * m_threshold)
            return;

        var xy = Vector2.Lerp(pos, targetPos + m_offset, m_speed * Time.deltaTime);
        transform.position = new Vector3(xy.x, xy.y, transform.position.z);
    }
}
