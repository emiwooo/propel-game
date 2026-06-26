using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Ground : MonoBehaviour
{
    public GameObject playerObject;
    public PlayerCotroller pcScript;
    public Collider2D playerCollider;
    public Collider2D groundCollider;
    public float playerY;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerY = 1;
    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = playerObject.GetComponent<Collider2D>();
        
        



    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Colliding!!!");
        playerY = 0;
        
         
        
        
        
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        playerY = 1;  
    }

}