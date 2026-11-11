using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    public enum GameState{MainMenu, Playing, Paused, GameOver, Victory}
    public GameState CurrentState;

    [SerializeField] private TargetSpawn targetSpawn;
    [SerializeField] private GameHUD gameHUD;

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
        for (int i=0; i<5; i++)
        {
            targetSpawn.spawnTarget();
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
}
