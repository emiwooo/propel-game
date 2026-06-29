using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private  PlayerCotroller player;
    [SerializeField] private TMPro.TextMeshProUGUI statsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerCotroller>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        statsText.text = player.StatsDesc();
    }
}
