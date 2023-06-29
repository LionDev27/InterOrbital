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
        if (_itemCraft.itemsRequired.Count != requireImages.Count)
        {
            for (int i = _itemCraft.itemsRequired.Count; i< requireImages.Count; i++)
            {
                requireImages[i].sprite = PlayerComponents.Instance.Inventory.itemVoid.itemSprite;
            }
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
            var actualAmount = PlayerComponents.Instance.Inventory.GetTotalItemAmount(_itemCraft.itemsRequired[i].item);
            var requiredAmount = _itemCraft.itemsRequired[i].amountRequired * _amountToCraft;
            var alpha = actualAmount >= requiredAmount ? 1 : 0.5f;
            requireImages[i].ChangueAlphaColor(alpha);
            requireTexts[i].color = actualAmount >= requiredAmount ? Color.green : Color.red;
            requireTexts[i].text = actualAmount + "/" + requiredAmount;
        }

        if (_itemCraft.itemsRequired.Count != requireImages.Count)
        {
            for (int i = _itemCraft.itemsRequired.Count; i < requireImages.Count; i++)
            {
                requireTexts[i].text = "";
            }
        }

        if (PlayerComponents.Instance.Inventory.CanCraft(_itemCraft, _amountToCraft) && HaveEnoughEnergyToCraft(GetCorrespondingEnergy()))
        {
            _craftButton.interactable = true;
        }
        else
        {
            _craftButton.interactable = false;
        }

    }


    private int GetCorrespondingEnergy()
    {
        if (_craftingItem is CraftingTable)
        {
            return SpaceshipComponents.Instance.SpaceshipEnergy.GetCurrentSpaceshipEnergy();
        }
        else
        {
            return PlayerComponents.Instance.PlayerEnergy.GetCurrentPlayerEnergy();
        }
    }

    private void UpdateEnergyRequired()
    {
        var energyRequired = _itemCraft.craftEnergyRequired * _amountToCraft;
        var currentEnergy = GetCorrespondingEnergy();
        

        var colorText = HaveEnoughEnergyToCraft(currentEnergy) ? Color.green : Color.red;
        energyRequiredText.color = colorText;
        energyRequiredText.text = currentEnergy + "/" + energyRequired;
    }
    
    private bool HaveEnoughEnergyToCraft(int currentEnergy)
    {
        var energyRequired = _itemCraft.craftEnergyRequired * _amountToCraft;
        
        return energyRequired <= currentEnergy;
    }

    private void DecreaseEnergyOnCraft()
    {
        var energyRequired = _itemCraft.craftEnergyRequired * _amountToCraft;
        if (_craftingItem is CraftingTable)
        {
            SpaceshipComponents.Instance.SpaceshipEnergy.LoseEnergy(energyRequired);
        }
        else
        {
            PlayerComponents.Instance.PlayerEnergy.LoseEnergy(energyRequired);
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
            if (PlayerComponents.Instance.Inventory.CanCraft(_itemCraft, testValue) && HaveEnoughEnergyToCraft(GetCorrespondingEnergy()))
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

    public void CraftItem(bool isPlayerCraft)
    {
        for (int i = 0; i < _itemCraft.itemsRequired.Count; i++)
        {
            PlayerComponents.Instance.Inventory.SubstractItems(_itemCraft.itemsRequired[i].item, _itemCraft.itemsRequired[i].amountRequired * _amountToCraft);       
        }
        DecreaseEnergyOnCraft();
        UpdateAmountRequired();
        UpdateEnergyRequired();
        _craftGrid.UpdateFeedback();
        if (!isPlayerCraft)
        {
            _craftingItem.Craft(_itemCraft, _amountToCraft);
        }
        else
        {
            for (int i = 0; i < _itemCraft.amountToCraft; i++)
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
    }

   

    public void SetCraftingItem(CraftingItem craftingItem)
    {
        _craftingItem = craftingItem;
    }
    


}
