using UnityEngine;

public class PickupController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.PickupRetrieved.Invoke();
            gameObject.SetActive(false);
        }
    }
}
