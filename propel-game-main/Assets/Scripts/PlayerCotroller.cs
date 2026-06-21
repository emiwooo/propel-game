using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCotroller : MonoBehaviour
{

    public InputAction MoveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        

    }

    
    // Update is called once per frame
    void Update()
    {


        Vector2 playerVelocity = MoveAction.ReadValue<Vector2>();
        Vector2 playerPosition = (Vector2)transform.position + playerVelocity;
        transform.position = playerPosition;

        Debug.Log(playerPosition);

       
   



      
        
    }


};

