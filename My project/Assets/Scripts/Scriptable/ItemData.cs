using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "New ItemData")]
public class ItemData : ScriptableObject
{

    [SerializeField] private string itemName;

    [SerializeField] private string itemDescription;

    [SerializeField] private Sprite itemImage;

    public string ItemName { get { return itemName; } }

    public string ItemDescription { get { return itemDescription; } }

    public Sprite ItemImage { get { return itemImage; } }

}
