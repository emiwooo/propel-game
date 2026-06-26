using UnityEditor;
using UnityEngine;

public class SmallCandy : CollectBehaviour
{
//    [SerializeField] private PlayerCotroller pcScript;
//    private void OnTriggerEnter(Collider playerCollider)
//    {
//        pcScript.thrustAllow += 1;
//    }

    protected override void Collect(PlayerCotroller player)
    {
        // Increase the player's thrust allowance by 1
        player.thrustAllow += 1;

        PlaySound(pickupSFX);
        Destroy(gameObject);
    }
}
