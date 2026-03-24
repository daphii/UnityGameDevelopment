using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndText : MonoBehaviour
{
    public static UnityEvent WinCondition = new();
    public static UnityEvent LoseCondition = new();

    bool hasEnded = false;

    [Title("End Text Options")]
    public string WinMessage = "Congratulations! You've collected all the pickups!";
    public string LoseMessage = "Game Over! Try again!";

    [Title("Components")]
    public TextMeshProUGUI TextField;

    private void Awake()
    {
        WinCondition.AddListener(OnWinCondition);
        LoseCondition.AddListener(OnLoseCondition);
    }

    void OnWinCondition()
    {
        if (hasEnded) return;

        hasEnded = true;
        Debug.Log(WinMessage);
        TextField.text = WinMessage;
        TextField.gameObject.SetActive(true);
        StartCoroutine(ResetTimer(3f));
    }

    void OnLoseCondition()
    {
        if (hasEnded) return;

        hasEnded = true;
        Debug.Log(LoseMessage);
        TextField.text = LoseMessage;
        TextField.gameObject.SetActive(true);
        StartCoroutine(ResetTimer(3f));
    }

    IEnumerator ResetTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
    }
    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
