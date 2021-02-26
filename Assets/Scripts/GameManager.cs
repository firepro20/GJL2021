using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Paused, End }
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // defines function and parameters if required
    public delegate void OnPauseCallHandler(GameState state);
    // event to subsbribe to
    public event OnPauseCallHandler OnPauseCalled;

    public Player player;
    public UIController gameUIController;
    GameState gState;

    // Player unlocked abilities
    int[] currentCharacterPowers = { 0, 0, 0, 0, 0, 0 };

    // Level Loading
    int levelIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Playing;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ActiveSceneLoaded;
        OnPauseCalled += gameUIController.ShowPauseMenu;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ActiveSceneLoaded;
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadLevel();
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
        levelIndex++;
        if (levelIndex < SceneManager.sceneCountInBuildSettings)
        SceneManager.LoadScene(levelIndex, mode);

        // Save player unlocked powers
        currentCharacterPowers = player.GetAllowedPowers();
    }

    private void ActiveSceneLoaded(Scene s, LoadSceneMode mode)
    {
        // Populate fields
        player = FindObjectOfType<Player>();
        gameUIController = FindObjectOfType<UIController>();

        // Reapply saved powers across new level
        if (!s.name.Contains("Menu")) 
        {
            player.SetAllowedPowers(currentCharacterPowers);
        }
       
    }
}
