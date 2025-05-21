using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class Item : MonoBehaviour, IInteractable
{

    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.ItemName}\n{data.ItemDescription}";
        return str;
    }
    public void OnInteract()
    {
        //Player 스크립트 먼저 수정
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
