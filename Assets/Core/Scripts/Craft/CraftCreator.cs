using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Player;
using InterOrbital.Utils;
using InterOrbital.Spaceship;

public class CraftCreator : MonoBehaviour
{
    private ItemCraftScriptableObject _itemCraft;
    private int _amountToCraft;
    [SerializeField] private Button _craftButton;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    private CraftGrid _craftGrid;
    private CraftingItem _craftingItem;
    public List<Image> requireImages;
    public List<TextMeshProUGUI> requireTexts;
    public Image craftResultImage;
    public TextMeshProUGUI itemCraftName;
    public GameObject gridRequires;

    public TextMeshProUGUI amountToCraftText;
    public TextMeshProUGUI energyRequiredText;


    private void Awake()
    {
        _craftGrid = GetComponentInChildren<CraftGrid>();
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
        UpdateEnergyRequired();
        craftResultImage.sprite =_itemCraft.itemSprite;
        itemCraftName.text = _itemCraft.itemName;
        _descriptionText.text = _itemCraft.itemDescription;
    }

    private void UpdateAmountRequired()
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

        if(PlayerComponents.Instance.Inventory.CanCraft(_itemCraft, _amountToCraft) && HaveEnoughEnergyToCraft())
        {
            _craftButton.interactable = true;
        }
        else
        {
            _craftButton.interactable = false;
        }

    }

    private void UpdateEnergyRequired()
    {
        var energyRequired = _itemCraft.craftEnergyRequired * _amountToCraft;
        var currentSpaceshipEnergy = SpaceshipComponents.Instance.SpaceshipEnergy.GetCurrentSpaceshipEnergy();

        var colorText = energyRequired <= currentSpaceshipEnergy ? Color.green : Color.red;
        energyRequiredText.color = colorText;
        energyRequiredText.text = energyRequired + "/" + currentSpaceshipEnergy;
    }
    
    private bool HaveEnoughEnergyToCraft()
    {
        var energyRequired = _itemCraft.craftEnergyRequired * _amountToCraft;
        var currentSpaceshipEnergy = SpaceshipComponents.Instance.SpaceshipEnergy.GetCurrentSpaceshipEnergy();
        
        return energyRequired <= currentSpaceshipEnergy;
    }

    private void DecreaseEnergyOnCraft()
    {
        var energyRequired = _itemCraft.craftEnergyRequired * _amountToCraft;
        SpaceshipComponents.Instance.SpaceshipEnergy.LoseEnergy(energyRequired);
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
        UpdateEnergyRequired();
    }

    public void DecreaseAmount()
    {
        _amountToCraft--;
        if(_amountToCraft <= 0)
            _amountToCraft =99;
        
        amountToCraftText.text = _amountToCraft.ToString();
        UpdateAmountRequired();
        UpdateEnergyRequired();
    }

    public void GetMaxAmountToCraft()
    {
        int maxValue = 0;
        int testValue = 1;
        for(int i=0; i< 99; i++)
        {
            if (PlayerComponents.Instance.Inventory.CanCraft(_itemCraft, testValue) && HaveEnoughEnergyToCraft())
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
        UpdateEnergyRequired();
    }

    public void GetMinAmountToCraft()
    {
        _amountToCraft = 1;
        amountToCraftText.text =_amountToCraft.ToString();
        UpdateAmountRequired();
        UpdateEnergyRequired();
    }

    public void CraftItem(bool isFast)
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {
            PlayerComponents.Instance.Inventory.SubstractItems(_itemCraft.itemsRequired[i].item, _itemCraft.itemsRequired[i].amountRequired * _amountToCraft);       
        }
        DecreaseEnergyOnCraft();
        UpdateAmountRequired();
        UpdateEnergyRequired();
        _craftGrid.UpdateFeedback();
        if (!isFast)
        {
            _craftingItem.Craft(_itemCraft, _amountToCraft);
        }
        else
        {
            GameObject obj = Instantiate(PlayerComponents.Instance.Inventory.dropItemPrefab);
            ItemObject item = obj.AddComponent<ItemObject>();
            item.ObtainComponents();
            Destroy(obj);
            item.SetItem(_itemCraft);
            item.amount = _amountToCraft;
            PlayerComponents.Instance.Inventory.AddItem(item);
        }
    }

   

    public void SetCraftingItem(CraftingItem craftingItem)
    {
        _craftingItem = craftingItem;
    }
    


}
