using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIShotPowerBar : MonoBehaviour
{
    public ShotController ShotController;

    Slider Slider;
    CanvasGroup SliderCanvas;

    bool BarActive;

    float lastValue;

    private void Awake()
    {
        Slider = GetComponent<Slider>();
        SliderCanvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Slider.value = 0f;
        SliderCanvas.alpha = 0f;
    }


    private void Update()
    {
        if (ShotController != null)
        {
            SetSlider(ShotController.ChargePercent);
        }
    }

    void SetSlider(float charge)
    {
        lastValue = Slider.value;
        if (charge > 0)
        {
            if (!BarActive)
            {
                FadeInSlider();
            }
            Slider.value = charge;
        }
        else if (BarActive && charge == 0 && lastValue > 0)
        {
            FadeOutSlider();
        }
    }

    void FadeOutSlider()
    {
        BarActive = false;
        SliderCanvas.DOFade(0f, 1f).SetDelay(1f);

    }

    void FadeInSlider()
    {
        SliderCanvas.DOKill();
        Slider.value = 0f;
        BarActive = true;
        SliderCanvas.DOFade(1f, 0.25f);
    }
}