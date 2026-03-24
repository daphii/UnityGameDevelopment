using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Score: {score}/{totalPickups}\n";
        return info;
    }

    [Title("Pickup Settings")]
    public GameObject PickupContainer;
    int totalPickups;


    [Title("UI Display")]
    [SerializeField]
    TextMeshProUGUI ScoreText;

    public static UnityEvent PickupRetrieved = new();


    int score;

    private void Awake()
    {
        PickupRetrieved.AddListener(OnPickupRetrieved);
    }

    private void Start()
    {
        score = 0;
        totalPickups = PickupContainer.transform.childCount;
        UpdateScoreText();
    }

    void OnPickupRetrieved()
    {
        score++;
        UpdateScoreText();
        Debug.Log($"Pickup retrieved! Current score: {score}");
        if (AllPickupsCollected())
        {
            EndText.WinCondition.Invoke();
        }
    }

    void UpdateScoreText()
    {
        ScoreText.text = $"Score: {score}/{totalPickups}";
    }

    bool AllPickupsCollected()
    {
        return score >= totalPickups;
    }
}
