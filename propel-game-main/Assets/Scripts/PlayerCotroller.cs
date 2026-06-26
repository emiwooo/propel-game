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
    public Vector3 spawnPosition = Vector3.zero;

    public Vector2 playerVelocity = new Vector2(0.0f, 0.0f);
    Vector2 playerAcceleration = new Vector2(0.0f, -0.1f);
    Vector2 realAcceleration = new Vector2(0.0f, 0.0f);
    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction ThrustAction;
    public InputAction ShootAction;

    // thrust
    public float thrustOn = 0f;
    public float maxThrust = 20f; // can be upgraded in shop
    public float thrustAllow = 20f;


    public float mouseOn = 0;
    public float thrustTracker = 0;
    public float lineTracker = 0;
    //Vector2 cursorOffset = new Vector2(168f, 203f);
    public bool pizzazzPurchased = false;

    private LineRenderer lineRenderer;
    public Ground groundControl;
    public Vector3 transTotal;
    public Vector2 mouseScreenPos;
    public GameObject bulletPrefab;
    public Rigidbody2D bulletRigid;

    // keep track of max altitude reached
    public float currentHeight = 0;
    public float currentMaxHeight = 0; // for this run specifically

    // small candy 
    public int smallCandyThrust = 1; // can be upgraded in shop

    // gun
    public bool hasGun = false;
    public int maxAmmo = 5; // can be upgraded in shop
    public int ammoCount = 5;

    // boost
    public bool hasBoost = false;
    public float boostDuration = 1f; // 1 sec
    public float boostTimer = 0f;
    public float boostRegenRate = 2f; // i.e. 2 thurst per sec
    
    


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

        // ACCELERATION
        Vector2 acceleration = new Vector2(0f, -0.003f); // gravity

        if (thrustOn == 1f && thrustAllow > 0 && thrustTracker == 0)
        {
            acceleration.y = 0.2f; // thrust up
            thrustAllow -= 1;
            thrustTracker = 1;
        }
        else if (thrustOn == 0f)
        {
            thrustTracker = 0;
        }

        // VELOCITY
        playerVelocity += acceleration;
        

        playerVelocity.y = Mathf.Clamp(playerVelocity.y, -3f, 0.5f);
            
            // MOVOE!!!
            Vector3 movement =
            new Vector3(moveInput.x, 0, 0) * 3f * dt;
        
        if (groundControl.playerY == 0f)
            {
                playerVelocity.y = Mathf.Clamp(playerVelocity.y, 0f, 0.5f);
                
            }
        
        transTotal = (Vector3)playerVelocity + movement;
        transform.position += (Vector3)playerVelocity + movement;
        

        // LINE RENDERER
        if (mouseOn == 1f)
        {
            lineRenderer.enabled = true;

            Vector2 mouseScreenPos = LookAction.ReadValue<Vector2>();
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bulletRigid = bulletPrefab.GetComponent<Rigidbody2D>();
            
            Debug.Log(bulletRigid.linearVelocity);
        Vector3 mouseWorldPos =
                Camera.main.ScreenToWorldPoint(
                    new Vector3(mouseScreenPos.x, mouseScreenPos.y, -Camera.main.transform.position.z)
                );

            mouseWorldPos.z = 0;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, mouseWorldPos);
        }
        else
        {
            lineRenderer.enabled = false;
        }

        currentHeight = transform.position.y; // for ui
        if (currentHeight > currentMaxHeight)
        {
            currentMaxHeight = currentHeight;
        }
        //Debug.Log("Current Height: " + currentHeight);

        Debug.Log(thrustAllow);

        // for testing
        if (thrustAllow <= 0)
        {
            GameManager.Instance.GameOver();
        }

    }

    public void resetPlayerState()
    {
        thrustAllow = maxThrust;
        thrustTracker = 0;
        playerVelocity = new Vector2(0.0f, 0.0f);
        transTotal = Vector3.zero;
        transform.position = spawnPosition;
        currentHeight = spawnPosition.y;
        currentMaxHeight = spawnPosition.y;
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
        ammoCount = maxAmmo; 
    }

    public void ApplyShopUpgrades(ShopManager shop)
    {
        maxThrust = 5 + shop.shopDatabase["Max Thrust"].levelPurchased;

        smallCandyThrust = 1 + shop.shopDatabase["Small Candy"].levelPurchased;

        boostDuration = 1f + 0.5f * shop.shopDatabase["Large Candy"].levelPurchased;

        maxAmmo = 5 + shop.shopDatabase["Max Ammo"].levelPurchased;

        pizzazzPurchased = shop.shopDatabase["Pizzazz"].levelPurchased > 0;

        // Keep current values within the new limits
        thrustAllow = Mathf.Min(thrustAllow, maxThrust);
        ammoCount = Mathf.Min(ammoCount, maxAmmo);
    }

        public void BurstConfetti()
    {
        if (confettiParticleSystem != null && pizzazzPurchased)
        {
            confettiParticleSystem.Play();
        }
    }


};

