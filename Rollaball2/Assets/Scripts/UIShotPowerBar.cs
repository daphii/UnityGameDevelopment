using UnityEngine;
using UnityEngine.UI;

public class UIShotPowerBar : MonoBehaviour
{
    public BallController BallController;

    Slider Slider;

    private void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Slider.value = 0f;
    }


    private void Update()
    {
        if (BallController != null)
        {
            Slider.value = BallController.ChargePercent;
        }
    }
}