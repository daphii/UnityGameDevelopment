using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBallSelectIndicator : MonoBehaviour
{
    public static UnityEvent<GameObject> NewBallSelected = new();

    CanvasGroup CanvasGroup;

    [Title("Components")]
    public Slider PowerSlider;
    public Slider InfluenceSlider;
    public Slider BouncinessSlider;

    [Space]
    public TextMeshProUGUI BallNameText;


    float fadeWindowDelay = 2f;
    float fadeWindowTimer;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        NewBallSelected.AddListener(OnNewBallSelected);
    }

    private void Start()
    {
        CanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (fadeWindowTimer > 0)
        {
            fadeWindowTimer -= Time.deltaTime;
            if (fadeWindowTimer < 0)
            {
                CanvasGroup.DOFade(0f, 1f);
                fadeWindowTimer = 0f;
            }
        }
    }

    void OnNewBallSelected(GameObject ball)
    {
        CanvasGroup.DOKill();
        CanvasGroup.DOFade(1f, 0.25f);

        if (ball.TryGetComponent<BallDataReader>(out var ballData))
        {
            PowerSlider.DOValue(ballData.PowerRating, 0.25f);
            InfluenceSlider.DOValue(ballData.InfluenceRating, 0.25f);
            BouncinessSlider.DOValue(ballData.BouncinessRating, 0.25f);
            fadeWindowTimer = fadeWindowDelay;
            BallNameText.text = ballData.Name;
        }
        else
        {
            Debug.LogError("Selected ball does not have BallDataReader component.");
        }
    }


}
