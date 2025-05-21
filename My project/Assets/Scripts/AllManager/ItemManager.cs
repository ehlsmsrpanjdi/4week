using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject Rock;

    static ItemManager instance;
    public static ItemManager Instance
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
    }


    public void SpawnItem(ItemEnum _item)
    {
        switch (_item)
        {
            case ItemEnum.Rock:
                GameObject obj = MonoBehaviour.Instantiate(Rock);
                obj.transform.position = CharacterManager.Instance.Player.transform.position;
                break;
            default:
                break;
        }
    }



}
