using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    protected SphereCollider ItemColider;
    protected Rigidbody ItemRigidBody;

    protected void Awake()
    {
        ItemColider = GetComponent<SphereCollider>();
        ItemRigidBody = GetComponent<Rigidbody>();
    }

}
