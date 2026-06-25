using Microsoft.Extensions.DependencyInjection;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;


public class PlayerCotroller : MonoBehaviour
{

    Vector2 playerVelocity = new Vector2(0.0f, 0.0f);
    Vector2 playerAcceleration = new Vector2(0.0f, -0.1f);
    Vector2 realAcceleration = new Vector2(0.0f, 0.0f);
    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction ThrustAction;
    public InputAction ShootAction;
    public float thrustOn = 0f;
    public float thrustAllow = 5;
    public float mouseOn = 0;
    public float thrustTracker = 0;
    public float lineTracker = 0;
    //Vector2 cursorOffset = new Vector2(168f, 203f);
    public int ammoCount = 5;
    private LineRenderer lineRenderer;


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
    Vector2 acceleration = new Vector2(0f, -3f); // gravity

    if (thrustOn == 1f && thrustAllow > 0 && thrustTracker == 0)
    {
        acceleration.y = 6f; // thrust up
        thrustAllow -= 1;
        thrustTracker = 1;
    }
    else if (thrustOn == 0f)
    {
        thrustTracker = 0;
    }

    // VELOCITY
    playerVelocity += acceleration * dt;

    playerVelocity.y = Mathf.Clamp(playerVelocity.y, -5f, 5f);

    // MOVOE!!!
    Vector3 movement =
        new Vector3(moveInput.x, 0, 0) * 3f * dt;

    transform.position += (Vector3)playerVelocity * dt + movement;

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

        Debug.Log(thrustAllow);
}


};

