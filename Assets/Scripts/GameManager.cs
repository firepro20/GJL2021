using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Paused, Playing, End }
public class GameManager : Singleton<GameManager>
{
    // defines function and parameters if required
    public delegate void OnPauseCallHandler(GameState state);
    // event to subsbribe to
    public event OnPauseCallHandler OnPauseCalled;

    public Player player_;
    public UIController uiController;
    GameState gState;
    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Playing;
    }

    private void OnEnable()
    {
        OnPauseCalled += uiController.ShowPauseMenu;
    }

    private void OnDisable()
    {
        OnPauseCalled -= uiController.ShowPauseMenu;
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

}
