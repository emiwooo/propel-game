using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class CollectBehaviour : MonoBehaviour
{
    [SerializeField] public AudioClip pickupSFX;
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 0.8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCotroller player = collision.gameObject.GetComponent<PlayerCotroller>();
            if (player != null)
            {
                Collect(player);
            }
            Debug.Log("Player collided with collectible: " + gameObject.name);
        }
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, sfxVolume);
        }
    }

    protected abstract void Collect(PlayerCotroller player);

}