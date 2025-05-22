using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DangerousImage : MonoBehaviour ,IUI
{
    // Start is called before the first frame update
    Image DangerousImg;

    float DangerTime = 0;

    private void Awake()
    {
        DangerousImg = GetComponent<Image>();
    }

    public bool DangerUpdate()
    {
        DangerTime += Time.deltaTime;
        float colorAlpha = Mathf.LerpUnclamped(0, 0.5f, DangerTime / 3f);
        DangerousImg.color = new Color(1, 0, 0, colorAlpha);
        if(colorAlpha > 0.5f)
        {
            return true;
        }
        return false;
    }

    public void DangerReset()
    {
        DangerTime = 0f;
        DangerousImg.color = new Color(1, 0, 0, 0f);
    }

    void IUI.Init()
    {
    }

    void IUI.Open()
    {
    }

    void IUI.Close()
    {
    }
}
