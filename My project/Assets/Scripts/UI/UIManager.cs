using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IUI
{
    public void Init();
    public void Open();
    public void Close();
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }

    InventroyUI Inventory;
    ItemText itemText;
    DangerousImage dangerImg;

    List<IUI> UIList = new List<IUI>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(this);
        }
        Inventory = GetComponentInChildren<InventroyUI>();
        itemText = GetComponentInChildren<ItemText>();
        dangerImg = GetComponentInChildren<DangerousImage>();
        
        UIList.Add(Inventory);
        UIList.Add(itemText);
        UIList.Add(dangerImg);
    }


    public void Start()
    {
        foreach (IUI ui in UIList)
        {
            ui.Init();
        }
    }

    public bool GetItem(ItemData _itemData)
    {
        return Inventory.GetItem(_itemData);
    }

    public void SetText(ItemData _itemDat)
    {
        itemText.SetText(_itemDat.ItemText);
    }

    public void ResetText()
    {
        itemText.ResetText();
    }

    public void ItemUse(int _itemIndex)
    {
        Inventory.ItemUse(_itemIndex - 1);
    }

    public bool DangerousUpdate()
    {
        return dangerImg.DangerUpdate();
    }

    public void DangerousReset()
    {
        dangerImg.DangerReset();
    }

}
