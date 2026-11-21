using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;

    void OnEnable() => GameManager.OnStateChanged += OnState;
    void OnDisable() => GameManager.OnStateChanged -= OnState;

    void Start() 
    {
        UpdateSensitivityText(sensitivitySlider.value); 
    }

    private void OnState(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Paused)
        {
            pausePanel.SetActive(true);
            UnSettings();
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void Settings()
    {
        mainPausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void UnSettings()
    {
        mainPausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void UpdateSensitivityText(float value)
    {
        sensitivityValueText.text = value.ToString("0.00");
    }
}
