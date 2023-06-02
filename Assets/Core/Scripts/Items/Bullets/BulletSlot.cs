using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletSlot : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image bulletImage;
    [SerializeField] private TextMeshProUGUI amount;


    public void SetBackgroundSprite(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public void SetBulletSprite(Sprite sprite) 
    {
        bulletImage.sprite = sprite;
    }
}
