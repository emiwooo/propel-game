using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Data;
using System.Drawing;

public class ThrustBar : MonoBehaviour
{
    public PlayerCotroller player;
    public Slider slider;
    UnityEngine.Color myGreen = new UnityEngine.Color(0.2667f, 0.7647f, 0.3922f);
    UnityEngine.Color myPink = new UnityEngine.Color(0.9686f, 0.4471f, 0.7647f);
    UnityEngine.Color myGrey = new UnityEngine.Color(0.65f, 0.65f, 0.65f);
    

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

        var fill = (slider as UnityEngine.UI.Slider).GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");

        if (player.hasBoost)
        {
            fill.color = myPink;
        } else
        {
            fill.color = myGreen;
        }

        if (player.thrustAllow < 1)
        {
            fill.color = myGrey;
        }

    }

    
}