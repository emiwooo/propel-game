using UnityEngine;

public class AltitudeDisplay : MonoBehaviour
{
    [SerializeField] private PlayerCotroller playerController;
    [SerializeField] private TMPro.TextMeshProUGUI altitudeText;

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
    public void UpdateText()
    {
        altitudeText.text = "Altitude\n" + playerController.currentHeight.ToString("F0") + "m";
    }

}
