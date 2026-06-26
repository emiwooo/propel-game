using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] protected float speed = 3f; // horizontal speed
    [SerializeField] protected float range = 5f; // horizontal range
    public float Range => range; // to get it easily

    private float startX; // initial x position of the enemy
    private int direction = 1; // direction of movement (1 for right, -1 for left)
    
    [Header("Enemy Stats")]
    [SerializeField] protected int currentHealth = 1;
    [SerializeField] protected int maxHealth = 1;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip spawnSFX;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 0.8f;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        startX = transform.position.x;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlaySound(spawnSFX);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        float movement = direction * speed * Time.deltaTime;
        transform.Translate(Vector3.right * movement);

        // check boundaries
        if (transform.position.x >= startX + range)
        {
            direction = -1; // change direction to left
            FlipSprite();
        }
        else if (transform.position.x <= startX - range)
        {
            direction = 1; // change direction to right
            FlipSprite();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        PlaySound(deathSFX);
        Destroy(gameObject);
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, sfxVolume);
        }
    }

    protected void FlipSprite()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * direction;
        transform.localScale = localScale;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCotroller player = collision.gameObject.GetComponent<PlayerCotroller>();
            if (player != null)
            {
                player.TakeDamage(1); 
            }
            Debug.Log("Player collided with enemy: " + gameObject.name);
        }
    }
}
