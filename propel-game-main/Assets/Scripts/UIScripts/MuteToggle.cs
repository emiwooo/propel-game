using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    private bool isMuted = false;

    void Start()
    {
        // 0 = unmuted, 1 = muted
        isMuted = PlayerPrefs.GetInt("GameMuted", 0) == 1;
        ApplyMute();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("GameMuted", isMuted ? 1 : 0);
        ApplyMute();
    }

    private void ApplyMute()
    {
        AudioListener.volume = isMuted ? 0f : 1f;
    }
}