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
    public float thrustAllow = 5;
    public float mouseOn = 0;
    public float thrustTracker = 0;
   




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
        mouseOn = ThrustAction.ReadValue<float>();
        playerAcceleration = new Vector2(0.0f, -0.01f); 
       


        if ((mouseOn == 1.0f) && (playerCursor.x > 145) && (playerCursor.x < 190) && (playerCursor.y > 167) && (playerCursor.y < 243) && (thrustAllow > 0) && (thrustTracker == 0))
        {
            playerAcceleration = new Vector2(0.0f, 5f);
            thrustAllow -= 1;
            thrustTracker = 1;
            


        }
        else if (mouseOn == 0f)
        {
            thrustTracker = 0;
            playerAcceleration = new Vector2(0.0f, -0.1f);

        }
        else if (mouseOn == 1f)
        {



        }

        realAcceleration += playerAcceleration;
        
        realAcceleration.y = Math.Clamp(realAcceleration.y, -0.2f, 5f);




        transform.position = playerPosition + realAcceleration;
        playerVelocity = playerVelocity + playerAcceleration;
        playerVelocity.y = Math.Clamp(playerVelocity.y, -0.05f, +0.1f);
        Debug.Log(playerCursor);
        Debug.Log(mouseOn);
        Debug.Log(thrustAllow);
      
       

       
   



      
        
    }


};

