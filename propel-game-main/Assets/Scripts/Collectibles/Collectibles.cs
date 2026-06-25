using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CollectBehaviour : MonoBehaviour
{
    public GameObject playerObject;
    public Collider2D playerCollider;
    public Collider2D collectCollider;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = playerObject.GetComponent<Collider2D>();

    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Colliding!!!");

    }

}