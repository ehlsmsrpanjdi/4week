using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour, IUI
{
    [SerializeField] Image _image;

    public delegate void OnHealthChange(float _Damage);
    public event OnHealthChange HealthChange;

    public void OnHealthUpdate(float _percent)
    {
        HealthChange.Invoke(_percent);
    }

    public void Init()
    {
        HealthChange += (float _percent) =>
        {
            _image.fillAmount = _percent;
        };
    }

    public void Open()
    {
    }

    public void Close()
    {
    }
}
