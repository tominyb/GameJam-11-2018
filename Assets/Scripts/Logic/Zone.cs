using UnityEngine;

public class Zone : MonoBehaviour
{
    private System.Action m_callback;

    public void OnPlayerEnter(System.Action callback)
    {
        m_callback = callback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            m_callback?.Invoke();
    }
}
