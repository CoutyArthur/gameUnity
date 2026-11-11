using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float elapsed;
    private int score;

    void OnEnable() => fps.OnTargetHit += AddScore;
    void OnDisable() => fps.OnTargetHit -= AddScore;

    void Awake()
    {
        UpdateScore();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        int minutes = (int)(elapsed / 60f);
        int seconds = (int)(elapsed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddScore(bool touche)
    {
        if (touche)
        {
            score += 1;
            UpdateScore();
        }
    }

    private void UpdateScore() => scoreText.text = $"Score : {score}";
}