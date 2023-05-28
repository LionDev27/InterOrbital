using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Player;
using InterOrbital.Utils;
using UnityEngine.UI;
using TMPro;

public class ChestInventory : Inventory
{
    [SerializeField] private GameObject _gridChest;
    [SerializeField] private GameObject _inventorySlot;
    [SerializeField] private SizeChest _size;
    

    private void Start()
    {
        InitSlots();
        StartInventory();
    }
   
    public void SetChest(ChestInventory chestAux)
    {

    }

    public void SetInventory()
    {
       
        _itemsSlot = new Image[_totalNumberOfSlots];
        _itemsSlotBackGround = new Image[_totalNumberOfSlots];
        _textAmount = new TextMeshProUGUI[_totalNumberOfSlots];

        for(int i=0; i < _totalNumberOfSlots; i++)
        {
            GameObject invSlot = Instantiate(_inventorySlot);
            invSlot.transform.SetParent(_gridChest.transform);
           
            _itemsSlot[i] = invSlot.transform.GetChild(0).GetComponent<Image>();
            _itemsSlotBackGround[i] = invSlot.transform.GetComponent<Image>();
            invSlot.transform.GetChild(0).GetComponent<DraggableItem>().inventoryIndex = i;
            _textAmount[i] = invSlot.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            
        }

        _backgroundDefaultImage = _itemsSlotBackGround[1].sprite;
    }




}
