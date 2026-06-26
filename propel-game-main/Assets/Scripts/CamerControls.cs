using UnityEngine;

public class CamerControls : MonoBehaviour
{
    public PlayerCotroller playerScript;
    public float camerY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        camerY = playerScript.transTotal.y;
        transform.position += new Vector3(0, camerY, 0);
        
        
    }
}
