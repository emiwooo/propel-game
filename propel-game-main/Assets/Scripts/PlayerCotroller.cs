using Microsoft.Extensions.DependencyInjection;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;



public class PlayerCotroller : MonoBehaviour
{

    public ParticleSystem confettiParticleSystem;
    private SpriteFlash spriteFlash;
    public Vector3 spawnPosition = new Vector3(0f, 0f, 0f);

    public Vector2 playerVelocity = new Vector2(0.0f, 0.0f);
    Vector2 playerAcceleration = new Vector2(0.0f, -0.1f);
    Vector2 realAcceleration = new Vector2(0.0f, 0.0f);

    // input
    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction ThrustAction;
    public InputAction ShootAction;
    private float moveSpeed = 5f;

    // thrust
    public float thrustOn = 0f;
    public float maxThrust = 15f; // can be upgraded in shop
    public float thrustAllow = 15f;


    public float mouseOn = 0;
    public float thrustTracker = 0;
    public float lineTracker = 0;
    public bool pizzazzPurchased = false;

    private LineRenderer lineRenderer;
    public Ground groundControl;
    public Vector3 transTotal;
    public Vector2 mouseScreenPos;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    // give player temp immunity after taking damage
    private float iSecs = 0.7f;
    private float iTimer = 0.0f;
    

    // keep track of max altitude reached
    public float currentHeight = 0;
    public float currentMaxHeight = 0; // for this run specifically

    // small candy 
    public int smallCandyThrust = 1; // can be upgraded in shop

    // gun
    public bool hasGun = false;
    public int maxAmmo = 3; // can be upgraded in shop
    public int ammoCount = 3;
    public float shotTracker = 0;

    // boost
    public bool hasBoost = false;
    public float boostDuration = 1f; // 2 sec, can be upgraded
    private float boostTimer = 0f;
    private float boostRegenRate = 4f; // i.e. 10 thurst per sec
    
    
    // player falls faster the further they fall
    private float fallTime = 0f;
    private float gameOverTime = 2f; // instant game over if thrust-less and falling for 5 secs
    private float extraGravityMultiplier = 3f;

    // so player doesn't immediately die lol
    private bool gameStarted = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.startColor = Color.purple;
        lineRenderer.endColor = Color.purple;

        MoveAction.Enable();
        LookAction.Enable();
        ThrustAction.Enable();
        ShootAction.Enable();

        lineRenderer.enabled = false;

