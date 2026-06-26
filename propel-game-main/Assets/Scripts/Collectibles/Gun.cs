using UnityEngine;

public class Gun : CollectBehaviour
{
    protected override void Collect(PlayerCotroller player)
    {
        player.hasGun = true;
        PlaySound(pickupSFX);
        Destroy(gameObject);
    }
}
