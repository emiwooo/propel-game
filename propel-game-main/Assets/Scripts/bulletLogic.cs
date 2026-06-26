using UnityEditor;
using UnityEngine;

public class bulletLogic : MonoBehaviour
{
    public Rigidbody2D bulletRigid;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletRigid.linearVelocity = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
