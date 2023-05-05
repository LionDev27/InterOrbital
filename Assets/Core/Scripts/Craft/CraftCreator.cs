using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Player;

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
            var actualAmount = PlayerComponents.Instance.Inventory.GetItemAmount(_itemCraft.itemsRequired[i].item);
            var requiredAmount = _itemCraft.itemsRequired[i].amountRequired;
            requireImages[i].sprite = _itemCraft.itemsRequired[i].item.itemSprite;
            var imageColor = requireImages[i].color;
            imageColor.a = actualAmount >= requiredAmount ? 1 : 0.5f;
            requireImages[i].color = imageColor;
            requireTexts[i].color = actualAmount >= requiredAmount ? Color.green : Color.red;
            requireTexts[i].text = actualAmount + "/" + requiredAmount;
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
