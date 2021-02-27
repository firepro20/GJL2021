using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Image blackScreen;
    GameState gState;

    // Player unlocked abilities
    int[] currentCharacterPowers = { 0, 0, 0, 0, 0, 0 };
    int[] levelStartPowers = { 0, 0, 0, 0, 0, 0 };

    // Level Loading
    int levelIndex = 1;
    bool isRestarting = false;

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
        if (Input.GetButtonDown("Cancel") && gState == GameState.Playing)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && gState == GameState.Paused)
        {
            Unpause();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }
    }

    public void Pause()
    {
        OnPauseCalled?.Invoke(gState);
        gState = GameState.Paused;
    }

    public void Unpause()
    {
        OnPauseCalled?.Invoke(gState);
        gState = GameState.Playing;
    }

    public void RestartLevel()
    {
        isRestarting = true;
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOFade(1f, 1f).OnComplete(() =>
        {
            if (levelIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(levelIndex);
            Unpause();
        });
    }

    public void QuitToMainMenu()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOFade(1f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("MainMenu");
            // Reset party member and coin count
            for (int i = 0; i < currentCharacterPowers.Length; i++)
            {
                currentCharacterPowers[i] = 0;
                levelStartPowers[i] = 0;
            }
            levelIndex = 1;
            Unpause();
            Destroy(gameObject);
        });
    }

    public GameState GetGameState()
    {
        return gState;
    }

    public void SetGameState(GameState g)
    {
        gState = g;
    }

    public void LoadNextLevel(bool additive = false)
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOFade(1f, 1f).OnComplete(() =>
        {
            LoadSceneMode mode;
            if (additive)
            {
                mode = LoadSceneMode.Additive;
            }
            else
            {
                mode = LoadSceneMode.Single;
            }

            levelIndex++;
            if (levelIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(levelIndex, mode);

            // Save player unlocked powers
            levelStartPowers = (int[]) player.GetAllowedPowers().Clone();
        });
    }

    private void ActiveSceneLoaded(Scene s, LoadSceneMode mode)
    {
        // Populate fields
        player = FindObjectOfType<Player>();
        gameUIController = FindObjectOfType<UIController>();

        // Reapply saved powers across new level
        if (!s.name.Contains("Menu"))
        {
            OnPauseCalled = null;
            OnPauseCalled += gameUIController.ShowPauseMenu;

            if (isRestarting)
            {
                player.SetAllowedPowers(levelStartPowers);
                isRestarting = false;
            }
            else
            {
                player.SetAllowedPowers(currentCharacterPowers);
            }
            gameUIController.UpdatePartyMembersUI(player.GetAllowedPowers());
        }

        FadeOut();
    }

    public void SetLevelStartPowers(int[] startPowers)
    {
        levelStartPowers = startPowers;
    }

    public int[] LoadLevelStartPowers()
    {
        return levelStartPowers;
    }

    public void SaveCurrentPowers(int[] currentPowers)
    {
        currentCharacterPowers = currentPowers;
    }

    public int[] LoadCurrentPowers()
    {
        return currentCharacterPowers;
    }

    public void FadeOut()
    {
        blackScreen.DOFade(0f, 1f).OnComplete(() =>
        {
            blackScreen.gameObject.SetActive(false);
        });
    }
}
