using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static UnityEvent WinCondition = new();
    public static UnityEvent LoseCondition = new();

    public GameObject Player;
    public GameObject GameOverExplosion;

    private void Awake()
    {
        WinCondition.AddListener(OnWin);
        LoseCondition.AddListener(OnLose);
    }

    private void OnWin()
    {
        // Handle win condition
    }
    private void OnLose()
    {
        Instantiate(GameOverExplosion, Player.transform.position, Player.transform.rotation);
        Player.SetActive(false);
        StartCoroutine(ResetTimer(2f));
    }

    IEnumerator ResetTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Reset();
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
