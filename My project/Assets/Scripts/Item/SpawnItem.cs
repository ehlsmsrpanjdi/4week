using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    SphereCollider itemCollider;

    [SerializeField] ItemData itemData;

    public ItemData _ItemData { get => itemData; }

    void Awake()
    {
        itemCollider = GetComponent<SphereCollider>();
        itemCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == true)
        {
            if (true == UIManager.Instance.GetItem(itemData))
            {
                Destroy(this.gameObject);
            }
        }
    }

}