        spriteFlash = GetComponent<SpriteFlash>();
        

    }

    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        float dt = Time.deltaTime;

        // BOOST
        if (hasBoost)
        {
            boostTimer -= dt;

            // Regenerate thrust while boosting
            thrustAllow += boostRegenRate * dt;
            thrustAllow = Mathf.Min(thrustAllow, maxThrust);

            if (boostTimer <= 0f)
            {
                hasBoost = false;
            }
        }

        // INPUT
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();

        mouseOn = ShootAction.ReadValue<float>();
        thrustOn = ThrustAction.ReadValue<float>();

        // has player started pressing thrust button?
        if (!gameStarted && thrustOn == 1f)
        {
            gameStarted = true;
        }

        // falling time tracker
        bool isFalling = playerVelocity.y < 0f;
        if (isFalling)
        {
            fallTime += dt;
        }
        else
        {
            fallTime = 0f;
        }

        // ACCELERATION
        float gravity = -50f;
        float scaledGravity = gravity * (1f + fallTime * extraGravityMultiplier);

        if (thrustOn == 1f && thrustAllow > 0 && thrustTracker == 0)
        {
            //Debug.Log($"Thrust used. Remaining: {thrustAllow}");
            playerVelocity.y = 50f;
            thrustAllow -= 1;
            thrustTracker = 1;
        }
        else if (thrustOn == 0f)
        {
            thrustTracker = 0;
        }

        // VELOCITY
        if (gameStarted) {
            playerVelocity += new Vector2(0f, scaledGravity) * dt;
            playerVelocity.y *= 0.99f; // resistance feeling
        } else
        {
            // if no input yet, no velocity yet either
            playerVelocity = new Vector2(0.0f, 0.0f);;
        }

        transform.position += (Vector3)(playerVelocity * dt);
        transform.position += Vector3.right * moveInput.x * moveSpeed * dt;
        

        // SHOOTY
        if (mouseOn == 1f && shotTracker == 0 && ammoCount > 0 && hasGun)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(LookAction.ReadValue<Vector2>());
            mouseWorldPos.z = 0;

            Vector2 aimDir = (Vector2)(mouseWorldPos - transform.position);
            aimDir.Normalize();

            GameObject bulletFix = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bulletFix.GetComponent<Rigidbody2D>();
            bulletRigid.linearVelocity = aimDir * bulletSpeed;

            ammoCount -= 1;
            shotTracker = 1;
        }

        if (mouseOn == 0)
        {
            shotTracker = 0;
        }

        if (ammoCount <= 0)
        {
            hasGun = false;
        }

        currentHeight = MathF.Max(transform.position.y, 0); // for ui
        if (currentHeight > currentMaxHeight)
        {
            currentMaxHeight = currentHeight;
        }

        // damage time tracker
        iTimer += dt;

        if (thrustAllow <= 0 && fallTime > gameOverTime) 
        {
            Debug.Log("fell to death");
            GameManager.Instance.GameOver();
        }

    }

    public void resetPlayerState()
    {
        gameStarted = false;
        thrustAllow = maxThrust;
        thrustTracker = 0;
        playerVelocity = new Vector2(0.0f, 0.0f);;
        transform.position = spawnPosition;
        currentHeight = spawnPosition.y;
        currentMaxHeight = spawnPosition.y;
        hasGun = false;
        ammoCount = 0;
    }

    // for thrust bar ui
    public float ThrustPercent
    {
        get { return thrustAllow / maxThrust; }
    }
    
    public void TakeDamage(int damage)
    {
        if (hasBoost)
        {
            // If boost is active, ignore damage
            return;
        }

        // still have invinc frames?
        if (iTimer <= iSecs)
        {
            return;
        } else
        {
            iTimer = 0; // yes can damage, reset timer 
        }

        if (spriteFlash != null)
        {
            spriteFlash.CallFlash();
        }

        thrustAllow -= damage;
        if (thrustAllow < 0)
        {
            thrustAllow = 0;
        }
    }
    public void ActivateBoost()
    {
        hasBoost = true;
        boostTimer = boostDuration;
    }

    public void SmallCandyCollected()
    {
        thrustAllow += smallCandyThrust;
        thrustAllow = Mathf.Min(thrustAllow, maxThrust); // Ensure thrust does not exceed maxThrust
    }

    public void GunCollected()
    {
        hasGun = true;
        //Debug.Log($"Gun collected. maxAmmo={maxAmmo}, ammoCount={ammoCount}");
        ammoCount = maxAmmo; 
    }

    public void ApplyShopUpgrades(ShopManager shop)
    {
        maxThrust = 10 + (2 * shop.shopDatabase["Max Thrust"].levelPurchased);

        smallCandyThrust = 1 + shop.shopDatabase["Small Candy"].levelPurchased;

        boostDuration = 1f + 0.2f * shop.shopDatabase["Large Candy"].levelPurchased;

        maxAmmo = 3 + shop.shopDatabase["Max Ammo"].levelPurchased;

        pizzazzPurchased = shop.shopDatabase["Pizzazz"].levelPurchased > 0;

        // Keep current values within the new limits
        thrustAllow = Mathf.Min(thrustAllow, maxThrust);
        //ammoCount = Mathf.Min(ammoCount, maxAmmo);
    }

        public void BurstConfetti()
    {
        if (confettiParticleSystem != null && pizzazzPurchased)
        {
            confettiParticleSystem.Play();
        }
    }


};

