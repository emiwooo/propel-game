using UnityEngine;

public class BulletSound : MonoBehaviour
{
    public AudioClip bulletSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlaySound(bulletSFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}
