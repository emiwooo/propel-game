using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip bulletSFX;
    public float lifetime = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlaySound(bulletSFX);
        Destroy(gameObject, lifetime);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}
