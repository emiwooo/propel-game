using UnityEngine;

public class LargeCandy : CollectBehaviour
{
    protected override void Collect(PlayerCotroller player)
    {
        player.ActivateBoost();
        PlaySound(pickupSFX);
        Destroy(gameObject);
        player.candyCollected++;
    }
}
