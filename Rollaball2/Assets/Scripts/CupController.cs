using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;


public class CupController : MonoBehaviour
{
    [Title("Pin Settings")]
    [SerializeField, Required]
    private Transform Pin;
    [Space]
    float pinStartHeight;
    float pinLiftHeight = 10f;

    private void Start()
    {
        pinStartHeight = Pin.position.y;
        ResetPin();
    }

    public void LiftPin()
    {
        Pin.DOMoveY(pinStartHeight + pinLiftHeight, 2f).SetEase(Ease.InQuad);
    }

    public void ResetPin()
    {
        Pin.DOMoveY(pinStartHeight, 2f).SetEase(Ease.OutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ball Detected in Proximity, Lifting Pin");
            LiftPin();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ball Exited Proximity, Resetting Pin");
            ResetPin();
        }
    }

    private void OnDestroy()
    {
        Pin.position = new Vector3(Pin.position.x, pinStartHeight, Pin.position.z);
        DOTween.Kill(Pin);
    }
}
