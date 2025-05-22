using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    CapsuleCollider objectCollider;

    private void Awake()
    {
        objectCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerController>().JumpFunction(300f);
    }

}
