using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DangerousImage : MonoBehaviour, IUI
{
    // Start is called before the first frame update
    Image DangerousImg;

    float DangerTime = 0;

    Coroutine HitCoroutine;
    private void Awake()
    {
        DangerousImg = GetComponent<Image>();
    }

    public bool DangerUpdate()
    {
        DangerTime += Time.deltaTime;
        float colorAlpha = Mathf.LerpUnclamped(0, 0.5f, DangerTime / 3f);
        DangerousImg.color = new Color(1, 0, 0, colorAlpha);
        if (colorAlpha > 0.5f)
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

    public void OnHit()
    {
        if (HitCoroutine == null)
        {
            HitCoroutine = StartCoroutine(OnHitCoroutine());
        }
        else
        {
            StopCoroutine(HitCoroutine);
            HitCoroutine = StartCoroutine(OnHitCoroutine());
        }
    }

    IEnumerator OnHitCoroutine()
    {
        float HitTime = 0f;
        Color HurtColor = new Color(1, 0, 0, 0.5f);
        Color OriginColor = new Color(1, 0, 0, 0f);

        DangerousImg.color = HurtColor;

        while (HitTime < 1)
        {
            Color LerpColor = Vector4.Lerp(HurtColor, OriginColor, HitTime);
            DangerousImg.color = LerpColor;
            HitTime += Time.deltaTime;
            yield return null;
        }
        DangerousImg.color = OriginColor;
        yield return null;
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
