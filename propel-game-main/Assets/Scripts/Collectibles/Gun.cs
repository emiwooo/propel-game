using UnityEngine;

public class Gun : CollectBehaviour
{
    protected override void Collect(PlayerCotroller player)
    {
        // TODO: enable gun for player

        PlaySound(pickupSFX);
        Destroy(gameObject);
    }
}
