using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Paused, End }
public class GameManager : Singleton<GameManager>
{
    // defines function and parameters if required
    public delegate void OnPauseCallHandler(GameState state);
    // event to subsbribe to
    public event OnPauseCallHandler OnPauseCalled;

    public Player player;
    public UIController gameUIController;
    GameState gState;

    // Level Loading
    int levelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Playing;
    }

    private void OnEnable()
    {
        OnPauseCalled += gameUIController.ShowPauseMenu;
    }

    private void OnDisable()
    {
        OnPauseCalled -= gameUIController.ShowPauseMenu;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInterruptInput();
    }

    void CheckInterruptInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gState == GameState.Playing)
        {
            OnPauseCalled?.Invoke(gState);
            gState = GameState.Paused;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gState == GameState.Paused)
        {
            OnPauseCalled?.Invoke(gState);
            gState = GameState.Playing;
        }
    }

    public GameState GetGameState()
    {
        return gState;
    }

    public void SetGameState(GameState g)
    {
        gState = g;
    }

    public void LoadLevel(bool additive = false)
    {
        LoadSceneMode mode;
        if (additive) { mode = LoadSceneMode.Additive; } else { mode = LoadSceneMode.Single; }
        SceneManager.LoadScene(++levelIndex, mode);
    }
}
