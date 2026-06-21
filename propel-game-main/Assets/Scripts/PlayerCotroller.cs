using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class PlayerCotroller : MonoBehaviour
{

    Vector2 playerVelocity = new Vector2(0.0f, 0.0f);
    Vector2 playerAcceleration = new Vector2(0.0f, 0.0f);
   
    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction ThrustAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        LookAction.Enable();
        ThrustAction.Enable();
        

    }

    
    // Update is called once per frame
    void Update()
    {

        
        Vector2 playerMovement = MoveAction.ReadValue<Vector2>()*0.1f;
        Vector2 playerPosition = (Vector2)transform.position + playerMovement + playerVelocity;
        Vector2 playerCursor = LookAction.ReadValue<Vector2>();
        float thrustOn = ThrustAction.ReadValue<float>();


        if ((thrustOn > 0.0f) && (playerCursor.x > 160) && (playerCursor.x < 170) && (playerCursor.y > 200) && (playerCursor.y < 220))
        {
            playerAcceleration = new Vector2(0.0f, +0.02f);
        
        }
        else
        {
            playerAcceleration = new Vector2(0.0f, -0.01f);
        }

        transform.position = playerPosition + playerAcceleration;
        playerVelocity = playerVelocity + playerAcceleration;
        playerVelocity.y = Math.Clamp(playerVelocity.y, -0.05f, +0.1f);
        
      
       

       
   



      
        
    }


};

