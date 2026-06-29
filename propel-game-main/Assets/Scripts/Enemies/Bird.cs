using UnityEngine;

public class Bird : Enemy
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Die()
    {
        base.Die();
        player.birdsKilled++;
    }
}