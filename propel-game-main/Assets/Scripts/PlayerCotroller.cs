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

    class Global
    {

        public static Vector2 initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {



        //I create a movement vector based on player movement
        Vector2 move = MoveAction.ReadValue<Vector2>();

        //I check for current position
        Vector2 initialPosition = (Vector2)transform.position;
       

        //I add it to the current position and store it in a Vector2 variable called position
        Vector2 position = (Vector2)transform.position + move;

        //I calculate velocity by subtracting my initial position from my final position and dividing by the time it took
        Vector2 velocity = (position - initialPosition) / Time.deltaTime;

        //I calculate acceleration
        Vector2 positionAcceleration = (velocity - Global.initialVelocity) / Time.deltaTime;

        //I create an initialVelocity variable
        Global.initialVelocity = (position - initialPosition) / Time.deltaTime;
      
        //I change the final position to the previous variable
        transform.position = position + 1 * 1/2 * positionAcceleration * Time.deltaTime * Time.deltaTime;


       // Debug.Log(velocity);
        Debug.Log(Global.initialVelocity);
        // Debug.Log(positionAcceleration);
       
   



      
        
    }


};

