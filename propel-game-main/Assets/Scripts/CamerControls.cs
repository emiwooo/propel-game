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
    void LateUpdate()
    {

//        camerY = playerScript.transTotal.y;
//        transform.position += new Vector3(0, camerY, 0);
        
        Vector3 pos = transform.position;
        pos.y = playerScript.transform.position.y;
        transform.position = pos;
        
    }
}
