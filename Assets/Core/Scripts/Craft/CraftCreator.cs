using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftCreator : MonoBehaviour
{
    public List<Image> requireImages;
    public List<TextMeshProUGUI> requireTexts;
    
    public void SelectCraft(ItemCraftScriptableObject itemCraft)
    {
        for(int i=0; i< itemCraft.itemsRequired.Count; i++)
        {
            requireImages[i].sprite = itemCraft.itemsRequired[i].item.itemSprite;
        }
    }

    
}
