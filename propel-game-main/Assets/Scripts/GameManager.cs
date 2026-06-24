using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Shop
    }
    public GameState CurrentState { get; private set; }
    public int maxAltitude = 0;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayHUDPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject shopPanel;

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
        
    }

    void ChangeState(GameState newState)
    {
        CurrentState = newState;
        
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
            case GameState.Shop:
                Time.timeScale = 0f;
                break;
        }

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
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
            ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void OpenShop()
    {
        ChangeState(GameState.Shop);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }

}
