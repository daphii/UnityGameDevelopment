using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    float timeElapsed = 0f;
    float scoreMultiplier = 3f;
    int score = 0;

    public UIDocument uiDocument;
    private Label scoreLabel;

    public TextMeshProUGUI scoreText;

    bool isGameOver = false;

    private void Awake()
    {
        GameManager.LoseCondition.AddListener(GameOver);
        scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
    }

    private void Update()
    {
        if (isGameOver) return;

        timeElapsed += Time.deltaTime;
        score = Mathf.FloorToInt(timeElapsed * scoreMultiplier);
        UpdateText(score);
    }

    void UpdateText(int newScore)
    {
        string formattedScore = "Score: " + newScore.ToString();
        scoreLabel.text = formattedScore;
        scoreText.text = formattedScore;

    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
