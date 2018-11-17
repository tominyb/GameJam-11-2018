using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject m_icon;
    [SerializeField] private Player m_player;

    private GameObject[] m_icons;

    private void Start()
    {
        InitIcons();
        m_player.OnHealthChanged += OnHealthChanged;
    }

    private void InitIcons()
    {
        int initialHealth = m_player.Health;
        m_icons = new GameObject[initialHealth];
        for (int i = 0; i < initialHealth; ++i)
        {
            m_icons[i] = Instantiate(m_icon, transform);
        }
    }

    private void OnHealthChanged(int newHealth)
    {
        int iconCount = m_icons.Length;
        for (int i = newHealth; i < iconCount; ++i)
        {
            m_icons[i].SetActive(false);
        }
    }
}
