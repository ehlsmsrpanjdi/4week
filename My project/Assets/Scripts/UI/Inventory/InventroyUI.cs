using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventroyUI : MonoBehaviour, IUI
{
    Image[] Images;
    ItemData[] itemDatas;

    int ImageCount { get; set; }

    public void Close()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        Images = GetComponentsInChildren<Image>();
        ImageCount = Images.Length;
        itemDatas = new ItemData[ImageCount];
    }

    public void Open()
    {
    }

    public bool GetItem(ItemData _itemData)
    {
        for(int itemindex = 0; itemindex < Images.Length; ++itemindex)
        {
            if (null == Images[itemindex].sprite)
            {
                Images[itemindex].sprite = _itemData.ItemImg;
                Images[itemindex].color = new Vector4(255, 255, 255, 200);
                itemDatas[itemindex] = _itemData;
                return true;
            }
        }
        return false;
    }

    public void ItemUse(int _itemIndex)
    {
        if (itemDatas[_itemIndex] != null)
        {
            ItemEnum enumValue = itemDatas[_itemIndex].ItemEnum;
            ItemManager.Instance.SpawnItem(enumValue);
            itemDatas[_itemIndex] = null;
            Images[_itemIndex].sprite = null;
            Images[_itemIndex].color = new Vector4(0, 0, 0, 0.3f);
        }
    }
}
