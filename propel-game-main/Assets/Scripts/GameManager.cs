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
    public int maxAltitude = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance = this;
        DontDestroyOnLoad(this.gameObject);
        GameManager.Instance.ChangeState(GameState.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
            case GameState.Shop:
                break;
        }
    }
}
