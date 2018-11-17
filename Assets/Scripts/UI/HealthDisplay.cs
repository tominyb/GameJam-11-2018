using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject m_icon;
    [SerializeField] private int m_health;

    private GameObject[] m_icons;

    private void Start()
    {
        m_icons = new GameObject[m_health];
        for (int i = 0; i < m_health; ++i)
        {
            m_icons[i] = Instantiate(m_icon, transform);
        }
    }
}
