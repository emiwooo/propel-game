using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;
    
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        // Automatically find the SpriteRenderer if not assigned
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.Log("no sprite renderer found for flash damage script");
            
        originalColor = spriteRenderer.color;
    }

    public void CallFlash()
    {
        // Stop any ongoing flash so they don't stack awkwardly
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}