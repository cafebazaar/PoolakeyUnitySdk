using System;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private Image gasSlider;
    [SerializeField] private Sprite[] gasSprites;

    [SerializeField] private Image skinImage;
    [SerializeField] private Sprite[] skinSprites;


    public int gas
    {
        get => PlayerPrefs.GetInt("gas", 4);
    }
    public int skin = 0;

    void Start()
    {
        SetGas(Mathf.Min(gas, 4));
        SetSkin(skin);
    }

    public void Drive()
    {
        if (gas == 5)
        {
            return;
        }
        SetGas(Mathf.Max(0, gas - 1));
    }

    public void Increase()
    {
        if (gas == 5)
        {
            return;
        }
        SetGas(Mathf.Min(gas + 1, 4));
    }

    public void SetGas(int gas)
    {
        PlayerPrefs.SetInt("gas", gas);
        gasSlider.sprite = gasSprites[gas];
    }

    internal void SetSkin(int skin)
    {
        skinImage.sprite = skinSprites[skin];
    }
}
