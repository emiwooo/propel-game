using UnityEngine;
using UnityEngine.UI;

public class ThrustBar : MonoBehaviour
{
    public PlayerCotroller player;
    public Slider slider;

    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(
            slider.value,
            player.ThrustPercent,
            Time.deltaTime * 10f
        );
    }

    
}