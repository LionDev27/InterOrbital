using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletSlot : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _bulletImage;
    [SerializeField] private TextMeshProUGUI _amount;


    public void SetBackgroundSprite(Sprite sprite)
    {
        _backgroundImage.sprite = sprite;
    }

    public void SetBulletSprite(Sprite sprite)
    {
        _bulletImage.sprite = sprite;
    }

    public void SetBulletAsNoSelected()
    {
        _bulletImage.color = new Color(1, 1, 1, 0.5f);
    }

    public void SetBulletAsSelected()
    {
        _bulletImage.color = new Color(1, 1, 1, 1);
    }


    public void SetBulletAmount(int amount)
    {
        if(amount == 0) 
        {
            _amount.text = "";
            _bulletImage.color = new Color(1,1,1,0.5f);
        }
        else
        {
            _amount.text = amount.ToString();
            _bulletImage.color = Color.white;
        }
    }
}
