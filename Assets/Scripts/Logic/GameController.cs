using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI splashText;

    private void Awake()
    {
        if (!player)
            Debug.LogError("No player character set!");
    }

    private void Start()
    {
        splashText.gameObject.SetActive(false);
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

        splashText.gameObject.SetActive(true);
        for (var i = 3; i != 0; --i)
        {
            splashText.text = i.ToString();
            Debug.Log(i);
            yield return new WaitForSecondsRealtime(1);
        }

        splashText.text = "";
        splashText.gameObject.SetActive(false);

        Time.timeScale = 1;
        while (player.Health > 0)
        {
            yield return null;
        }
        
        splashText.gameObject.SetActive(true);
        splashText.text = "You lost...";
        yield return new WaitForSeconds(3);

        ResetLevel();
    }



    private void ResetLevel()
    {
        player.TakeDamage(-10);
        player.transform.position = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ILevelRoutine());
    }

    private void CompleteLevel(bool win)
    {

    }
}