using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private Player m_player;
    [SerializeField] private Zone m_endZone;
    [SerializeField] private TrackController m_trackController;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI m_splashText;

    private void Awake()
    {
        if (!m_player)
            Debug.LogError("No player character set!");
    }

    private void Start()
    {
        m_endZone.OnPlayerEnter(CompleteLevel);
        m_splashText.gameObject.SetActive(false);
        StartCoroutine(ILevelRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetLevel();
    }

    private IEnumerator ILevelRoutine()
    {
        Time.timeScale = 0;

        m_splashText.gameObject.SetActive(true);
        for (var i = 3; i != 0; --i)
        {
            m_splashText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        m_splashText.text = "";
        m_splashText.gameObject.SetActive(false);

        Time.timeScale = 1;
        while (m_player.Health > 0)
        {
            yield return null;
        }
        
        m_splashText.gameObject.SetActive(true);
        m_splashText.text = "You lost...";
        StartCoroutine(IStartLevelResetRoutine());
    }

    private IEnumerator IStartLevelResetRoutine()
    {
        yield return new WaitForSeconds(3);

        ResetLevel();
    }

    private void ResetLevel()
    {
        m_player.TakeDamage(-10);
        m_player.transform.position = Vector3.zero;
        m_player.ResetForces();
        m_trackController.ResetAll();
        StopAllCoroutines();
        StartCoroutine(ILevelRoutine());
    }

    private void CompleteLevel()
    {
        m_splashText.gameObject.SetActive(true);
        m_splashText.text = "You win!";
        StartCoroutine(IStartLevelResetRoutine());
    }
}