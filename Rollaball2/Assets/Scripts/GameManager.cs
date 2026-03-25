using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static UnityEvent ResetGame = new();

    private void Awake()
    {
        ResetGame.AddListener(OnResetGame);
    }

    public void OnResetGame()
    {
        StartCoroutine(ResetDelay(2f));
    }

    IEnumerator ResetDelay(float delay = 0)
    {
        Debug.Log($"Resetting game in {delay} seconds...");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
