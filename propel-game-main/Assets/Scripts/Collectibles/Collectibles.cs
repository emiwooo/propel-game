using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class CollectBehaviour : MonoBehaviour
{
    public AudioClip pickupSFX;
    public PlayerCotroller player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerCotroller>();
        }
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
            //Debug.Log("Player collided with collectible: " + gameObject.name);
        }
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    protected abstract void Collect(PlayerCotroller player);

}