using UnityEngine;

public class Gun : CollectBehaviour
{
    protected override void Collect(PlayerCotroller player)
    {
        player.GunCollected();
        PlaySound(pickupSFX);
        Destroy(gameObject);
    }
}
