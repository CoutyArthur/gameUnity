using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int minutes;
    private int seconds;

    void Awake()
    {
        scoreText.text = $"Score : {0}";
    }

    void Update()
    {
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnMinutesChanged += UpdateMinutes;
        GameManager.OnSecondsChanged += UpdateSeconds;
    }

    private void UpdateScore(int score) => scoreText.text = $"Score : {score}";
    private void UpdateMinutes(int min) => minutes = min;
    private void UpdateSeconds(int sec) => seconds = sec;
}