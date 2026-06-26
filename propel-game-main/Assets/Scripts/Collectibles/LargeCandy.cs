using UnityEngine;

public class LargeCandy : CollectBehaviour
{
    protected override void Collect(PlayerCotroller player)
    {
        // TODO: add BOOST

        PlaySound(pickupSFX);
        Destroy(gameObject);
    }
}
