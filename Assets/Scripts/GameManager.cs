using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameStates { countDown, running, raceOver};

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    GameStates gameStates = GameStates.countDown;

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void LevelStart()
    {
        gameStates = GameStates.countDown;
        Debug.Log("Level Started");
    }

    public GameStates GetGameStates()
    {
        return gameStates;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameStates != newGameState)
        {
            gameStates = newGameState;

            OnGameStateChanged?.Invoke(this);
        }
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");
        ChangeGameState(GameStates.running);
    }

    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted");
        ChangeGameState(GameStates.raceOver);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }

}
