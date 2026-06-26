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
    public float maxThrust = 5f; // can be upgraded in shop
    public float thrustAllow = 5;


    public float mouseOn = 0;
    public float thrustTracker = 0;
    public float lineTracker = 0;
    //Vector2 cursorOffset = new Vector2(168f, 203f);
    public int ammoCount = 5;
    private LineRenderer lineRenderer;
    public Ground groundControl;
    public Vector3 transTotal;

    // keep track of max altitude reached
    public float currentHeight = 0;
    public float currentMaxHeight = 0; // for this run specifically
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
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

        // INPUT
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();

        mouseOn = ShootAction.ReadValue<float>();
        thrustOn = ThrustAction.ReadValue<float>();

        // ACCELERATION
        Vector2 acceleration = new Vector2(0f, -0.001f); // gravity

        if (thrustOn == 1f && thrustAllow > 0 && thrustTracker == 0)
        {
            acceleration.y = 0.3f; // thrust up
            thrustAllow -= 1;
            thrustTracker = 1;
        }
        else if (thrustOn == 0f)
        {
            thrustTracker = 0;
        }

        // VELOCITY
        playerVelocity += acceleration;
        

        playerVelocity.y = Mathf.Clamp(playerVelocity.y, -3f, 10f);
            
            // MOVOE!!!
            Vector3 movement =
            new Vector3(moveInput.x, 0, 0) * 3f * dt;

        if (groundControl.playerY == 0f)
            {
                playerVelocity.y = Mathf.Clamp(playerVelocity.y, 0f, 10f);
            }
        
        transTotal = (Vector3)playerVelocity + movement;
        transform.position += (Vector3)playerVelocity + movement;

        // LINE RENDERER
        if (mouseOn == 1f)
        {
            lineRenderer.enabled = true;

            Vector2 mouseScreenPos = LookAction.ReadValue<Vector2>();

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


        // for testing
        if (thrustAllow == 0)
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


};

