using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPanel : MonoBehaviour
{
    [SerializeField] private float m_rowHeight;
    [SerializeField] private RectTransform m_healthBarPrefab;
    [SerializeField] private float offsetX;
    [SerializeField] private int m_health;

    private RectTransform m_rect;
    private List<RectTransform> m_rows = new List<RectTransform>();

    private int HealthBarsPerRow
    {
        get
        {
            return (int) (m_rect.rect.width / (m_healthBarPrefab.rect.width + offsetX));
        }
    }

    private void Awake()
    {
        m_rect = (RectTransform) transform;

        for (int i = 0; i < m_health; ++i)
        {
            AddHealthBar();
        }
    }

    private void AddHealthBar()
    {
        if (m_rows.Count <= (m_health / (HealthBarsPerRow)))
        {
            AddRow();
        }

        RectTransform parent = m_rows[m_rows.Count - 1];
        RectTransform bar    = Instantiate(m_healthBarPrefab, parent);

        bar.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (parent.childCount - 1) * (m_healthBarPrefab.rect.width + offsetX), bar.rect.width);
    }

    private void RemoveHealthBar()
    {
        if (m_rows.Count == 0 || (m_rows.Count == 1 && m_rows[m_rows.Count - 1].childCount == 0))
        {
            return;
        }

        if (m_rows[m_rows.Count - 1].childCount == 0)
        {
            RemoveRow();
        }

        Transform row = m_rows[m_rows.Count - 1];
        Destroy(row.GetChild(row.childCount - 1).gameObject);
    }

    private void AddRow()
    {
        RectTransform row = new GameObject().AddComponent<RectTransform>();
        row.name = $"Row {m_rows.Count}";
        row.SetParent(transform);
        row.localPosition = Vector3.zero;
        row.sizeDelta = new Vector2(m_rect.rect.width, m_rowHeight);
        row.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_rowHeight * m_rows.Count, m_rowHeight);
        m_rows.Add(row);
    }

    private void RemoveRow()
    {
        if (m_rows.Count == 0)
        {
            return;
        }

        Destroy(m_rows[m_rows.Count - 1].gameObject);
        m_rows.RemoveAt(m_rows.Count - 1);
    }

    public void ChangeHealth(int change)
    {
        m_health += change;

        if (change > 0)
        {
            for (int i = 0; i < change; ++i)
            {
                AddHealthBar();
            }
        }

        else if (change < 0)
        {
            for (int i = 0; i < Mathf.Abs(change); ++i)
            {
                RemoveHealthBar();
            }
        }
    }
}
