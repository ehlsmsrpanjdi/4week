using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemText : MonoBehaviour, IUI
{
    //TextMeshPro textMeshPro;

    TextMeshProUGUI textMeshPro;

    public void Close()
    {
    }

    public void Init()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void Open()
    {
    }

    public void SetText(string text)
    {
        textMeshPro.SetText(text);
    }

    public void ResetText()
    {
        textMeshPro.SetText("");
    }
}
