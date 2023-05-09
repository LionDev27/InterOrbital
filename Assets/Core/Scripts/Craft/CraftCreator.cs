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

    public List<Image> requireImages;
    public List<TextMeshProUGUI> requireTexts;
    public Image craftResultImage;
    public TextMeshProUGUI itemCraftDescription;
    public GameObject gridRequires;
    
    private void SetCraft()
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {      
           requireImages[i].sprite = _itemCraft.itemsRequired[i].item.itemSprite;
           requireTexts[i].text = _itemCraft.itemsRequired[i].amountRequired.ToString();
        }

        craftResultImage.sprite =_itemCraft.itemSprite;
        itemCraftDescription.text = _itemCraft.itemDescription;
    }


    public void SetItemCraft(ItemCraftScriptableObject itemCraft)
    {
        _itemCraft = itemCraft;
        SetCraft();
    }

    
}
