using UnityEngine;

public class CupController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ball entered the cup! Resetting game...");
            GameManager.ResetGame.Invoke();
        }
    }
}
