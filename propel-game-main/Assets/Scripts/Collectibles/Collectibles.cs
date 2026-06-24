using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CollectBehaviour : MonoBehaviour
{
    public GameObject playerObject;
    public Collider playerCollider;
    public Collider collectCollider;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = playerObject.GetComponent<Collider>();

    }

    void OnTriggerEnter(Collider playerCollider)
    {
        //Debug.Log("Colliding!!!");

    }

}