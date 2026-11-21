using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    public enum GameState{MainMenu, Playing, Paused, GameOver, Victory}
    public GameState CurrentState;
    public static event Action<GameState> OnStateChanged;

    [SerializeField] private TargetSpawn targetSpawn;
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject SettingsPanel;

    private int score;
    public static event Action<int> OnScoreChanged;

    private float elapsedTime;
    private int minutes;
    private int seconds;
    public static event Action<int> OnMinutesChanged;
    public static event Action<int> OnSecondsChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ChangeState(GameState.Playing);
        score = 0;
        for (int i=0; i<5; i++)
        {
            targetSpawn.spawnTarget();
        }
    }

    void Update()
    {
        bool pausePressed = Keyboard.current.escapeKey.wasPressedThisFrame;
        #if UNITY_EDITOR
                pausePressed |= Keyboard.current.pKey.wasPressedThisFrame;   // P pour tester dans l'éditeur
        #endif

        if (pausePressed)
        {
            if (CurrentState == GameState.Playing)
            {
                Paused();
            }
            else if (CurrentState == GameState.Paused)
            {
                UnPaused();
                
            }
        }
        if(CurrentState == GameState.Playing)
        {
            TimeUpdate();
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState =  newState;
        if(CurrentState == GameState.Paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        OnStateChanged?.Invoke(CurrentState);
    }

    public void Paused()
    {
        ChangeState(GameState.Paused);
    }

    public void UnPaused()
    {
        ChangeState(GameState.Playing);
    }

    public void Resume() => UnPaused();


    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnEnable() => fps.OnTargetHit += AddScore;
    public void AddScore(bool touche)
    {
        if (touche)
        {
            score += 1;
            OnScoreChanged?.Invoke(score);
        }
    }

    public void TimeUpdate()
    {
        elapsedTime += Time.deltaTime;

        int newMinutes = (int)(elapsedTime / 60f);
        if (newMinutes != minutes)
        {
            minutes = newMinutes;
            OnMinutesChanged?.Invoke(minutes);
        }

        int newSeconds = (int)(elapsedTime % 60f);
        if (newSeconds != seconds)
        {
            seconds = newSeconds;
            OnSecondsChanged?.Invoke(seconds);
        }
    }
}
