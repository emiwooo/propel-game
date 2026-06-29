using UnityEngine.InputSystem;
using UnityEngine;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerCotroller playerController;
    public CamerControls camerControls;
    public LevelGenerator levelGenerator;
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Shop,
        Controls
    }
    public GameState CurrentState { get; private set; }
    public float highestAltitude = 0;
    public InputAction PauseAction;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayHUDPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Player Stats")]
    public int money { get; set; } = 0;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseAction.WasPressedThisFrame())
        {
            if (CurrentState == GameState.Playing)
                PauseGame();
            else if (CurrentState == GameState.Paused)
                ResumeGame();
        }
    }

    void ChangeState(GameState newState)
    {
        CurrentState = newState;
        UpdateUI(newState);
    }
    
    void UpdateUI(GameState state)
    {
        // disable all panels first
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (gameplayHUDPanel) gameplayHUDPanel.SetActive(false);
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(false);
        if (controlsPanel) controlsPanel.SetActive(false);

        // enable the relevant panel based on the current state
        switch (state)
        {
            case GameState.MainMenu:
                if (mainMenuPanel) mainMenuPanel.SetActive(true);
                break;
            case GameState.Playing:
                if (gameplayHUDPanel) gameplayHUDPanel.SetActive(true);
                break;
            case GameState.Paused:
                if (pauseMenuPanel) pauseMenuPanel.SetActive(true);
                break;
            case GameState.GameOver:
                if (gameOverPanel) gameOverPanel.SetActive(true);
                break;
            case GameState.Shop:
                if (shopPanel) shopPanel.SetActive(true);
                break;
            case GameState.Controls:
                if (controlsPanel) controlsPanel.SetActive(true);
                break;
        }
    }


    public void StartGame()
    {
        UnityEngine.Debug.Log("resetting level");

        // need to reset state of player
        playerController.resetPlayerState();
        levelGenerator.ResetAndGenerate();
        ChangeState(GameState.Playing);
        playerController.BurstConfetti();
    }

    public void PauseGame()
    {
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void GameOver()
    {
        if (playerController.currentMaxHeight > highestAltitude)
        {
            highestAltitude = playerController.currentMaxHeight;
        }
        money += (int)playerController.currentMaxHeight / 10;
        ChangeState(GameState.GameOver);
        // resetting again just in case
        levelGenerator.ResetAndGenerate();
    }

    public void OpenShop()
    {
        ChangeState(GameState.Shop);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }

    public void Controls()
    {
        ChangeState(GameState.Controls);
    }

    void OnEnable()
    {
        PauseAction.Enable();
    }

    void OnDisable()
    {
        PauseAction.Disable();
    }

}
