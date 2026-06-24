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
    Vector2 cursorOffset = new Vector2(168f, 203f);

    
   




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        LookAction.Enable();
        ThrustAction.Enable();
        ShootAction.Enable();
        

    }

    
    // Update is called once per frame
    void Update()
    {

        
        Vector2 playerMovement = MoveAction.ReadValue<Vector2>()*0.3f;
        Vector2 playerPosition = (Vector2)transform.position + playerMovement + playerVelocity;
        Vector2 playerCursor = LookAction.ReadValue<Vector2>() - cursorOffset;
        mouseOn = ShootAction.ReadValue<float>();
        thrustOn = ThrustAction.ReadValue<float>();
        playerAcceleration = new Vector2(0.0f, -0.01f);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 4;
        lineRenderer.startColor = Color.purple;
        Vector2 lineOrigin = transform.position;
        



        if ((thrustOn == 1.0f) && (thrustAllow > 0) && (thrustTracker == 0))
        {
            playerAcceleration = new Vector2(0.0f, 5f);
            thrustAllow -= 1;
            thrustTracker = 1;
            


        }
        else if (thrustOn == 0f)
        {
            thrustTracker = 0;
            playerAcceleration = new Vector2(0.0f, -0.1f);

        }
        
        if (mouseOn == 1.0f)
        {
            
            lineRenderer.SetPosition(0, lineOrigin);
            lineRenderer.SetPosition(1, lineOrigin + playerCursor);
            
        }

        realAcceleration += playerAcceleration;
        
        realAcceleration.y = Math.Clamp(realAcceleration.y, -0.2f, 5f);




        transform.position = playerPosition + realAcceleration;
        playerVelocity = playerVelocity + playerAcceleration;
        playerVelocity.y = Math.Clamp(playerVelocity.y, -0.05f, +0.1f);
        Debug.Log(playerCursor);
        //Debug.Log(mouseOn);
        //Debug.Log(thrustAllow);
        
        
      
       

       
   



      
        
    }


};

