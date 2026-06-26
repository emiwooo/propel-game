using UnityEngine;

public class SunSpin : MonoBehaviour
{
    public int rotationSpeed = 10; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            transform.Rotate (new Vector3 (0, 0, rotationSpeed) * Time.deltaTime);
        }
        
    }
}
