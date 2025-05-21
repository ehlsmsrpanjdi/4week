using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemEnum
{
    Rock = 1,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "CreateItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] Sprite itemImg;
    [SerializeField] string itemText;
    [SerializeField] ItemEnum itemEnum;

    public Sprite ItemImg { get => itemImg; }

    public string ItemText { get => itemText; }

    public ItemEnum ItemEnum { get => itemEnum; }
}
