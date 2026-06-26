using UnityEngine;

public class LargeCandy : CollectBehaviour
{
    protected override void Collect(PlayerCotroller player)
    {
        player.activateBoost();
        PlaySound(pickupSFX);
        Destroy(gameObject);
    }
}
