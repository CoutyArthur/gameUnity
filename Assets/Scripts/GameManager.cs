using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    public enum GameState{MainMenu, Playing, Paused, GameOver, Victory}
    public GameState CurrentState;

    [SerializeField] private TargetSpawn targetSpawn;
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private GameObject pausePanel;

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
    }

    public void Paused()
    {
        ChangeState(GameState.Paused);
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnPaused()
    {
        ChangeState(GameState.Playing);
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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
}
