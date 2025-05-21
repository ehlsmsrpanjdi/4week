
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockItem : UseItem
{
    Vector3 Direction;
    float ProjectileSpeed = 20;

    private void Start()
    {
        Direction = CharacterManager.Instance.Player.transform.forward;
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        ItemRigidBody.velocity = Direction * ProjectileSpeed;
    }
}
