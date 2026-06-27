using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    
    [SerializeField] private PlayerCotroller playerController;
    [SerializeField] private TMPro.TextMeshProUGUI altitudeText;
    [SerializeField] private TMPro.TextMeshProUGUI highestAltitudeText;
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        float altitude = playerController.currentMaxHeight;
        altitudeText.text = altitude.ToString("F0") + "m";
        highestAltitudeText.text = "Highest Record\n" + GameManager.Instance.highestAltitude.ToString("F0") + "m";
        moneyText.text = "You gained $" + (int)(playerController.currentMaxHeight / 10) + "!";
    }
}
