using UnityEngine;

[System.Serializable]
public class BoundingBox
{
    [SerializeField] private Vector2 m_min;
    [SerializeField] private Vector2 m_max;

    public BoundingBox(Vector2 min, Vector2 max)
    {
        m_min = min;
        m_max = max;
    }

    public Vector2 ClampInside(Vector2 v)
    {
        return new Vector2(
            Mathf.Clamp(v.x, m_min.x, m_max.x),
            Mathf.Clamp(v.y, m_min.y, m_max.y)
        );
    }
}
