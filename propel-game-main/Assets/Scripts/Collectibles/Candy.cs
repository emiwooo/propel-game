using UnityEditor;
using UnityEngine;

public class Candy : CollectBehaviour
{



    public PlayerCotroller pcScript;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

       
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        pcScript.thrustAllow += 1;
    }
}
