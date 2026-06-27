using UnityEngine;

public class CamerControls : MonoBehaviour
{
    public PlayerCotroller playerScript;
    public float camerY;
    [SerializeField] public float verticalOffset;
    public float minCameraY = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float targetY = playerScript.transform.position.y + verticalOffset;

        targetY = Mathf.Max(targetY, minCameraY);

        Vector3 pos = transform.position;
        pos.y = targetY;
        transform.position = pos;

        if (transform.position.y <= minCameraY && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.GameOver();    
        }
    }
}
