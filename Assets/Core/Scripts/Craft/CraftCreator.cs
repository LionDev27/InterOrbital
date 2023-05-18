using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Player;
using InterOrbital.Utils;

public class CraftCreator : MonoBehaviour
{
    private ItemCraftScriptableObject _itemCraft;
    private int _amountToCraft;
    [SerializeField] private Button _craftButton;
    private CraftGrid _craftGrid;

    public List<Image> requireImages;
    public List<TextMeshProUGUI> requireTexts;
    public Image craftResultImage;
    public TextMeshProUGUI itemCraftName;
    public GameObject gridRequires;

    public TextMeshProUGUI amountToCraftText;

    private void Awake()
    {
        _craftGrid = FindObjectOfType<CraftGrid>();
    }

    private void SetCraft()
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {      
            _amountToCraft= 1;
            amountToCraftText.text = _amountToCraft.ToString();
            requireImages[i].sprite = _itemCraft.itemsRequired[i].item.itemSprite;
        }

        UpdateAmountRequired();
        craftResultImage.sprite =_itemCraft.itemSprite;
        itemCraftName.text = _itemCraft.itemName;
    }

    public void UpdateAmountRequired()
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {
            var actualAmount = PlayerComponents.Instance.Inventory.GetItemAmount(_itemCraft.itemsRequired[i].item);
            var requiredAmount = _itemCraft.itemsRequired[i].amountRequired * _amountToCraft;
            var alpha = actualAmount >= requiredAmount ? 1 : 0.5f;
            requireImages[i].ChangueAlphaColor(alpha);
            requireTexts[i].color = actualAmount >= requiredAmount ? Color.green : Color.red;
            requireTexts[i].text = actualAmount + "/" + requiredAmount;
        }

        if(PlayerComponents.Instance.Inventory.CanCraft(_itemCraft, _amountToCraft))
        {
            _craftButton.interactable = true;
        }
        else
        {
            _craftButton.interactable = false;
        }

    }
    
    public void SetItemCraftCreator(ItemCraftScriptableObject itemCraft)
    {
        _itemCraft = itemCraft;
        SetCraft();
    }

    public void IncreaseAmount()
    {
        _amountToCraft++;
        if (_amountToCraft > 99)
            _amountToCraft = 1;
        amountToCraftText.text = _amountToCraft.ToString();
        UpdateAmountRequired();
    }

    public void DecreaseAmount()
    {
        _amountToCraft--;
        if(_amountToCraft <= 0)
            _amountToCraft =99;
        
        amountToCraftText.text = _amountToCraft.ToString();
        UpdateAmountRequired();
    }

    public void GetMaxAmountToCraft()
    {
        int maxValue = 0;
        int testValue = 1;
        for(int i=0; i< 99; i++)
        {
            if (PlayerComponents.Instance.Inventory.CanCraft(_itemCraft, testValue))
            {
                maxValue=testValue;
                testValue++;
            }
            else
                break;
        }

        maxValue = maxValue == 0 ? 1 : maxValue;
        _amountToCraft = maxValue > 99 ? 99 : maxValue;
        amountToCraftText.text = maxValue.ToString();
        UpdateAmountRequired();
    }

    public void GetMinAmountToCraft()
    {
        _amountToCraft = 1;
        amountToCraftText.text =_amountToCraft.ToString();
        UpdateAmountRequired();
    }

    public void CraftItem()
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {
            PlayerComponents.Instance.Inventory.RestItems(_itemCraft.itemsRequired[i].item, _itemCraft.itemsRequired[i].amountRequired);       
        }
        UpdateAmountRequired();
        _craftGrid.UpdateFeedback();
    }
    


}
