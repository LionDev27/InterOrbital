using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftCreator : MonoBehaviour
{
    private ItemCraftScriptableObject _itemCraft;

    private Image[] _requireImages;
    public List<TextMeshProUGUI> requireTexts;

    public GameObject gridRequires;


    private void Start()
    {
        InitImages();
    }

    private void InitImages()
    {
        _requireImages =new Image[4];
        for(int i=0; i< _requireImages.Length; i++)
        {
            _requireImages[i] = gridRequires.transform.GetChild(i).GetChild
        }
    }

    private void SetCraft()
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {
            _requireImages[i].sprite = _itemCraft.itemsRequired[i].item.itemSprite;
            _requireImages[i].color = Color.red;
        }
    }


    public void SetItemCraft(ItemCraftScriptableObject itemCraft)
    {
        _itemCraft = itemCraft;
    }

    
}
