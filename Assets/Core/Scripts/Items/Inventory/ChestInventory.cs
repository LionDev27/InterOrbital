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
        _itemsSlot = chestAux._itemsSlot;
        _itemsSlotBackGround = chestAux._itemsSlotBackGround;
        _textAmount = chestAux._textAmount;

    }

   




}
